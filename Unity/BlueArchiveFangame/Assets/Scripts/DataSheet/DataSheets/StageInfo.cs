using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageInfo : DataSheet
{
    public List<StageEnemyInfo> enemyList;

    /// �ٸ� ������ �̾����� ��Ż ����Ʈ (�̾��� ���������� id�� ����)
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
