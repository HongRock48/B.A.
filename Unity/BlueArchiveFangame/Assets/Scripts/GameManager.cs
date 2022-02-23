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
    private int collidedWithWallCount;

    CharacterInfo Momoi = new CharacterInfo();
    public void StartGame()
    {
        Debug.Log("game start!");
        var jsonReader = new JsonReader();
        var resourcePath = "Assets/Resources/Data";

        Momoi = jsonReader.LoadJsonFile<CharacterInfo>(resourcePath, "momoiDS");

        player.InitializePlayer(player.gameObject, Momoi);
    }

    private List<VectorReplacer> ReplaceUnityValues(List<Vector3> vector3List)
    {
        var vectorReplacerList = new List<VectorReplacer>();

        foreach (var vec3 in vector3List)
        {
            vectorReplacerList.Add(new VectorReplacer
            {
                x = vec3.x,
                y = vec3.y,
                z = vec3.z
            });
        }

        return vectorReplacerList;
    }

    private List<QuaternionReplacer> ReplaceUnityValues(List<Quaternion> quaternionList)
    {
        var quaternionReplacerList = new List<QuaternionReplacer>();

        foreach (var quaternion in quaternionList)
        {
            quaternionReplacerList.Add(new QuaternionReplacer
            {
                x = quaternion.x,
                y = quaternion.y,
                z = quaternion.z,
                w = quaternion.w
            });
        }

        return quaternionReplacerList;
    }

    public void AddCollidedWithWallCount()
    {
        collidedWithWallCount += 1;
    }

    public void SubtractCollidedWithWallCount()
    {
        collidedWithWallCount -= 1;
    }

    public int GetCollidedWithWallCount()
    {
        return collidedWithWallCount;
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
