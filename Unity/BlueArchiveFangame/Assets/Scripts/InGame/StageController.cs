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
    /// ������������ �ʱ�ȭ
    /// </summary>
    /// <param name="gameObject"> �÷����� ������Ʈ </param>
    /// <param name="info"> ���� ���õ� ĳ���� ���� </param>
    public void InitializeStage(GameObject gameObject, CharacterInfo info)
    {
    }
}
