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

    void Start() {
        this.UpdateAsObservable()
            .Where(stream => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND)
            .Subscribe(stream => PlayerIdle());

        this.UpdateAsObservable()
            .Where(stream => Input.GetKeyDown(KeyCode.Space) && playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND)
            .Subscribe(stream => PlayerJump());

        this.FixedUpdateAsObservable()
            .Where(stream => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            .Subscribe(stream => {
                if (Input.GetKey(KeyCode.LeftShift) && gameManager.GetCollidedWithWallCount() == 0) {
                    PlayerDash();
                }
                else {
                    PlayerWalk();
                }
            });

        this.UpdateAsObservable()
            .Where(stream => playerStatus == (int)StaticValues.PLAYER_STATUS.JUMPED && IsPlayerLanded())
            .Subscribe(stream => playerStatus = (int)StaticValues.PLAYER_STATUS.ON_GROUND);

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
    /// 
    /// 아이들
    /// </summary>
    public void PlayerIdle() {
        switch (characterInfo.id) {
            case (int)StaticValues.PLAYABLE_CHARACTER.MOMOI:
                AnimationControll((int)StaticValues.ANIMATION_NUMBER.IDLE);
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
    }

    /// <summary>
    /// 대쉬
    /// </summary>
    public void PlayerDash() {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.DASH : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        PlayerMove(StaticValues.DASH_SPEED, animationNum);
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
    /// 공격
    /// </summary>
    public void PlayerAttack() {
        switch (characterInfo.id) {
            case 1:
                break;

            default:

                break;
        }
    }

    /// <summary>
    /// 착지했는지 확인
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

}
