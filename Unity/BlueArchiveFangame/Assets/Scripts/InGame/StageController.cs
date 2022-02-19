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
    public PlayerController player;
    public GameObject walls;
    public GameObject portars;

    void Start() {
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        var jsonReader = new JsonReader();
        var resourcePath = "Assets/Resources/Data";
        var Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");

        walls.OnTriggerEnterAsObservable()
            .Where(stream => stream.gameObject.CompareTag("Player"))
            .Subscribe(stream => {
                gameManager.AddCollidedWithWallCount();
                player.PlayerWalk(Momoi);
            });

        walls.OnTriggerExitAsObservable()
            .Where(stream => stream.gameObject.CompareTag("Player"))
            .Subscribe(stream => {
                gameManager.SubtractCollidedWithWallCount();
                player.PlayerWalk(Momoi);
            });
    }
    
    public StageInfo ReadStageInfoJson(string infoName)
    {
        var jsonReader = new JsonReader();
        return jsonReader.JsonToOject<StageInfo>(StaticValues.DATA_SHEET_PATH + infoName + StaticValues.JSON_FILE);
    }
}
