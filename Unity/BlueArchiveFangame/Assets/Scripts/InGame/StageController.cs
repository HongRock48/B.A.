using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class StageController : MonoBehaviour
{
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        JsonReader jsonReader = new JsonReader();
        var resourcePath = "Assets/Resources/Data";

        CharacterInfo Momoi = new CharacterInfo();
        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");

        this.OnTriggerEnterAsObservable()
            .Where(stream => stream.gameObject.CompareTag("Player"))
            .Subscribe(stream => {
                if (gameManager.GetIsCollidedWithWall() == false) {
                    gameManager.SetIsCollidedWithWall(true);
                }
                player.PlayerWalk(Momoi);
            });

        this.OnTriggerExitAsObservable()
            .Where(stream => stream.gameObject.CompareTag("Player"))
            .Subscribe(stream => {
                Debug.Log("out!");
                if (gameManager.GetIsCollidedWithWall() == true) {
                    gameManager.SetIsCollidedWithWall(false);
                }
                player.PlayerWalk(Momoi);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
