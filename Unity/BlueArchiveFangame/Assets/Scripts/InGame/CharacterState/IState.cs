using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {
    public void OnEnter(CharacterController player);
    public void OnExit();
}
