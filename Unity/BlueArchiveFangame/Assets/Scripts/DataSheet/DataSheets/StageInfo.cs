using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo :  DataSheet {
    //public Dictionary<string, List<Vector3>> enemyDictionary;
    public List<StageEnemyInfo> enemyList;

    /// �ٸ� ������ �̾����� ��Żid ����Ʈ
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