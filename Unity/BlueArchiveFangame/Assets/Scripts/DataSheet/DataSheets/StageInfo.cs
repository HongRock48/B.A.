using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo :  DataSheet {
    //public Dictionary<string, List<Vector3>> enemyDictionary;
    public List<StageEnemyInfo> enemyList;

    /// 다른 맵으로 이어지는 포탈id 리스트
    public List<int> portalList;
}

public class StageEnemyInfo {
    public string enemyName;
    public List<VectorReplacer> enemyPositionList;
}

public class VectorReplacer {
    public float x;
    public float y;
    public float z;
}

public class QuaternionReplacer
{
    public float x;
    public float y;
    public float z;
    public float w;
}