using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : Singleton {
    [SerializeField]
    private PlayerController player;
    private bool isCollidedWithWall = false;

    CharacterInfo Momoi = new CharacterInfo();
    public void StartGame() {
        Debug.Log("game start!");
        JsonReader jsonReader = new JsonReader();
        string resourcePath = "Assets/Resources/Data";

        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");

        player.InitializePlayer(player.gameObject, Momoi);
        Subject<string> subject = new Subject<string>();

    }

    public void SetIsCollidedWithWall(bool isCollided) {
        isCollidedWithWall = isCollided;
    }

    public bool GetIsCollidedWithWall()
    {
        return isCollidedWithWall;
    }

    public void UpdateGame() {
    }

    void Start() {
        StartGame();
    }

    void Update() {
        UpdateGame();
    }
}
