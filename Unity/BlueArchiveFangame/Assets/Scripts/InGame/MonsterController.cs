using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private Animator droneAnimator;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject hitBox;

    void Start()
    {
        hitBox.OnTriggerEnterAsObservable()
            .Where(stream => stream.gameObject.CompareTag("PlayerAttack"))
            .Subscribe(stream =>
            {
                Damaged();
            });
    }

    public void Damaged()
    {
        
        Debug.Log("Damaged!");
    }
}
