using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageInfo : DataSheet
{
    //public Dictionary<string, List<Vector3>> enemyDictionary;
    //public List<StageEnemyInfo> enemyList;

    /// 다른 맵으로 이어지는 포탈 리스트 (이어진 스테이지의 id로 저장)
    //public List<int> portalList;

    /// 카메라 고정 포인트 리스트
    public List<Vector3Replacer> cameraPointList;
    public Vector3Replacer startingPoint;
}

[Serializable]
public class StageEnemyInfo
{
    public string enemyName;
    public List<Vector3Replacer> enemyPositionList;
}
