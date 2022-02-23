using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StaticValues
{
    public static float WALK_SPEED = 2;
    public static float DASH_SPEED = 7;
    public static float JUMP_AMMOUNT = 15;
    public static float GRAVITY_SCALE = 2;

    public static string DATA_SHEET_PATH = "Assets/Resources/Data";
    public static string JSON_FILE = ".json";

    public enum PLAYABLE_CHARACTER
    {
        MOMOI = 1,
        MIDORI = 2,
        ARISU = 3,
        YUZU = 4,
    };

    public enum ANIMATION_NUMBER
    {
        IDLE = 0,
        WALK = 1,
        DASH = 2,
        JUMP = 3,
        ATTACK_COMBO_FIRST = 4,
        ATTACK_COMBO_SECOND = 5,
        ATTACK_COMBO_THIRD = 6,
        DASH_ATTACK = 7,
        JUMP_ATTACK = 8,
        HIT = 9,
        DOWN = 10,
        GET_UP = 11,
        SKILL = 12,
    };

    public enum PLAYER_STATUS
    {
        ON_GROUND = 0,
        WALK = 1,
        DASH = 2,
        JUMPED = 3,
        ATTACK_COMBO_FIRST = 4,
        ATTACK_COMBO_SECOND = 5,
        ATTACK_COMBO_THIRD = 6,
        CANT_MOVE = 98,
        STAND = 99,
    };
}
