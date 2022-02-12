using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private Animator momoiAnimator;
    [SerializeField]
    private CameraController cameraController;

    private Rigidbody playerRigidBody;

    private void AnimationControll(CharacterInfo info, int animationNum) {
        switch (info.id) {
            case 1:
                if (momoiAnimator.GetInteger("animationNum") != animationNum) {
                    momoiAnimator.SetInteger("animationNum", animationNum);
                }
                break;

            default:

                break;
        }
    }

    public void InitializePlayer(GameObject gameObject, CharacterInfo info) {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void PlayerIdle(CharacterInfo info) {
        switch (info.id)
        {
            case 1:
                AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.idle);
                break;

            default:

                break;
        }
    }

    public void PlayerMove(float speed) {
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

    public void PlayerWalk(CharacterInfo info) {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.walk);
        PlayerMove(StaticValues.WALK_SPEED);
    }

    public void PlayerDash(CharacterInfo info) {
        AnimationControll(info, (int)StaticValues.ANIMATION_NUMBER.dash);
        PlayerMove(StaticValues.DASH_SPEED);
    }

    public void PlayerAttack(CharacterInfo info) {
        switch (info.id) {
            case 1:
                break;

            default:

                break;
        }
    }

    void Start() {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        JsonReader jsonReader = new JsonReader();
        var resourcePath = "Assets/Resources/Data";

        CharacterInfo Momoi = new CharacterInfo();
        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");

        this.FixedUpdateAsObservable()
            .Where(stream => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            .Subscribe(stream => {
                if (Input.GetKey(KeyCode.LeftShift) && !gameManager.GetIsCollidedWithWall()) {
                    Debug.Log("gameManager.GetIsCollidedWithWall()");
                    PlayerDash(Momoi);
                }
                else {
                    PlayerWalk(Momoi);
                }
            });

        this.FixedUpdateAsObservable()
            .Where(stream => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            .Subscribe(stream => PlayerIdle(Momoi));
    }
}
