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
    /// ù��° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void FirstAttackStarted()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST, true);
    }

    /// <summary>
    /// ù��° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void FirstAttackFinished()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST, false);
        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST);
    }

    /// <summary>
    /// �ι�° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void SecondAttackStarted()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND, true);
    }

    /// <summary>
    /// �ι�° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void SecondAttackFinished()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND, false);
        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND);
    }

    /// <summary>
    /// ����° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void ThirdAttackStarted()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD, true);
    }

    /// <summary>
    /// ����° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void ThirdAttackFinished()
    {
        characterController.SetCurrentAttackStatus((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_THIRD, false);
        ToIdleOrMoveState();
    }

    /// <summary>
    /// ���� ���¿� ���� ���� ���� ���� �Ǵ� ���̵�, �̵� ���·� ��ȯ
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
    /// �̵�Ű �Է¿��ο� ���� ���̵�, �̵� ���·� ��ȯ
    /// </summary>
    public void ToIdleOrMoveState()
    {
        characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            // ���̵� ���·�
            characterController.PlayerIdle();
        }
        else
        {
            // �̵� ���·�
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
