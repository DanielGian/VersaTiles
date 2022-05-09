using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData_SCR
{
    public enum Element
    {
        Null,
        Fire,
        Water,
        Electric,
        Wood,
        Light,
        Dark
    }
    public int Level;
    public Element mainElement;
    public int HP;
    public int MaxHP;
    public int ATK;
    public float ATK_Multiplier;
    public int ATK_LevelNum;
    public readonly int[] ATK_Levels = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
    public int DEF;
    public int DEF_LevelNum;
    public readonly int[] DEF_Levels = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
    public float DEF_Multiplier;
    public int CP;
    public int MaxCP;
    public List<GameObject> CommandSlots = new List<GameObject>();
    private int slotNum;
    public int EXP;
    public int MaxEXP;
    public readonly int[] EXP_Cap = new int[11] { 100, 210, 330, 460, 600, 750, 910, 1080, 1260, 1450, 1650 };
    public float penaltyTime;
    public int LUCK;
    public int LUCK_LevelNum;
    public readonly int[] LUCK_Levels = new int[4] { 0, 10, 30, 50 };

    public PlayerData_SCR(BattleManager_SCR.MainPlayer player)
    {
        Level = player.Level;
        mainElement = (Element)((int)player.mainElement);
        MaxHP = player.MaxHP;
        HP = MaxHP;
        MaxCP = player.MaxCP;
        CP = MaxCP;
        slotNum = player.GetSlotNumber();
        EXP = player.EXP;
        MaxEXP = player.MaxEXP;
    }
}
