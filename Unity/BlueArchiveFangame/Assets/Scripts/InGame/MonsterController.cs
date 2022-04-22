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

    private Rigidbody monsterRigidBody;
    private MonsterInfo monsterInfo;

    private int monsterStatus;

    void Start()
    {
        hitBox.OnTriggerEnterAsObservable()
            .Where(stream => stream.gameObject.CompareTag("PlayerAttack"))
            .Subscribe(stream =>
            {
                Damaged();
            });
    }

    /// <summary>
    /// �������� �ʱ�ȭ
    /// TODO: ���� ���ݵ� �ʱ�ȭ (����, ���� ����?)
    /// </summary>
    /// <param name="gameObject"> ���� ������Ʈ </param>
    /// <param name="info"> ���� ���� ���� </param>
    public void InitializeMonster(GameObject gameObject, MonsterInfo monsterInfo, StageInfo stageInfo)
    {
        monsterRigidBody = gameObject.GetComponent<Rigidbody>();
        this.monsterInfo = monsterInfo;
        //transform.position = gameManager.valueConverter.ConvertToUnityValue(stageInfo.startingPoint);


    }

    public void Damaged()
    {
        
        Debug.Log("Damaged!");
    }


    public bool SearchPlayer()
    {


        return true;
    }

    public void Move()
    {

    }
}
