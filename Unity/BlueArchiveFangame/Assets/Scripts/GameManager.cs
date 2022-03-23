using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Singleton
{
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private GameObject map;
    public ValueConverter valueConverter;

    CharacterInfo Momoi = new CharacterInfo();
    StageInfo forest00 = new StageInfo();
    public void StartGame()
    {
        valueConverter = new ValueConverter();

        Debug.Log("game start!");
        var jsonReader = new JsonReader();
        var resourcePath = "Assets/Resources/Data";

        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");
        forest00 = jsonReader.LoadJsonFile<StageInfo>(resourcePath, "forest00");

        Debug.Log("name : " + forest00.dataName);
        foreach(var v in forest00.cameraPointList)
        {
            Debug.Log(v.x + ", " + v.y + ", " + v.z);
        }
        Debug.Log("startingPoint : " + forest00.startingPoint.x + ", " + forest00.startingPoint.y + ", " + forest00.startingPoint.z);

        player.InitializePlayer(player.gameObject, Momoi, forest00);
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
