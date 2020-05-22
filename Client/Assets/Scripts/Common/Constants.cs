
using UnityEngine;
public enum TxtColor {
    Red,
    Green,
    Blue,
    Yellow
}

public enum DamageType{
    None,
    AD = 1, // phsical
    AP = 2 // magical
}

public enum EntityType {
    None,
    Player,
    Monster
}
public enum EntityState {
    None,
    ControlledState, // be attacked, cannot be controlled, but can be hurted
}
public enum MonsterType {
    None,
    Normal=1,
    Boss=2
}
public class Constants {
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str, TxtColor c) {
        string result = "";
        switch(c) {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }
        return result;
    }
    // AutoGuide NPC
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCtrader = 3;

    // Scene and ID
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;
    public const string SceneMainCity = "SceneMainCity";

    // BG audio
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGLeipzig = "bgLeipzig";
    public const string AssassinHit = "assassin_Hit";

    // Login button audio
    public const string UILoginBtn = "uiLoginBtn";

    // Normal UI click audio
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string dungeonItemEnter = "fbitem";


    // screen standard width-height-rate
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    // Controller roller distance
    public const int ScreenOPDis = 90;

    // Action trigger param
    public const int ActionDefault = -1;
    public const int ActionBorn = 0;
    public const int ActionDie = 100;
    public const int ActionHit = 101;

    public const int DieAniLength = 5000;
    // Mix parameter
    public const int BlendIdle = 0;
    public const int BlendMove = 1;
    // Role move speed
    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 3;

    // Motion smooth acceleration
    public const float AccelerSpeed = 5;
    public const float AccelerHPSpeed = 0.3f;

    // continous attack time space
    public const int ComboSpace = 500;
}