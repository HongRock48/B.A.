using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageInfo : DataSheet
{
    //public Dictionary<string, List<Vector3>> enemyDictionary;
    //public List<StageEnemyInfo> enemyList;

    /// �ٸ� ������ �̾����� ��Ż ����Ʈ (�̾��� ���������� id�� ����)
    //public List<int> portalList;

    /// ī�޶� ���� ����Ʈ ����Ʈ
    public List<Vector3Replacer> cameraPointList;
    public Vector3Replacer startingPoint;
}

[Serializable]
public class StageEnemyInfo
{
    public string enemyName;
    public List<Vector3Replacer> enemyPositionList;
}
