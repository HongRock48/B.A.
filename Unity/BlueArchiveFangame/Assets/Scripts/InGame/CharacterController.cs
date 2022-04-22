using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Animator momoiAnimator;
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject hitBox;
    [SerializeField]
    private GameObject FirstAtkColBox;
    [SerializeField]
    private GameObject SecondAtkColBox;
    [SerializeField]
    private GameObject ThirdAtkColBox;

    private Rigidbody playerRigidBody;
    private CharacterInfo characterInfo;

    private int playerStatus;
    private int playerMovingStatus;
    private int collidedWithWallCount;

    private bool isPlayingFirstAttack;
    private bool isPlayingSecondAttack;
    private bool isPlayingThirdAttack;

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(stream =>
            {
                if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND)
                {
                    PlayerIdle();
                }

                if (IsPlayerLanded() == true)
                {
                    Debug.Log("landed");
                }
                else
                {
                    Debug.Log("air");
                }

                switch (playerStatus)
                {
                    case (int)StaticValues.PLAYER_STATUS.ON_GROUND:
                        // ���Ӱ���
                        if (Input.GetKeyDown(KeyCode.Z) && playerMovingStatus != (int)StaticValues.PLAYER_STATUS.DASH)
                        {
                            playerMovingStatus = (int)StaticValues.PLAYER_STATUS.CANT_MOVE;

                            // ���Ӱ��� ����
                            FirstAttackCombo();
                        }

                        // ����
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            PlayerJump();
                        }
                        break;

                    case (int)StaticValues.PLAYER_STATUS.JUMPED:
                        playerStatus = IsPlayerLanded() ? (int)StaticValues.PLAYER_STATUS.ON_GROUND : playerStatus;
                        break;

                    case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST:
                        if (Input.GetKeyDown(KeyCode.Z) && isPlayingFirstAttack)
                        {
                            playerMovingStatus = (int)StaticValues.PLAYER_STATUS.CANT_MOVE;
                            // 2�� ���Ӱ���
                            playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND;
                        }
                        break;

                    case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND:
                        if (Input.GetKeyDown(KeyCode.Z) && isPlayingSecondAttack)
                        {
                            playerMovingStatus = (int)StaticValues.PLAYER_STATUS.CANT_MOVE;

                            // 3�� ���Ӱ���
                            playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD;
                        }
                        break;

                    case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD:
                        break;
                }
            });
        
        // �ε巯�� �̵��� ���ؼ� �̵����� �޼ҵ�� fixedUpdate���� ����
        this.FixedUpdateAsObservable()
            .Subscribe(stream =>
            {
                switch (playerStatus)
                {
                    case (int)StaticValues.PLAYER_STATUS.ON_GROUND:
                        // �ȱ�, �޸���
                        if (playerMovingStatus != (int)StaticValues.PLAYER_STATUS.CANT_MOVE)
                        {
                            PlayerMove();
                        }
                        break;

                    case (int)StaticValues.PLAYER_STATUS.JUMPED:
                        // �������·� �ȱ�, �޸���
                        PlayerMove();
                        break;
                }

                // �÷��̾� ĳ���Ϳ� �߷� �߰�
                ResetGravity();
            });
    }

    /// <summary>
    /// �÷��̾����� �ʱ�ȭ
    /// </summary>
    /// <param name="gameObject"> �÷����� ������Ʈ </param>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    public void InitializePlayer(GameObject gameObject, CharacterInfo characterInfo, StageInfo stageInfo)
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        this.characterInfo = characterInfo;
        transform.position = gameManager.valueConverter.ConvertToUnityValue(stageInfo.startingPoint);
    }

    /// <summary>
    /// �ִϸ��̼� ���� �޼ҵ�
    /// </summary>
    /// <param name="animationNum"> ���� ����ؾ��ϴ� �ִϸ��̼� </param>
    private void AnimationControll(int animationNum)
    {
        switch(characterInfo.id)
        {
            case (int)StaticValues.PLAYABLE_CHARACTER.MOMOI:
                if (momoiAnimator.GetInteger("animationNum") != animationNum)
                {
                    momoiAnimator.SetInteger("animationNum", animationNum);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// ���̵�
    /// </summary>
    public void PlayerIdle()
    {
        switch(characterInfo.id)
        {
            case (int)StaticValues.PLAYABLE_CHARACTER.MOMOI:
                AnimationControll((int)StaticValues.ANIMATION_NUMBER.IDLE);
                playerMovingStatus = (int)StaticValues.PLAYER_STATUS.STAND;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// �÷��̾� ������
    /// </summary>
    public void PlayerMove()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            return;
        }
                
        if (Input.GetKey(KeyCode.LeftShift) && collidedWithWallCount == 0)
        {
            PlayerDash();
        }
        else
        {
            PlayerWalk();
        }
    }

    /// <summary>
    /// �ȱ�
    /// </summary>
    public void PlayerWalk()
    {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.WALK : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        HandleMovement(StaticValues.WALK_SPEED, animationNum);
        playerMovingStatus = (int)StaticValues.PLAYER_STATUS.WALK;
    }

    /// <summary>
    /// �뽬
    /// </summary>
    public void PlayerDash()
    {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.DASH : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        HandleMovement(StaticValues.DASH_SPEED, animationNum);
        playerMovingStatus = (int)StaticValues.PLAYER_STATUS.DASH;
    }

    /// <summary>
    /// ĳ���͸� �����̴� �޼ҵ�
    /// </summary>
    /// <param name="speed"> �ӵ� </param>
    private void HandleMovement(float speed, int animationNum)
    {
        AnimationControll(animationNum);

        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        if (moveDirX < 0)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (moveDirX > 0)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = collidedWithWallCount >= 2 ? (moveHorizontal + moveVertical).normalized * speed * 0.1f : (moveHorizontal + moveVertical).normalized * speed;

        playerRigidBody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void PlayerJump()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.JUMP);

        playerRigidBody.AddForce(Vector3.up * StaticValues.JUMP_AMMOUNT, ForceMode.Impulse);
        playerStatus = (int)StaticValues.PLAYER_STATUS.JUMPED;
    }

    /// <summary>
    /// ù��° ���Ӱ���
    /// </summary>
    public void FirstAttackCombo()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_FIRST);
        playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST;
    }

    /// <summary>
    /// �ι�° ���Ӱ���
    /// </summary>
    public void SecondAttackCombo()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_SECOND);
    }

    /// <summary>
    /// ����° ���Ӱ���
    /// </summary>
    public void ThirdAttackCombo()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_THIRD);
    }

    /// <summary>
    /// ���� �����ߴ��� Ȯ��
    /// </summary>
    /// <returns> ������ �����ΰ� </returns>
    public bool IsPlayerLanded()
    {
        var distToGround = hitBox.GetComponent<BoxCollider>().bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + StaticValues.ON_GROUND_DISTANCE);
    }

    /// <summary>
    /// �߷� �߰� �޼ҵ�
    /// </summary>
    private void ResetGravity()
    {
        playerRigidBody.AddForce(Physics.gravity * StaticValues.GRAVITY_SCALE * playerRigidBody.mass);
    }

    /// <summary>
    /// �� �浹 Ƚ�� ����
    /// </summary>
    public void AddCollidedWithWallCount()
    {
        collidedWithWallCount += 1;
    }

    /// <summary>
    /// �� �浹 Ƚ�� ����
    /// </summary>
    public void SubtractCollidedWithWallCount()
    {
        collidedWithWallCount -= 1;
    }

    /// <summary>
    /// �ܺο��� �÷��̾� ���� ����� �޼ҵ�
    /// </summary>
    /// <param name="newStatus">������ ����</param>
    public void SetPlayerStatus(int newStatus)
    {
        playerStatus = newStatus;
    }

    /// <summary>
    /// �ܺο��� �÷��̾� ���� �������� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    public int GetPlayerStatus()
    {
        return playerStatus;
    }

    /// <summary>
    /// �÷��̾� ���� ���� ����� �޼ҵ�
    /// </summary>
    /// <param name="playerAttackStatus">���� ���� ����</param>
    /// <param name="status">������ ��</param>
    public void SetCurrentAttackStatus(int playerAttackStatus, bool status)
    {
        switch(playerAttackStatus)
        {
            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST:
                isPlayingFirstAttack = status;
                break;

            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND:
                isPlayingSecondAttack = status;
                break;

            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD:
                isPlayingThirdAttack = status;
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerAttackStatus"></param>
    /// <returns></returns>
    public bool GetCurrentAttackStatus(int playerAttackStatus)
    {
        var currentStatus = false;

        switch(playerAttackStatus)
        {
            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST:
                currentStatus = isPlayingFirstAttack;
                break;

            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND:
                currentStatus = isPlayingSecondAttack;
                break;

            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD:
                currentStatus = isPlayingThirdAttack;
                break;
        }

        return currentStatus;
    }

    /// <summary>
    /// ���� ���¿� ���� ���ݹ��� �浹�ڽ� ��Ʈ��
    /// </summary>
    /// <param name="playerAttackStatus"></param>
    /// <param name="isOn"></param>
    public void SetAttackCollisionBoxActivation(int playerAttackStatus, bool isOn)
    {
        switch (playerAttackStatus)
        {
            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST:
                FirstAtkColBox.SetActive(isOn);
                break;

            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND:
                SecondAtkColBox.SetActive(isOn);
                break;

            case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD:
                ThirdAtkColBox.SetActive(isOn);
                break;
        }
    }
}
