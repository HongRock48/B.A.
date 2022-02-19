using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class PlayerController : MonoBehaviour {
    // TODO: 플레이어 상태 FSM으로 변경

    [SerializeField]
    private Animator momoiAnimator;
    [SerializeField]
    private CameraController cameraController;

    private Rigidbody playerRigidBody;

    private bool isPlayerJupmed = false;        // FSM으로 바꾸면 삭제해도 될듯

    /// <summary>
    /// 애니메이션 변경 메소드
    /// </summary>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    /// <param name="animationNum"> 현재 재생해야하는 애니메이션 </param>
    private void AnimationControll(CharacterInfo info, int animationNum) {
        switch (info.id) {
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
    /// 플레이어정보 초기화
    /// </summary>
    /// <param name="gameObject"> 플레리어 오브젝트 </param>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void InitializePlayer(GameObject gameObject, CharacterInfo info) {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void PlayerIdle(CharacterInfo info) {
        switch (info.id)
        {
            case (int)StaticValues.PLAYABLE_CHARACTER.MOMOI:
                AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.IDLE);
                break;

            default:

                break;
        }
    }

    /// <summary>
    /// 걷기
    /// </summary>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void PlayerWalk(CharacterInfo info) {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.WALK);
        PlayerMove(StaticValues.WALK_SPEED);
    }

    /// <summary>
    /// 대쉬
    /// </summary>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void PlayerDash(CharacterInfo info) {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.DASH);
        PlayerMove(StaticValues.DASH_SPEED);
    }

    /// <summary>
    /// 점프
    /// </summary>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void PlayerJump(CharacterInfo info)
    {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.JUMP);

        playerRigidBody.AddForce(Vector3.up * StaticValues.JUMP_AMMOUNT, ForceMode.Impulse);
        isPlayerJupmed = true;
    }

    /// <summary>
    /// 캐릭터를 움직이는 메소드
    /// </summary>
    /// <param name="speed"> 속도 </param>
    private void PlayerMove(float speed) {
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
    /// 공격
    /// </summary>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void PlayerAttack(CharacterInfo info) {
        switch (info.id) {
            case 1:
                break;

            default:

                break;
        }
    }

    /// <summary>
    /// 착지했는지 확인
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerLanded() {
        var distToGround = gameObject.GetComponent<BoxCollider>().bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void ResetGravity() {
        playerRigidBody.AddForce(Physics.gravity* StaticValues.GRAVITY_SCALE * playerRigidBody.mass);
    }

    void Start() {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        JsonReader jsonReader = new JsonReader();
        var resourcePath = "Assets/Resources/Data";


        // TODO: 캐릭터 정보 밖으로 뺄것 (CharacterInfo Momoi = new CharacterInfo();) 등
        CharacterInfo Momoi = new CharacterInfo();
        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");

        this.FixedUpdateAsObservable()
            .Subscribe(stream => ResetGravity());

        this.FixedUpdateAsObservable()
            .Where(stream => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            .Subscribe(stream => {
                if (Input.GetKey(KeyCode.LeftShift) && gameManager.GetCollidedWithWallCount() == 0) {
                    PlayerDash(Momoi);
                }
                else {
                    PlayerWalk(Momoi);
                }
            });

        this.UpdateAsObservable()
            .Where(stream => Input.GetKeyDown(KeyCode.Space) && isPlayerJupmed == false)
            .Subscribe(stream => PlayerJump(Momoi));

        this.UpdateAsObservable()
            .Where(stream => isPlayerJupmed == true && IsPlayerLanded())
            .Subscribe(stream => isPlayerJupmed = false);

        this.UpdateAsObservable()
            .Where(stream => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && isPlayerJupmed == false)
            .Subscribe(stream => PlayerIdle(Momoi));
    }
}
