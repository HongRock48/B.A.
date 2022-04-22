using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// json저장용 클래스에서 unity변수로 변환(혹은 그반대 작업)
/// </summary>
public class ValueConverter : MonoBehaviour
{
    public List<Vector3Replacer> ReplaceUnityValues(List<Vector3> vector3List)
    {
        var vectorReplacerList = new List<Vector3Replacer>();

        foreach (var vec3 in vector3List)
        {
            vectorReplacerList.Add(new Vector3Replacer
            {
                x = vec3.x,
                y = vec3.y,
                z = vec3.z
            });
        }

        return vectorReplacerList;
    }

    public List<QuaternionReplacer> ReplaceUnityValues(List<Quaternion> quaternionList)
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

    public List<Vector3> ConvertToUnityValue(List<Vector3Replacer> vector3ReplacerList)
    {
        var vector3List = new List<Vector3>();

        foreach (var vec3 in vector3List)
        {
            vector3List.Add(new Vector3
            {
                x = vec3.x,
                y = vec3.y,
                z = vec3.z
            });
        }

        return vector3List;
    }

    public List<Quaternion> ConvertToUnityValue(List<QuaternionReplacer> quaternionReplacerList)
    {
        var quaternionList = new List<Quaternion>();

        foreach (var quaternionReplacer in quaternionReplacerList)
        {
            quaternionList.Add(new Quaternion
            {
                x = quaternionReplacer.x,
                y = quaternionReplacer.y,
                z = quaternionReplacer.z,
                w = quaternionReplacer.w
            });
        }

        return quaternionList;
    }

    public Vector3 ConvertToUnityValue(Vector3Replacer vector3Replacer)
    {
        return new Vector3
        {
            x = vector3Replacer.x,
            y = vector3Replacer.y,
            z = vector3Replacer.z
        };
    }

    public Quaternion ConvertToUnityValue(QuaternionReplacer quaternionReplacer)
    {
        return new Quaternion
        {
            x = quaternionReplacer.x,
            y = quaternionReplacer.y,
            z = quaternionReplacer.z,
            w = quaternionReplacer.w
        };
    }
}

[Serializable]
public class Vector3Replacer
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class QuaternionReplacer
{
    public float x;
    public float y;
    public float z;
    public float w;
}