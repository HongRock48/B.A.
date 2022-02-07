using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : Singleton
{
    [SerializeField]
    private PlayerController player;

    CharacterInfo Momoi = new CharacterInfo();
    public void StartGame()
    {
        Debug.Log("game start!");
        JsonReader jsonReader = new JsonReader();
        string resourcePath = "Assets/Resources/Data";

        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");

        player.InitializePlayer(player.gameObject, Momoi);
        Subject<string> subject = new Subject<string>();

    }
    public void UpdateGame()
    {
        player.PlayerWalk(Momoi);
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        UpdateGame();
    }
}
