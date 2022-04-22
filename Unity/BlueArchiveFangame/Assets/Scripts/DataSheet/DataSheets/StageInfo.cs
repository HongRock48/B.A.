using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageInfo : DataSheet
{
    public List<StageEnemyInfo> enemyList;

    /// 다른 맵으로 이어지는 포탈 리스트 (이어진 스테이지의 id로 저장)
    //public List<int> portalList;

    public Vector3Replacer startingPoint;
}

[Serializable]
public class StageEnemyInfo
{
    public int enemyId;
    public int enemtLevel;
    public int spawnLocation;
}
