using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StaticValues {
    public static float WALK_SPEED = 2;
    public static float DASH_SPEED = 7;


    public enum ANIMATION_NUMBER {
        idle = 0,
        walk = 1,
        dash = 2,
        jump = 3,
        attack = 4,
    };
}
