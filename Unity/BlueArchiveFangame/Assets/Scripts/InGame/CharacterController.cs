using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class CharacterController : MonoBehaviour {
    // TODO: 플레이어 상태 FSM으로 변경

    [SerializeField]
    private Animator momoiAnimator;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private GameManager gameManager;

    private Rigidbody playerRigidBody;
    private CharacterInfo characterInfo;

    private int playerStatus;
    private int playerMovingStatus;

    void Start() {
        // 아이들
        this.UpdateAsObservable()
            .Where(stream => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND)
            .Subscribe(stream => PlayerIdle());

        this.UpdateAsObservable()
            .Subscribe(stream => {
                switch (playerStatus)
                {
                        case (int)StaticValues.PLAYER_STATUS.ON_GROUND:
                            // 점프
                            if (Input.GetKeyDown(KeyCode.Space)) {
                                PlayerJump(); 
                            }

                            // 걷기, 달리기
                            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {

                                if (Input.GetKey(KeyCode.LeftShift) && gameManager.GetCollidedWithWallCount() == 0) {
                                    PlayerDash();
                                }
                                else {
                                    PlayerWalk();
                                }
                            }

                            // 연속공격
                            if (Input.GetKeyDown(KeyCode.Z) && playerMovingStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND) {
                                FirstAttackCombo();
                            }
                            break;

                        case (int)StaticValues.PLAYER_STATUS.JUMPED:
                            playerStatus = IsPlayerLanded() ? (int)StaticValues.PLAYER_STATUS.ON_GROUND : playerStatus ;
                            break;

                        case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST:
                            if (Input.GetKeyDown(KeyCode.Z)) {
                                // 2차 연속공격
                                playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND;
                            }
                            break;

                        case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND:
                            if (Input.GetKeyDown(KeyCode.Z))
                            {
                                // 3차 연속공격
                                playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD;
                            }
                            break;

                        case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD:
                            break;
                }
            });

        // 중력 추가
        this.FixedUpdateAsObservable()
            .Subscribe(stream => ResetGravity());
    }

    /// <summary>
    /// 플레이어정보 초기화
    /// </summary>
    /// <param name="gameObject"> 플레리어 오브젝트 </param>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void InitializePlayer(GameObject gameObject, CharacterInfo info) {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        characterInfo = info;
    }

    /// <summary>
    /// 애니메이션 변경 메소드
    /// </summary>
    /// <param name="animationNum"> 현재 재생해야하는 애니메이션 </param>
    private void AnimationControll(int animationNum) {
        switch (characterInfo.id) {
            case (int)StaticValues.PLAYABLE_CHARACTER.MOMOI:
                if (momoiAnimator.GetInteger("animationNum") != animationNum) {
                    momoiAnimator.SetInteger("animationNum", animationNum);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 아이들
    /// </summary>
    public void PlayerIdle() {
        switch (characterInfo.id) {
            case (int)StaticValues.PLAYABLE_CHARACTER.MOMOI:
                AnimationControll((int)StaticValues.ANIMATION_NUMBER.IDLE);
                playerMovingStatus = (int)StaticValues.PLAYER_STATUS.ON_GROUND;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 걷기
    /// </summary>
    public void PlayerWalk() {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.WALK : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        PlayerMove(StaticValues.WALK_SPEED, animationNum);
        playerMovingStatus = (int)StaticValues.PLAYER_STATUS.WALK;
    }

    /// <summary>
    /// 대쉬
    /// </summary>
    public void PlayerDash() {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.DASH : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        PlayerMove(StaticValues.DASH_SPEED, animationNum);
        playerMovingStatus = (int)StaticValues.PLAYER_STATUS.DASH;
    }

    public void CheckCurrentStatusBeforeMoving()
    {
        if(playerStatus != (int)StaticValues.PLAYER_STATUS.ON_GROUND || playerStatus != (int)StaticValues.PLAYER_STATUS.JUMPED) {
            playerStatus = IsPlayerLanded() ? (int)StaticValues.PLAYER_STATUS.ON_GROUND : (int)StaticValues.PLAYER_STATUS.JUMPED;
        }
    }

    /// <summary>
    /// 캐릭터를 움직이는 메소드
    /// </summary>
    /// <param name="speed"> 속도 </param>
    private void PlayerMove(float speed, int animationNum) {
        AnimationControll(animationNum);

        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        if (moveDirX < 0) {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (moveDirX > 0) {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        playerRigidBody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    /// <summary>
    /// 점프
    /// </summary>
    public void PlayerJump() {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.JUMP);

        playerRigidBody.AddForce(Vector3.up * StaticValues.JUMP_AMMOUNT, ForceMode.Impulse);
        playerStatus = (int)StaticValues.PLAYER_STATUS.JUMPED;
    }

    /// <summary>
    /// 첫번째 연속공격
    /// </summary>
    public void FirstAttackCombo() {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_FIRST);
        playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST;
    }

    /// <summary>
    /// 두번째 연속공격
    /// </summary>
    public void SecondAttackCombo() {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_SECOND);
    }

    /// <summary>
    /// 세번째 연속공격
    /// </summary>
    public void ThirdAttackCombo() {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_THIRD);
    }

    /// <summary>
    /// 땅에 착지했는지 확인
    /// </summary>
    /// <returns> 착지한 상태인가 </returns>
    public bool IsPlayerLanded() {
        var distToGround = gameObject.GetComponent<BoxCollider>().bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    /// <summary>
    /// 중력 추가 메소드
    /// </summary>
    private void ResetGravity() {
        playerRigidBody.AddForce(Physics.gravity* StaticValues.GRAVITY_SCALE * playerRigidBody.mass);
    }


    public void SetPlayerStatus(int newStatus)
    {
        playerStatus = newStatus;
    }

    public int GetPlayerStatus()
    {
        return playerStatus;
    }
}
