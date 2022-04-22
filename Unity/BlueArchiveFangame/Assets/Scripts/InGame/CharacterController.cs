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
                        // 연속공격
                        if (Input.GetKeyDown(KeyCode.Z) && playerMovingStatus != (int)StaticValues.PLAYER_STATUS.DASH)
                        {
                            playerMovingStatus = (int)StaticValues.PLAYER_STATUS.CANT_MOVE;

                            // 연속공격 시작
                            FirstAttackCombo();
                        }

                        // 점프
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
                            // 2차 연속공격
                            playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND;
                        }
                        break;

                    case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND:
                        if (Input.GetKeyDown(KeyCode.Z) && isPlayingSecondAttack)
                        {
                            playerMovingStatus = (int)StaticValues.PLAYER_STATUS.CANT_MOVE;

                            // 3차 연속공격
                            playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD;
                        }
                        break;

                    case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD:
                        break;
                }
            });
        
        // 부드러운 이동을 위해서 이동관련 메소드는 fixedUpdate에서 실행
        this.FixedUpdateAsObservable()
            .Subscribe(stream =>
            {
                switch (playerStatus)
                {
                    case (int)StaticValues.PLAYER_STATUS.ON_GROUND:
                        // 걷기, 달리기
                        if (playerMovingStatus != (int)StaticValues.PLAYER_STATUS.CANT_MOVE)
                        {
                            PlayerMove();
                        }
                        break;

                    case (int)StaticValues.PLAYER_STATUS.JUMPED:
                        // 점프상태로 걷기, 달리기
                        PlayerMove();
                        break;
                }

                // 플레이어 캐릭터에 중력 추가
                ResetGravity();
            });
    }

    /// <summary>
    /// 플레이어정보 초기화
    /// </summary>
    /// <param name="gameObject"> 플레리어 오브젝트 </param>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void InitializePlayer(GameObject gameObject, CharacterInfo characterInfo, StageInfo stageInfo)
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        this.characterInfo = characterInfo;
        transform.position = gameManager.valueConverter.ConvertToUnityValue(stageInfo.startingPoint);
    }

    /// <summary>
    /// 애니메이션 변경 메소드
    /// </summary>
    /// <param name="animationNum"> 현재 재생해야하는 애니메이션 </param>
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
    /// 아이들
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
    /// 플레이어 움직임
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
    /// 걷기
    /// </summary>
    public void PlayerWalk()
    {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.WALK : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        HandleMovement(StaticValues.WALK_SPEED, animationNum);
        playerMovingStatus = (int)StaticValues.PLAYER_STATUS.WALK;
    }

    /// <summary>
    /// 대쉬
    /// </summary>
    public void PlayerDash()
    {
        var animationNum = playerStatus == (int)StaticValues.PLAYER_STATUS.ON_GROUND ?
            (int)StaticValues.ANIMATION_NUMBER.DASH : (int)StaticValues.ANIMATION_NUMBER.JUMP;

        HandleMovement(StaticValues.DASH_SPEED, animationNum);
        playerMovingStatus = (int)StaticValues.PLAYER_STATUS.DASH;
    }

    /// <summary>
    /// 캐릭터를 움직이는 메소드
    /// </summary>
    /// <param name="speed"> 속도 </param>
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
    /// 점프
    /// </summary>
    public void PlayerJump()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.JUMP);

        playerRigidBody.AddForce(Vector3.up * StaticValues.JUMP_AMMOUNT, ForceMode.Impulse);
        playerStatus = (int)StaticValues.PLAYER_STATUS.JUMPED;
    }

    /// <summary>
    /// 첫번째 연속공격
    /// </summary>
    public void FirstAttackCombo()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_FIRST);
        playerStatus = (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST;
    }

    /// <summary>
    /// 두번째 연속공격
    /// </summary>
    public void SecondAttackCombo()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_SECOND);
    }

    /// <summary>
    /// 세번째 연속공격
    /// </summary>
    public void ThirdAttackCombo()
    {
        AnimationControll((int)StaticValues.ANIMATION_NUMBER.ATTACK_COMBO_THIRD);
    }

    /// <summary>
    /// 땅에 착지했는지 확인
    /// </summary>
    /// <returns> 착지한 상태인가 </returns>
    public bool IsPlayerLanded()
    {
        var distToGround = hitBox.GetComponent<BoxCollider>().bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + StaticValues.ON_GROUND_DISTANCE);
    }

    /// <summary>
    /// 중력 추가 메소드
    /// </summary>
    private void ResetGravity()
    {
        playerRigidBody.AddForce(Physics.gravity * StaticValues.GRAVITY_SCALE * playerRigidBody.mass);
    }

    /// <summary>
    /// 벽 충돌 횟수 증가
    /// </summary>
    public void AddCollidedWithWallCount()
    {
        collidedWithWallCount += 1;
    }

    /// <summary>
    /// 벽 충돌 횟수 감소
    /// </summary>
    public void SubtractCollidedWithWallCount()
    {
        collidedWithWallCount -= 1;
    }

    /// <summary>
    /// 외부에서 플레이어 상태 변경용 메소드
    /// </summary>
    /// <param name="newStatus">변경할 상태</param>
    public void SetPlayerStatus(int newStatus)
    {
        playerStatus = newStatus;
    }

    /// <summary>
    /// 외부에서 플레이어 상태 가져오는 메소드
    /// </summary>
    /// <returns></returns>
    public int GetPlayerStatus()
    {
        return playerStatus;
    }

    /// <summary>
    /// 플레이어 공격 상태 변경용 메소드
    /// </summary>
    /// <param name="playerAttackStatus">현재 공격 상태</param>
    /// <param name="status">변경할 값</param>
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
    /// 공격 상태에 따라서 공격범위 충돌박스 컨트롤
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
