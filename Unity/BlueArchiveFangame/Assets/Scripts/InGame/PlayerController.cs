using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class PlayerController : MonoBehaviour {
    // TODO: �÷��̾� ���� FSM���� ����

    [SerializeField]
    private Animator momoiAnimator;
    [SerializeField]
    private CameraController cameraController;

    private Rigidbody playerRigidBody;

    private bool isPlayerJupmed = false;        // FSM���� �ٲٸ� �����ص� �ɵ�

    /// <summary>
    /// �ִϸ��̼� ���� �޼ҵ�
    /// </summary>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    /// <param name="animationNum"> ���� ����ؾ��ϴ� �ִϸ��̼� </param>
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
    /// �÷��̾����� �ʱ�ȭ
    /// </summary>
    /// <param name="gameObject"> �÷����� ������Ʈ </param>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
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
    /// �ȱ�
    /// </summary>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    public void PlayerWalk(CharacterInfo info) {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.WALK);
        PlayerMove(StaticValues.WALK_SPEED);
    }

    /// <summary>
    /// �뽬
    /// </summary>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    public void PlayerDash(CharacterInfo info) {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.DASH);
        PlayerMove(StaticValues.DASH_SPEED);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    public void PlayerJump(CharacterInfo info)
    {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.JUMP);

        playerRigidBody.AddForce(Vector3.up * StaticValues.JUMP_AMMOUNT, ForceMode.Impulse);
        isPlayerJupmed = true;
    }

    /// <summary>
    /// ĳ���͸� �����̴� �޼ҵ�
    /// </summary>
    /// <param name="speed"> �ӵ� </param>
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
    /// ����
    /// </summary>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    public void PlayerAttack(CharacterInfo info) {
        switch (info.id) {
            case 1:
                break;

            default:

                break;
        }
    }

    /// <summary>
    /// �����ߴ��� Ȯ��
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


        // TODO: ĳ���� ���� ������ ���� (CharacterInfo Momoi = new CharacterInfo();) ��
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
