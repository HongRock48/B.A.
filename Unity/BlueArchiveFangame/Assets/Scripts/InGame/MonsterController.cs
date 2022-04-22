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
    /// 몬스터정보 초기화
    /// TODO: 몬스터 스텟들 초기화 (레벨, 몬스터 별로?)
    /// </summary>
    /// <param name="gameObject"> 몬스터 오브젝트 </param>
    /// <param name="info"> 현재 몬스터 정보 </param>
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
