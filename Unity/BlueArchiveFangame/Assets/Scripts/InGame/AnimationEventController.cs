using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private CharacterController characterController;

    void Start() {
        characterController = GetComponentInParent<CharacterController>();
    }

    public void FirstAttackFinished() {
        if (characterController.GetPlayerStatus() == (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_FIRST) {
            // ���̵� ���·�
            characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);
        }
        else {
            // 2��° ���Ӱ�������
            characterController.SecondAttackCombo();
        }
    }

    public void SecondAttackFinished() {
        if (characterController.GetPlayerStatus() == (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND) {
            // ���̵� ���·�
            characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);
        }
        else {
            // 2��° ���Ӱ�������
            characterController.ThirdAttackCombo();
        }
    }

    public void ThirdAttackFinished() {
        // �� �̻� ���Ӱ����� �����Ƿ� �ٷ� ���̵� ���·� ��ȯ
        characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);
    }
}
