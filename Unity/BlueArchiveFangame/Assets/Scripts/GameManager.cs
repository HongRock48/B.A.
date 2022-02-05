using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton
{
    public void StartGame()
    {
        Debug.Log("game start!");
        CharacterInfo Momoi = new CharacterInfo();
        JsonReader jsonReader = new JsonReader();
        string resourcePath = "Assets/Resources/Data";

        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");
    }
    public void UpdateGame()
    {

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
