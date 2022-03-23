using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private GameObject walls;
    [SerializeField]
    private GameObject portars;

    void Start()
    {
        walls.OnTriggerEnterAsObservable()
            .Where(stream => stream.gameObject.CompareTag("Player"))
            .Subscribe(stream =>
            {
                player.AddCollidedWithWallCount();
                player.PlayerWalk();
            });

        walls.OnTriggerExitAsObservable()
            .Where(stream => stream.gameObject.CompareTag("Player"))
            .Subscribe(stream =>
            {
                player.SubtractCollidedWithWallCount();
                player.PlayerWalk();
            });
    }

    /// <summary>
    /// 스테이지정보 초기화
    /// </summary>
    /// <param name="gameObject"> 플레리어 오브젝트 </param>
    /// <param name="info"> 현재 선택된 캐릭터 정보 </param>
    public void InitializeStage(GameObject gameObject, CharacterInfo info)
    {
    }
}
