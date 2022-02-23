using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
    }

    /// <summary>
    /// 첫번째 연속공격 애니메이션 시작 이벤트
    /// </summary>
    public void FirstAttackStarted()
    {
        characterController.isPlayingFirstAttack = true;
    }

    /// <summary>
    /// 첫번째 연속공격 애니메이션 종료 이벤트
    /// </summary>
    public void FirstAttackFinished()
    {
        Debug.Log("공격1 종료");
        characterController.isPlayingFirstAttack = false;
        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST);
    }

    /// <summary>
    /// 두번째 연속공격 애니메이션 시작 이벤트
    /// </summary>
    public void SecondAttackStarted()
    {
        characterController.isPlayingSecondAttack = true;
    }

    /// <summary>
    /// 두번째 연속공격 애니메이션 종료 이벤트
    /// </summary>
    public void SecondAttackFinished()
    {
        Debug.Log("공격2 종료");
        characterController.isPlayingSecondAttack = false;

        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND);
    }

    /// <summary>
    /// 세번째 연속공격 애니메이션 시작 이벤트
    /// </summary>
    public void ThirdAttackStarted()
    {
        characterController.isPlayingThirdAttack = true;
    }

    /// <summary>
    /// 세번째 연속공격 애니메이션 종료 이벤트
    /// </summary>
    public void ThirdAttackFinished()
    {
        Debug.Log("공격3 종료");
        characterController.isPlayingThirdAttack = false;
        ToIdleOrMoveState();
    }

    /// <summary>
    /// 현재 상태에 따라서 다음 공격 상태 또는 아이들, 이동 상태로 전환
    /// </summary>
    /// <param name="statusNum"></param>
    public void CheckNextStateAfterAttack(int statusNum)
    {
        if (characterController.GetPlayerStatus() == statusNum)
        {
            ToIdleOrMoveState();
        }
        else
        {
            switch (statusNum)
            {
                case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST:
                    characterController.SecondAttackCombo();
                    break;

                case (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND:
                    characterController.ThirdAttackCombo();
                    break;
            }
        }
    }

    /// <summary>
    /// 이동키 입력여부에 따라서 아이들, 이동 상태로 전환
    /// </summary>
    public void ToIdleOrMoveState()
    {
        characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            // 아이들 상태로
            characterController.PlayerIdle();
            Debug.Log("to idle");
        }
        else
        {
            // 이동 상태로
            characterController.PlayerMove();
            Debug.Log("to move");
        }
    }
}
