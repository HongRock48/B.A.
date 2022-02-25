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
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST, true);
    }

    /// <summary>
    /// 첫번째 연속공격 애니메이션 종료 이벤트
    /// </summary>
    public void FirstAttackFinished()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST, false);
        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST);
    }

    /// <summary>
    /// 두번째 연속공격 애니메이션 시작 이벤트
    /// </summary>
    public void SecondAttackStarted()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND, true);
    }

    /// <summary>
    /// 두번째 연속공격 애니메이션 종료 이벤트
    /// </summary>
    public void SecondAttackFinished()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND, false);
        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND);
    }

    /// <summary>
    /// 세번째 연속공격 애니메이션 시작 이벤트
    /// </summary>
    public void ThirdAttackStarted()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD, true);
    }

    /// <summary>
    /// 세번째 연속공격 애니메이션 종료 이벤트
    /// </summary>
    public void ThirdAttackFinished()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD, false);
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
        }
        else
        {
            // 이동 상태로
            characterController.PlayerMove();
        }
    }

    public void MomoiFirstAtkDamageEvent()
    {
        characterController.SetAttackCollisionBoxActivation((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST, true);
    }

    public void MomoiFirstAtkDamageEndEvent()
    {
        characterController.SetAttackCollisionBoxActivation((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST, false);
    }

    public void MomoiSecondAtkDamageEvent()
    {
        characterController.SetAttackCollisionBoxActivation((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND, true);
    }
    public void MomoiSecondAtkDamageEndEvent()
    {
        characterController.SetAttackCollisionBoxActivation((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND, false);
    }

    public void MomoiThirdAtkDamageEvent()
    {
        characterController.SetAttackCollisionBoxActivation((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD, true);
    }
    public void MomoiThirdAtkDamageEndEvent()
    {
        characterController.SetAttackCollisionBoxActivation((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD, false);
    }

}
