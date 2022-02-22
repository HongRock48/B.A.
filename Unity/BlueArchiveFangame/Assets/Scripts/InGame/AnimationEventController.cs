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
            // 아이들 상태로
            characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);
        }
        else {
            // 2번째 연속공격으로
            characterController.SecondAttackCombo();
        }
    }

    public void SecondAttackFinished() {
        if (characterController.GetPlayerStatus() == (int)StaticValues.PLAYER_STATUS.ATTACK_COMBO_SECOND) {
            // 아이들 상태로
            characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);
        }
        else {
            // 2번째 연속공격으로
            characterController.ThirdAttackCombo();
        }
    }

    public void ThirdAttackFinished() {
        // 더 이상 연속공격이 없으므로 바로 아이들 상태로 변환
        characterController.SetPlayerStatus((int)StaticValues.PLAYER_STATUS.ON_GROUND);
    }
}
