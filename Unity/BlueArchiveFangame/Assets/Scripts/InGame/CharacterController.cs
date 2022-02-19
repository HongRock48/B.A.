using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class CharacterController : MonoBehaviour {
    // TODO: �÷��̾� ���� FSM���� ����

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
    /// �÷��̾����� �ʱ�ȭ
    /// </summary>
    /// <param name="gameObject"> �÷����� ������Ʈ </param>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    public void InitializePlayer(GameObject gameObject, CharacterInfo info) {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        characterInfo = info;
    }

    /// <summary>
    /// �ִϸ��̼� ���� �޼ҵ�
    /// </summary>
    /// <param name="animationNum"> ���� ����ؾ��ϴ� �ִϸ��̼� </param>
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
    /// ���̵�
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
    /// �ȱ�
    /// </summary>
    public void PlayerWalk() {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.WALK : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        PlayerMove(StaticValues.WALK_SPEED, animationNum);
    }

    /// <summary>
    /// �뽬
    /// </summary>
    public void PlayerDash() {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.DASH : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        PlayerMove(StaticValues.DASH_SPEED, animationNum);
    }

    /// <summary>
    /// ĳ���͸� �����̴� �޼ҵ�
    /// </summary>
    /// <param name="speed"> �ӵ� </param>
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
    /// ����
    /// </summary>
    public void PlayerJump() {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.JUMP);

        playerRigidBody.AddForce(Vector3.up * StaticValues.JUMP_AMMOUNT, ForceMode.Impulse);
        playerStatus = (int)StaticValues.PLAYER_STATUS.JUMPED;
    }

    /// <summary>
    /// ����
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
    /// �����ߴ��� Ȯ��
    /// </summary>
    /// <returns> ������ �����ΰ� </returns>
    public bool IsPlayerLanded() {
        var distToGround = gameObject.GetComponent<BoxCollider>().bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    /// <summary>
    /// �߷� �߰� �޼ҵ�
    /// </summary>
    private void ResetGravity() {
        playerRigidBody.AddForce(Physics.gravity* StaticValues.GRAVITY_SCALE * playerRigidBody.mass);
    }

}
