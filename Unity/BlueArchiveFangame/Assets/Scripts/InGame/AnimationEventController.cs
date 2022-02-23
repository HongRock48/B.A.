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
        characterController.isPlayingFirstAttack = true;
    }

    /// <summary>
    /// ù��° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void FirstAttackFinished()
    {
        Debug.Log("����1 ����");
        characterController.isPlayingFirstAttack = false;
        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST);
    }

    /// <summary>
    /// �ι�° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void SecondAttackStarted()
    {
        characterController.isPlayingSecondAttack = true;
    }

    /// <summary>
    /// �ι�° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void SecondAttackFinished()
    {
        Debug.Log("����2 ����");
        characterController.isPlayingSecondAttack = false;

        CheckNextStateAfterAttack((int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND);
    }

    /// <summary>
    /// ����° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void ThirdAttackStarted()
    {
        characterController.isPlayingThirdAttack = true;
    }

    /// <summary>
    /// ����° ���Ӱ��� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public void ThirdAttackFinished()
    {
        Debug.Log("����3 ����");
        characterController.isPlayingThirdAttack = false;
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
            Debug.Log("to idle");
        }
        else
        {
            // �̵� ���·�
            characterController.PlayerMove();
            Debug.Log("to move");
        }
    }
}
