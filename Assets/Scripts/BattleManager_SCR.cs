using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EaseFunctions;
using Rewired;
using TMPro;
using System.IO;

[System.Serializable]
public class BattleManager_SCR : MonoBehaviour
{
    public enum TimelineScript
    {
        None = 0,
        BattleBegin,
        CalculateTileSequence,
        GoToEnemyTurn,
        GoToPlayerTurn,
        BattleWon,
        LevelUp,
        IntroduceBoss,
        LevelCleared,
        GameOver
    }

    public enum Mode
    {
        Select = 0,
        Sequence,
        FormSequence,
        DecideCommand
    }

    public enum TileMethod
    {
        None = 0,
        Attack,
        MultiAttack,
        Guard,
        HealHP,
        HealCP,
        AddCommand,
        Multiply,
        SwapElement
    }

    public enum Enemy
    {
        Froopa = 0,
        Nutbug,
        Reshroom,
        Apple,
        Snapple,
        Punchey,
        Furb,
        Slugshroom,
        Octavine,
        Charco,
        Pyroma,
        EyeDye1,
        EyeDye2,
        EyeDye3,
        EyeDye4,
        EyeDye5,
        EyeDye6,
        Pokerface,
        MancalaSnake,
        Devol,
        Eighter,
        Lemonster,
        Bubblegoo,
        Honeygoo,
        Margarette,
        Freebee
    }
    public class EnemyStats
    {
        private Enemy enemy;
        private int turnCounter;
        public EnemyStats(Enemy _enemy, int _turnCounter)
        {
            enemy = _enemy;
            turnCounter = _turnCounter;
        }

        public Enemy GetEnemy() { return enemy; }
        public int GetTurnCounter() { return turnCounter; }
    }
    private readonly List<List<EnemyStats>> World1_1_Waves = new List<List<EnemyStats>>()
    {
        new List<EnemyStats> { new EnemyStats(Enemy.Froopa, 2), new EnemyStats(Enemy.Froopa, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Froopa, 1), new EnemyStats(Enemy.Nutbug, 2), new EnemyStats(Enemy.Froopa, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Nutbug, 1), new EnemyStats(Enemy.Nutbug, 2), new EnemyStats(Enemy.Nutbug, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Reshroom, 1), new EnemyStats(Enemy.Froopa, 2), new EnemyStats(Enemy.Nutbug, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Apple, 1) }
    };
    private readonly List<List<EnemyStats>> World1_2_Waves = new List<List<EnemyStats>>()
    {
        new List<EnemyStats> { new EnemyStats(Enemy.Punchey, 2), new EnemyStats(Enemy.Nutbug, 1), new EnemyStats(Enemy.Froopa, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Punchey, 1), new EnemyStats(Enemy.Punchey, 3) },
        new List<EnemyStats> { new EnemyStats(Enemy.Reshroom, 1), new EnemyStats(Enemy.Slugshroom, 3) },
        new List<EnemyStats> { new EnemyStats(Enemy.Furb, 1), new EnemyStats(Enemy.Furb, 2), new EnemyStats(Enemy.Furb, 1), new EnemyStats(Enemy.Slugshroom, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Slugshroom, 1), new EnemyStats(Enemy.Furb, 2), new EnemyStats(Enemy.Punchey, 3) },
        new List<EnemyStats> { new EnemyStats(Enemy.Octavine, 1) }
    };
    private readonly List<List<EnemyStats>> World1_3_Waves = new List<List<EnemyStats>>()
    {
        new List<EnemyStats> { new EnemyStats(Enemy.Furb, 2), new EnemyStats(Enemy.Furb, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Reshroom, 1), new EnemyStats(Enemy.Froopa, 1) },
        new List<EnemyStats> { new EnemyStats(Enemy.Charco, 2), new EnemyStats(Enemy.Nutbug, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Slugshroom, 2), new EnemyStats(Enemy.Furb, 2), new EnemyStats(Enemy.Reshroom, 1) },
        new List<EnemyStats> { new EnemyStats(Enemy.Punchey, 3), new EnemyStats(Enemy.Furb, 1), new EnemyStats(Enemy.Froopa, 2), new EnemyStats(Enemy.Charco, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Pyroma, 1) }
    };
    private readonly List<List<EnemyStats>> World2_1_Waves = new List<List<EnemyStats>>()
    {
        new List<EnemyStats> { new EnemyStats(Enemy.EyeDye1, 2), new EnemyStats(Enemy.EyeDye2, 1), new EnemyStats(Enemy.EyeDye3, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Devol, 3), new EnemyStats(Enemy.EyeDye3, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.EyeDye2, 2), new EnemyStats(Enemy.MancalaSnake, 3), new EnemyStats(Enemy.EyeDye2, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Margarette, 2), new EnemyStats(Enemy.EyeDye3, 1), new EnemyStats(Enemy.Devol, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.EyeDye1, 1), new EnemyStats(Enemy.EyeDye2, 1), new EnemyStats(Enemy.EyeDye3, 2),
                                new EnemyStats(Enemy.EyeDye4, 2), new EnemyStats(Enemy.EyeDye5, 3), new EnemyStats(Enemy.EyeDye6, 3) },
        new List<EnemyStats> { new EnemyStats(Enemy.Pokerface, 1) }
    };
    private readonly List<List<EnemyStats>> World2_2_Waves = new List<List<EnemyStats>>()
    {
        new List<EnemyStats> { new EnemyStats(Enemy.Freebee, 1), new EnemyStats(Enemy.Honeygoo, 2), new EnemyStats(Enemy.Freebee, 1) },
        new List<EnemyStats> { new EnemyStats(Enemy.Bubblegoo, 2), new EnemyStats(Enemy.Honeygoo, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Devol, 2), new EnemyStats(Enemy.MancalaSnake, 2), new EnemyStats(Enemy.Freebee, 1) },
        new List<EnemyStats> { new EnemyStats(Enemy.Bubblegoo, 2), new EnemyStats(Enemy.Lemonster, 1), new EnemyStats(Enemy.Margarette, 3) },
        new List<EnemyStats> { new EnemyStats(Enemy.Devol, 2), new EnemyStats(Enemy.EyeDye2, 1), new EnemyStats(Enemy.Freebee, 1), new EnemyStats(Enemy.Margarette, 2) },
        new List<EnemyStats> { new EnemyStats(Enemy.Eighter, 3) }
    };
    private List<GameObject> WaveNodes = new List<GameObject>();


    public class MainPlayer
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
        public enum StatusAilment
        {
            ATK_Up = 0,
            DEF_Up,
            Sleep,
            Lucky,
            Poisoned,
            Confused,
            Revenge,
            Burned,
            Paralyzed,
            Frozen,
            HP_Regen,
            CP_Regen,
            Stop,
            TouchDamage
        }
        BattleManager_SCR BM;

        public int Level = 1;
        public Element mainElement = Element.Null;
        public int HP = 20;
        public int MaxHP = 20;
        public int ATK = 0;
        public float ATK_Multiplier = 1f;
        public int ATK_LevelNum = 3;
        public readonly int[] ATK_Levels = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
        public int DEF = 0;
        public int DEF_LevelNum = 3;
        public readonly int[] DEF_Levels = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
        public int DEF_Plus = 0;
        public int CP = 8;
        public int MaxCP = 8;
        public List<GameObject> CommandSlots = new List<GameObject>();
        private int slotNum = 0;
        public int EXP = 0;
        public int MaxEXP = 100;
        public readonly int[] EXP_Cap = new int[12] { 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650 };
        public float penaltyTime = 25f;
        public int LUCK = 0;
        public int LUCK_LevelNum = 0;
        public readonly int[] LUCK_Levels = new int[4] { 0, 10, 30, 50 };
        public bool messyControls = false;
        public int messUpCounter = 0;
        private bool sunlightInEffect = false;

        private List<StatusAilment> AilmentList = new List<StatusAilment>();
        private List<GameObject> AilmentBoxes = new List<GameObject>();
        public readonly float MaxPenaltyTime = 25f;
        public int selectedCmdNum = 0;

        public MainPlayer()
        {
            Level = 1;
            mainElement = Element.Null;
            HP = 20;
            MaxHP = 20;
            ATK_Multiplier = 1f;
            ATK = ATK_Levels[3];
            ATK_LevelNum = 3;
            DEF_Plus = 0;
            DEF = DEF_Levels[3];
            DEF_LevelNum = 3;
            CP = 8;
            MaxCP = 8;
            LUCK = LUCK_Levels[0];
            LUCK_LevelNum = 0;
            EXP = 0;
            MaxEXP = EXP_Cap[0];
            penaltyTime = 25f;
        }

        public void CreateCommands()
        {
            if (CommandSlots.Count == 0)
            {
                CommandSlots.Add(OtherFunctions.CreateObjectFromResource("Prefabs/CommandBox_PFB", new Vector3(240f, 157f, -150f)));
                CommandSlots.Add(OtherFunctions.CreateObjectFromResource("Prefabs/CommandBox_PFB", new Vector3(240f, 67f, -150f)));
            }
        }
        public int GetSlotNumber() { return slotNum; }
        public void SetSlotNumber(int num) { selectedCmdNum = num; }
        public void Reassign()
        {
            slotNum = (selectedCmdNum - 1) % 2;
        }
        public string GetCmdName(int index) { return CommandSlots[index].GetComponent<Command_SCR>().GetName(); }
        public string GetCmdDesc(int index) { return CommandSlots[index].GetComponent<Command_SCR>().GetDescription(); }
        public int GetCmdCost(int index) { return CommandSlots[index].GetComponent<Command_SCR>().GetCost(); }
        public string GetCmdAffect(int index) { return CommandSlots[index].GetComponent<Command_SCR>().GetAffect(); }
        public string GetCmdRarity(int index) { return CommandSlots[index].GetComponent<Command_SCR>().GetRarity(); }
        public Color GetCmdColor(int index) { return CommandSlots[index].GetComponent<Command_SCR>().GetColor(); }
        public bool MetConditions(int index) { return CommandSlots[index].GetComponent<Command_SCR>().CommandIsAssigned(); }
        public void CommandAlert(int index)
        {
            GameManager_SCR.PlaySound(13);
            CommandSlots[index].GetComponent<Command_SCR>().Vibrate();
        }
        public void ChangeElement(Element element) { mainElement = element; }
        public void AddCommand()
        {
            CommandSlots[slotNum].GetComponent<Command_SCR>().AssignCommand();
            slotNum = ++slotNum % 2;
        }
        public IEnumerator UseCommand(int commandNum)
        {
            BM = GameObject.Find("BattleManager").GetComponent<BattleManager_SCR>();
            CP -= CommandSlots[commandNum].GetComponent<Command_SCR>().GetCost();
            float delay = CommandSlots[commandNum].GetComponent<Command_SCR>().ActivateCommand();
            yield return new WaitForSeconds(delay);

            int deadEnemyCount = 0;
            for (int i = 0; i < BM.Enemies.Length; i++)
            {
                GameObject Enemy = BM.Enemies[i];
                if (Enemy != null && Enemy.GetComponent<Enemy_SCR>().GetHP() == 0) { deadEnemyCount++; }
            }

            BM.StartCoroutine(BM.CheckEnemyStatus(true));
            if (BM.inBossBattle)
            {
                for (int i = 0; i < BM.Enemies.Length; i++)
                {
                    GameObject Enemy = BM.Enemies[i];
                    if (Enemy != null)
                    {
                        if (Enemy.GetComponent<Enemy_SCR>().GetHP() == 0) { yield return new WaitForSeconds(3.6f); break; }
                        else { yield return new WaitForSeconds(1.2f); break; }
                    }
                }
            }
            else { yield return new WaitForSeconds((float)deadEnemyCount * 1.2f); }
            if (BM.AllEnemiesDefeated())
            {
                BM.SS_Animation_index = 0;
                BM.tileGridCreated = false;
                if (BM.inBossBattle) { BM.TLS = TimelineScript.LevelCleared; }
                else
                { BM.TLS = TimelineScript.BattleWon; }
                BM.timeVal = 0.1f;
                BM.timeline_running = true;
                BM.DestroyGrid();
                Destroy(BM.TileSelector);
                yield break;
            }
            else
            {
                BM.FadeInTiles();
                BM.allowBattleControls = true;
                BM.WriteMessage($"Form a 4-tile sequence!", false);
            }
        }
        public bool ContainsAilment(StatusAilment SA)
        {
            for (int i = 0; i < GetAilmentCount(); i++)
            {
                if (AilmentList[i] == SA) { return true; }
            }
            return false;
        }
        public List<StatusAilment> GetAilmentList() { return AilmentList; }
        public int GetAilmentCount() { return AilmentBoxes.Count; }
        public void AddStatusAilment(StatusAilment SA, int turnNum)
        {
            BM = GameObject.Find("BattleManager").GetComponent<BattleManager_SCR>();
            bool repeatAilmentFound = false;
            int index = AilmentList.Count;
            if (turnNum != -1) { turnNum = Mathf.Clamp(turnNum, 1, 5); }

            for (int i = 0; i < AilmentList.Count; i++)
            {
                if (SA == AilmentList[i]) { repeatAilmentFound = true; index = i; break; }
            }

            if (repeatAilmentFound)
            {
                LookupAilment(index, SA);
                AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetTurns(turnNum);
            }
            else
            {
                AilmentList.Add(SA);
                AilmentBoxes.Add(OtherFunctions.CreateObjectFromResource("Prefabs/StatusAilmentIcon_PFB", BM.PlayerBoard.transform.position + new Vector3(163f + (index * 90f), -230f, -10f)));
                LookupAilment(index, SA);
                AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetTurns(turnNum);
                AilmentBoxes[index].transform.SetParent(BM.PlayerBoard.transform);
            }
        }
        public bool ZeroHP() { return HP == 0; }
        public void EnableSunlight() { sunlightInEffect = true; }
        public bool SunlightActive() { return sunlightInEffect; }
        private void LookupAilment(int index, StatusAilment SA)
        {
            switch (SA)
            {
                case StatusAilment.ATK_Up:
                    {
                        GameManager_SCR.PlaySound(32);
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(0);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 0);
                        break;
                    }
                case StatusAilment.DEF_Up:
                    {
                        GameManager_SCR.PlaySound(33);
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(1);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 1);
                        break;
                    }
                case StatusAilment.Sleep:
                    {
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(2);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 2);
                        break;
                    }
                case StatusAilment.Lucky:
                    {
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(3);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 3);
                        break;
                    }
                case StatusAilment.Poisoned:
                    {
                        GameManager_SCR.PlaySound(26);
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(4);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 4);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        OtherFunctions.CreateObjectFromResource("Prefabs/Poisoned_PFX", BoardPos + new Vector3(396f, -540f, -405f));
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 5);
                        break;
                    }
                case StatusAilment.Confused:
                    {
                        GameManager_SCR.PlaySound(24);
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(5);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 5);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        OtherFunctions.CreateObjectFromResource("Prefabs/Confused_PFX", BoardPos + new Vector3(396f, -540f, -405f));
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 6);
                        break;
                    }
                case StatusAilment.Revenge:
                    {
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(6);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 6);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 7);
                        break;
                    }
                case StatusAilment.Burned:
                    {
                        ATK_Multiplier = 0.75f;
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(7);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 7);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        //OtherFunctions.CreateObjectFromResource("Prefabs/Poisoned_PFX", BoardPos + new Vector3(396f, -540f, -405f));
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 8);
                        break;
                    }
                case StatusAilment.Paralyzed:
                    {
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(8);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 8);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 9);
                        break;
                    }
                case StatusAilment.Frozen:
                    {
                        Color color = BM.BoardOverlay.GetComponent<SpriteRenderer>().color + new Color(0f, 0f, 0f, 0.5f);
                        BM.StartCoroutine(EasingFunctions.ColorChangeFromRGBA(BM.BoardOverlay, color, 0.5f, Format.Scalar));
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(9);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 9);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        OtherFunctions.CreateObjectFromResource("Prefabs/Frozen_PFX", BoardPos + new Vector3(396f, -540f, -405f));
                        GameObject Sparkle_PFX = null;
                        if (GameObject.Find("Sparkle_PFX") == null) { Sparkle_PFX = OtherFunctions.CreateObjectFromResource("Prefabs/Sparkle_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f)); }
                        Sparkle_PFX.name = "Sparkle_PFX";
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 10);
                        break;
                    }
                case StatusAilment.HP_Regen:
                    {
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(10);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 10);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 11);
                        break;
                    }
                case StatusAilment.CP_Regen:
                    {
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(11);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 11);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 12);
                        break;
                    }
                case StatusAilment.TouchDamage:
                    {
                        AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(13);
                        OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 13);
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 14);
                        break;
                    }
            }
        }
        public void DecrementAilmentTurn()
        {
            for (int i = 0; i < AilmentBoxes.Count; i++) { AilmentBoxes[i].GetComponent<StatusAilmentIcon_SCR>().DecrementTurn(); }
            for (int i = 0; i < AilmentBoxes.Count; i++)
            {
                bool repeatLoop = false;
                if (AilmentBoxes[i].GetComponent<StatusAilmentIcon_SCR>().isTurnZero()) { RemoveStatusAilment(i); repeatLoop = true; }
                if (repeatLoop) { i--; }
            }

        }
        public void RemoveStatusAilment(int boxNum)
        {
            BM = GameObject.Find("BattleManager").GetComponent<BattleManager_SCR>();
            StatusAilment SA = (StatusAilment)AilmentBoxes[boxNum].GetComponent<StatusAilmentIcon_SCR>().GetStatusAilment();
            switch (SA)
            {
                case StatusAilment.ATK_Up:
                    {
                        ATK = ATK_Levels[3];
                        ATK_LevelNum = 3;
                        break;
                    }
                case StatusAilment.DEF_Up:
                    {
                        DEF = DEF_Levels[3];
                        DEF_LevelNum = 3;
                        break;
                    }
                case StatusAilment.Lucky:
                    {
                        LUCK = LUCK_Levels[0];
                        LUCK_LevelNum = 0;
                        break;
                    }
                case StatusAilment.Confused:
                    {
                        messyControls = false;
                        messUpCounter = 0;
                        break;
                    }
                case StatusAilment.Burned:
                    {
                        ATK_Multiplier = 1f;
                        break;
                    }
                case StatusAilment.Frozen:
                    {
                        Color color = BM.BoardOverlay.GetComponent<SpriteRenderer>().color - new Color(0f, 0f, 0f, 0.5f);
                        BM.StartCoroutine(EasingFunctions.ColorChangeFromRGBA(BM.BoardOverlay, color, 0.5f, Format.Scalar));
                        if (GameObject.Find("Sparkle_PFX") != null)
                        {
                            GameObject.Find("Sparkle_PFX").GetComponent<ParticleSystem>().Stop();
                        }
                        break;
                    }
            }
            Destroy(AilmentBoxes[boxNum]);
            AilmentList.RemoveAt(boxNum);
            AilmentBoxes.RemoveAt(boxNum);
            for (int i = 0; i < AilmentBoxes.Count; i++)
            {
                AilmentBoxes[i].transform.position = BM.PlayerBoard.transform.position + new Vector3(163f + (i * 90f), -230f, -10f);
            }
        }
        public void ExecuteStatusAilments(int boxNum)
        {
            StatusAilment SA = (StatusAilment)AilmentBoxes[boxNum].GetComponent<StatusAilmentIcon_SCR>().GetStatusAilment();
            switch (SA)
            {
                case StatusAilment.Poisoned:
                    {
                        BM.StartCoroutine(BM.ReceiveDamage(3));
                        break;
                    }
                case StatusAilment.Burned:
                    {
                        Vector3 BoardPos = BM.PlayerBoard.transform.position;
                        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/Burned_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
                        BM.StartCoroutine(BM.ReceiveDamage(3));
                        break;
                    }
                case StatusAilment.HP_Regen:
                    {
                        BM.AddHPToPlayer(3);
                        break;
                    }
                case StatusAilment.CP_Regen:
                    {
                        BM.AddCPToPlayer(2);
                        break;
                    }
            }
        }
        public bool ExecutionRequired(int boxNum)
        {
            StatusAilment SA = (StatusAilment)AilmentBoxes[boxNum].GetComponent<StatusAilmentIcon_SCR>().GetStatusAilment();
            return SA == StatusAilment.HP_Regen || SA == StatusAilment.CP_Regen || SA == StatusAilment.Burned || SA == StatusAilment.Poisoned
                || SA == StatusAilment.Sleep;
        }
        public void CureNegativeStatusAilments()
        {
            Vector3 BoardPos = BM.PlayerBoard.transform.position;
            GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/Cure_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
            for (int i = 0; i < AilmentBoxes.Count; i++)
            {
                bool repeatLoop = false;
                StatusAilment SA = (StatusAilment)AilmentBoxes[i].GetComponent<StatusAilmentIcon_SCR>().GetStatusAilment();

                if (SA == StatusAilment.Burned || SA == StatusAilment.Confused || SA == StatusAilment.Frozen || SA == StatusAilment.Paralyzed
                     || SA == StatusAilment.Poisoned || SA == StatusAilment.Sleep)
                {
                    RemoveStatusAilment(i);
                    repeatLoop = true;
                    if (repeatLoop) { i--; }
                }
            }
        }
        public bool isConfused() { return messyControls; }
    }

    private IEnumerator TypingMessage;
    private int worldNum;
    private int levelNum = 3;
    private int waveNum = 0;
    private int enemyCounter = 0;
    private int EXP_Reward = 0;
    private bool tileGridCreated = false;
    private bool allEnemiesSpawned = false;
    private bool allowBattleControls = false;
    private GameObject PlayerBoard;
    private GameObject PlayerElement;
    [SerializeField] private GameObject[] Enemies = new GameObject[6];
    private GameObject EnemyTarget;
    [SerializeField] private int targetNum = 0;
    [SerializeField] private int numOfEnemies = 0;
    private GameObject[] SpawnLocations = new GameObject[6];
    private GameObject[] BossLocations = new GameObject[3];
    private GameObject TileSelector;
    private TileAlgorithm_SCR TileAlgorithm;
    private Mode playerMode = Mode.Select;
    private Mode prevPlayerMode;
    private List<TileAlgorithm_SCR.Tile> PlayerGrid;
    private GameObject[] playTile = new GameObject[16];
    [SerializeField] private GameObject[] tileSequence = new GameObject[4];
    [SerializeField] private GameObject[] prevTiles = new GameObject[4];
    private GameObject[] selectedTile = new GameObject[4];
    private int tileBlockX = 0;
    private int tileBlockY = 0;
    [SerializeField] private int tileCounter = 0;
    private bool ignoreAddingTiles = false;
    private bool shakeUI = false;
    private bool inBossBattle = false;
    private bool enemyTurnOnce = false;
    private Vector3 staticPos;
    private readonly Vector3[,] TilePosition = new Vector3[4, 4]
    {
        { new Vector3(256f, 727f, -100f), new Vector3(256f, 587f, -100f), new Vector3(256f, 447f, -100f), new Vector3(256f, 307f, -100f)},
        { new Vector3(388f, 727f, -100f), new Vector3(388f, 587f, -100f), new Vector3(388f, 447f, -100f), new Vector3(388f, 307f, -100f)},
        { new Vector3(520f, 727f, -100f), new Vector3(520f, 587f, -100f), new Vector3(520f, 447f, -100f), new Vector3(520f, 307f, -100f)},
        { new Vector3(652f, 727f, -100f), new Vector3(652f, 587f, -100f), new Vector3(652f, 447f, -100f), new Vector3(652f, 307f, -100f)}
    };

    private GameObject BoardOverlay;
    private GameObject HPBar;
    private GameObject CPBar;
    private Image HP_HideBar;
    private Image CP_HideBar;
    private Image EXPBar;
    private GameObject Timer;
    private Image CircleTimer;
    private TMP_Text Timer_Text;
    private TMP_Text HP_Text;
    private TMP_Text CP_Text;
    private TMP_Text LevelNum_Text;
    private GameObject[] CommandButtons = new GameObject[2];
    private GameObject CommandPrompt;
    private TMP_Text Message_Text;
    private GameObject YouWonPrompt;
    private GameObject LevelClearedPrompt;
    private GameObject RewardBox;
    private GameObject RewardEXP_Text;
    private GameObject TilerIcon;
    private float HUD_vibration = 0f;
    private const string vowels = "aeiou";
    private const string newLine = "#SAVESTAT#";

    private GameObject GM;
    private GameObject BattleBkg;
    private GameObject DarkOverlay;
    private GameObject ReadyBar;
    private GameObject[] techBorder = new GameObject[2];
    private GameObject ReadyToPlayText;

    MainPlayer Player1 = new MainPlayer();
    private float selectCooldownTime = 0.2f;
    private Player PlayerControls;

    private readonly float[,] DamageMultiplier = new float[7, 7]
    {
        {1f,1f,1f,1f,1f,1f,1f},       //Null
        {1f,1f,0.5f,1f,1.5f,1f,1f},   //Fire
        {1f,1.5f,1f,0.5f,1f,1f,1f},   //Water
        {1f,1f,1.5f,1f,0.5f,1f,1f},   //Electric
        {1f,0.5f,1f,1.5f,1f,1f,1f},   //Wood
        {1f,1f,1f,1f,1f,1f,1.5f},     //Light
        {1f,1f,1f,1f,1f,1.5f,1f},     //Dark
    };

    //Timeline Variables
    private int SS_Animation_index = 0;
    private float timeVal = 4f;
    private readonly float timeValMax = 60f;
    private bool timeline_running = true;
    private TimelineScript TLS = TimelineScript.BattleBegin;
    private bool isCoroutineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        Player1.CreateCommands();
        staticPos = PlayerBoard.transform.position;
        TileAlgorithm = this.gameObject.GetComponent<TileAlgorithm_SCR>();
        StartBattle();
        CreateWaveNodes();
        TLS = TimelineScript.BattleBegin;
        SetBattleBackground();
        if (!GM.GetComponent<GameManager_SCR>().PenaltyTimerEnabled())
        {
            Timer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            CircleTimer.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            Timer_Text.color = new Color(1f, 1f, 1f, 0f);
        }
        if (worldNum != 1 || levelNum != 1)
        {
            try
            {
                LoadPlayerStats();
            }
            catch (Exception e) { Debug.LogError(e.Message); }
        }
    }

    public void SavePlayerStats()
    {
        int _WorldNum = worldNum;
        int _LevelNum = levelNum;
        int _Level = Player1.Level;
        int _MaxHP = Player1.MaxHP;
        int _MaxCP = Player1.MaxCP;
        int _EXP = Player1.EXP;
        int _MaxEXP = Player1.MaxEXP;

        if (worldNum == 1 && levelNum != 3 || worldNum == 2 && levelNum != 2) { _LevelNum++; }
        else { _WorldNum++; _LevelNum = 1; }

        string[] contents = new string[]
        {
            $"{_WorldNum}",
            $"{_LevelNum}",
            $"{_Level}",
            $"{_MaxHP}",
            $"{_MaxCP}",
            $"{_EXP}",
            $"{_MaxEXP}",
        };

        string saveString = String.Join(newLine, contents);
        File.WriteAllText(Application.dataPath + "/Player_Stats.txt", saveString);
    }
    public void LoadPlayerStats()
    {
        string saveString = File.ReadAllText(Application.dataPath + "/Player_Stats.txt");
        string[] contents = saveString.Split(new[] { newLine }, System.StringSplitOptions.None);

        Player1.Level = int.Parse(contents[2]);
        Player1.MaxHP = int.Parse(contents[3]);
        Player1.HP = Player1.MaxHP;
        Player1.MaxCP = int.Parse(contents[4]);
        Player1.CP = Player1.MaxCP;
        Player1.EXP = int.Parse(contents[5]);
        Player1.MaxEXP = int.Parse(contents[6]);
    }
    public void ErasePlayerStats()
    {
        File.WriteAllText(Application.dataPath + "/Player_Stats.txt", "");
        GM.GetComponent<GameManager_SCR>().DeactivateLoad();
    }

    // Update is called once per frame
    void Update()
    {
        numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (Player1 != null)
        {
            Player1.HP = Mathf.Clamp(Player1.HP, 0, Player1.MaxHP);
            Player1.CP = Mathf.Clamp(Player1.CP, 0, Player1.MaxCP);
        }
        PlayerControls = ReInput.players.GetPlayer(0);
        timeVal = Mathf.Clamp(timeVal, 0f, timeValMax);

        if (timeline_running)
        {
            if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
            else { StartCoroutine(ExecuteTimeline(TLS)); }
        }
        if (GM.GetComponent<GameManager_SCR>().PenaltyTimerEnabled() && allowBattleControls && playerMode != Mode.DecideCommand)
        {
            if (Player1.penaltyTime > 0f) { Player1.penaltyTime -= 1f * Time.deltaTime; }
            else { FormTileSequence(); }
        }
        if (allowBattleControls) { BattleControls(); }

        if (shakeUI)
        {
            float randX = UnityEngine.Random.Range(-HUD_vibration, HUD_vibration);
            float randY = UnityEngine.Random.Range(-HUD_vibration, HUD_vibration);
            PlayerBoard.transform.position = new Vector3(staticPos.x + randX, staticPos.y + randY, 0f);
            HP_HideBar.transform.position = new Vector3(378.5f + randX, 978.5f + randY, -5f);
            HP_Text.GetComponent<TMP_Text>().transform.position = new Vector3(302f + randX, 1013f + randY, -5f);
            CP_Text.GetComponent<TMP_Text>().transform.position = new Vector3(302f + randX, 922.9f + randY, -5f);
            LevelNum_Text.GetComponent<TMP_Text>().transform.position = new Vector3(84.2998f + randX, 971f + randY, -5f);
            CircleTimer.transform.position = new Vector3(727f + randX, 106.5f + randY, -5f);
            Timer_Text.GetComponent<TMP_Text>().transform.position = new Vector3(728f + randX, 90f + randY, -5f);

            if (HUD_vibration > 0f) { HUD_vibration -= 0.25f; }
            else { HUD_vibration = 0; shakeUI = false; }
        }
        else
        {
            PlayerBoard.transform.position = new Vector3(staticPos.x, staticPos.y, 0f);
            HP_HideBar.transform.position = new Vector3(378.5f, 978.5f, -5f);
            HP_Text.GetComponent<TMP_Text>().transform.position = new Vector3(302f, 1013f, -5f);
            CP_Text.GetComponent<TMP_Text>().transform.position = new Vector3(302f, 922.9f, -5f);
            LevelNum_Text.GetComponent<TMP_Text>().transform.position = new Vector3(84.2998f, 971f, -5f);
            CircleTimer.transform.position = new Vector3(727f, 106.5f, -5f);
            Timer_Text.GetComponent<TMP_Text>().transform.position = new Vector3(728f, 90f, -5f);
        }
        PlayerDisplay();
        LevelUpCap();
    }
    IEnumerator ExecuteTimeline(TimelineScript timelinescript)
    {
        if (isCoroutineRunning) { yield break; }
        isCoroutineRunning = true;

        if (timelinescript == TimelineScript.BattleBegin)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Idle);
                        if (!tileGridCreated) { CreatePlayerGrid(TileAlgorithm.GetGrid()); tileGridCreated = true; }
                        SpawnEnemy();
                        Debug.Log("Made it!");
                        if (!allEnemiesSpawned) { SS_Animation_index = 0; }
                        timeVal = 1.2f;
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < Player1.GetAilmentCount(); i++)
                        {
                            if (Player1.ExecutionRequired(i))
                            {
                                Player1.ExecuteStatusAilments(i);
                                yield return new WaitForSeconds(1f);
                            }
                            else { continue; }
                        }
                        Player1.DecrementAilmentTurn();
                        if (Player1.ContainsAilment(MainPlayer.StatusAilment.Frozen))
                        {
                            WriteMessage($"You are currently frozen and cannot attack or use Commands!", true);
                            yield return new WaitForSeconds(2f);
                            SS_Animation_index = 0;
                            TLS = TimelineScript.GoToEnemyTurn;
                            timeVal = 0f;
                            timeline_running = true;
                        }
                        else
                        {
                            TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Idle);
                            WriteMessage($"Form a 4-tile sequence!", false);
                            FadeInTiles();
                            allowBattleControls = true;
                            SS_Animation_index = 0;
                            timeline_running = false;
                            TileSelector = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(TilePosition[0, 0].x, TilePosition[0, 0].y, -105f));
                            OtherFunctions.ChangeSprite(TileSelector, "Sprites/GameplayUI/TileSelector");
                        }
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.CalculateTileSequence)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        if (tileCounter > 0)
                        {
                            StartCoroutine(CalculateTileSequence());
                            timeline_running = false;
                        }
                        else
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                Destroy(playTile[i]);
                            }
                            TLS = TimelineScript.GoToEnemyTurn;
                            timeVal = 0f;
                            timeline_running = true;
                        }
                        
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.GoToEnemyTurn)
        {
            if (!Player1.ContainsAilment(MainPlayer.StatusAilment.Frozen)) { tileGridCreated = false; }

            if (AllEnemiesDefeated())
            {
                DestroyGrid();
                enemyTurnOnce = false;
                SS_Animation_index = 0;
                if (inBossBattle) { TLS = TimelineScript.LevelCleared; }
                else { TLS = TimelineScript.BattleWon; }
                timeVal = 0.1f;
            }
            else
            {
                if (!enemyTurnOnce)
                {
                    if (!tileGridCreated) { CreatePlayerGrid(TileAlgorithm.GetGrid()); tileGridCreated = true; }
                    for (int i = 0; i < Enemies.Length; i++)
                    {
                        GameObject Enemy = Enemies[i];
                        if (Enemy == null) { continue; }
                        if (Enemy.GetComponent<Enemy_SCR>().DecrementTurn())
                        {
                            float delay = Enemy.GetComponent<Enemy_SCR>().ChooseMove();
                            yield return new WaitForSeconds(delay);
                            TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Idle);
                        }
                    }
                }

                if (!EnemyDeadInEnemyTurn())
                {
                    if (Player1.ZeroHP()) { TLS = TimelineScript.GameOver; SS_Animation_index = 0; timeVal = 0.2f; }
                    else
                    {
                        TLS = TimelineScript.GoToPlayerTurn;
                        timeVal = (enemyTurnOnce ? 0f : 1f);
                        enemyTurnOnce = false;
                    }
                }
                else
                {
                    StartCoroutine(CheckEnemyStatus());
                    enemyTurnOnce = true;
                    timeline_running = false;
                }
            }
        }
        else if (timelinescript == TimelineScript.GoToPlayerTurn)
        {
            for (int i = 0; i < Player1.GetAilmentCount(); i++)
            {
                if (Player1.ExecutionRequired(i))
                {
                    Player1.ExecuteStatusAilments(i);
                    yield return new WaitForSeconds(1f);
                }
                else { continue; }
            }
            for (int i = 0; i < Enemies.Length; i++)
            {
                GameObject Enemy = Enemies[i];
                if (Enemy != null)
                {
                    for (int j = 0; j < Enemy.GetComponent<Enemy_SCR>().GetAilmentCount(); j++)
                    {
                        if (Enemy.GetComponent<Enemy_SCR>().ExecutionRequired(j))
                        {
                            Enemy.GetComponent<Enemy_SCR>().ExecuteStatusAilments(j);
                            yield return new WaitForSeconds(1f);
                        }
                        else { continue; }
                    }
                }
            }

            if (Player1.ZeroHP()) { TLS = TimelineScript.GameOver; SS_Animation_index = 0; timeVal = 0.2f; }
            else
            {
                Player1.DecrementAilmentTurn();
                foreach (GameObject enemy in Enemies) { if (enemy != null) { enemy.GetComponent<Enemy_SCR>().DecrementAilmentTurn(); } }

                ResetVariablesInTurn();
                if (Player1.ContainsAilment(MainPlayer.StatusAilment.Frozen))
                {
                    WriteMessage($"You are currently frozen and cannot attack or use Commands!", true);
                    yield return new WaitForSeconds(2f);
                    SS_Animation_index = 0;
                    TLS = TimelineScript.GoToEnemyTurn;
                    timeVal = 0f;
                    timeline_running = true;
                }
                else
                {
                    FadeInTiles();
                    TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Idle);
                    WriteMessage($"Form a 4-tile sequence!", false);
                    allowBattleControls = true;
                    SS_Animation_index = 0;
                    timeline_running = false;
                    TileSelector = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(TilePosition[0, 0].x, TilePosition[0, 0].y, -105f));
                    OtherFunctions.ChangeSprite(TileSelector, "Sprites/GameplayUI/TileSelector");
                }
            }
        }
        else if (timelinescript == TimelineScript.BattleWon)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        Vector3 BoardPos = PlayerBoard.transform.position;

                        TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Happy);
                        YouWonPrompt = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(BoardPos.x + 420f, BoardPos.y - 320f, -500f));
                        YouWonPrompt.AddComponent<YouWon_SCR>();
                        GameManager_SCR.PlaySound(5);
                        WriteMessage($"Congrats! You defeated all the enemies!", true);
                        timeVal = 1.5f;
                        break;
                    }
                case 2:
                    {
                        Vector3 BoardPos = PlayerBoard.transform.position;
                        Vector3 PromptPos = YouWonPrompt.transform.position;

                        StartCoroutine(EasingFunctions.TranslateTo(YouWonPrompt, PromptPos + new Vector3(0f, 120f, 0f), 0.5f, 3, Easing.EaseOut));
                        RewardBox = OtherFunctions.CreateObjectFromResource("Prefabs/RewardBox_PFB", new Vector3(BoardPos.x + 420f, BoardPos.y - 720f, -500f));
                        RewardBox.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                        RewardEXP_Text = RewardBox.transform.Find("Canvas/EXP Text").gameObject;
                        RewardEXP_Text.transform.position = RewardBox.transform.position + new Vector3(108f, 0f, -5f);
                        RewardEXP_Text.GetComponent<TMP_Text>().text = $"+{EXP_Reward} EXP";
                        WriteMessage($"You earned {EXP_Reward} EXP!", true);
                        timeVal = 1f;
                        break;
                    }
                case 3:
                    {
                        if (EXP_Reward > 0)
                        {
                            Player1.EXP += 1;
                            EXP_Reward--;
                            SS_Animation_index = 2;
                            timeVal = 0.01f;
                        }
                        else { timeVal = 1f; }

                        RewardEXP_Text.GetComponent<TMP_Text>().text = $"+{EXP_Reward} EXP";
                        break;
                    }
                case 4:
                    {
                        Destroy(YouWonPrompt);
                        Destroy(RewardBox);
                        Destroy(RewardEXP_Text);
                        timeVal = 0.5f;
                        break;
                    }
                case 5:
                    {
                        waveNum++;
                        TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Walk);
                        ResetVariablesInWave();
                        WriteMessage($"Entering Wave {waveNum + 1}!", true);
                        StartCoroutine(EasingFunctions.TranslateTo(TilerIcon, WaveNodes[waveNum].transform.position, 1f));

                        if (OnLastWave()) { TLS = TimelineScript.IntroduceBoss; }
                        else { TLS = TimelineScript.BattleBegin; }
                        SS_Animation_index = 0;
                        timeVal = 1f;
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.IntroduceBoss)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        StartCoroutine(AudioFade_SCR.Fade(GM.GetComponent<GameManager_SCR>().GetAudioSrc(), 0f, 1f));
                        inBossBattle = true;
                        TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Idle);
                        DarkOverlay = GameObject.Find("Dark Overlay");
                        for (int i = 0; i < techBorder.Length; i++) { techBorder[i].GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f); }
                        ReadyToPlayText.GetComponent<TMP_Text>().text = $"WARNING!";
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(ReadyBar, "#000000", 0.25f, 1f));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[0], new Vector3(0f, 668f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[1], new Vector3(1920f, 412f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(ReadyToPlayText, new Vector3(960f, 540f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.25f, 0.5f));
                        timeVal = 1.5f;
                        break;
                    }
                case 2:
                    {
                        if (worldNum != 1 || levelNum != 1)
                        {
                            StartCoroutine(AudioFade_SCR.Fade(GM.GetComponent<GameManager_SCR>().GetAudioSrc(), 0.6f, 1f));
                            PlayBossMusic();
                        }
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(ReadyBar, "#000000", 0.25f, 0f));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[0], new Vector3(0f, 1260f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[1], new Vector3(1920f, -180f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(ReadyToPlayText, new Vector3(2700f, 540f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.25f, 0f));
                        TLS = TimelineScript.BattleBegin;
                        SS_Animation_index = 0;
                        timeVal = 0.5f;
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.LevelCleared)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        StartCoroutine(AudioFade_SCR.Fade(GM.GetComponent<GameManager_SCR>().GetAudioSrc(), 0f, 1f));
                        yield return new WaitForSeconds(1.5f);
                        Vector3 BoardPos = PlayerBoard.transform.position;

                        TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Happy);
                        LevelClearedPrompt = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(BoardPos.x + 254f, BoardPos.y - 320f, -500f));
                        LevelClearedPrompt.AddComponent<LevelCleared_SCR>();
                        WriteMessage($"Congrats! You defeated the boss and cleared all waves!", true);
                        GameManager_SCR.PlaySound(6);
                        timeVal = 3.5f;
                        break;
                    }
                case 2:
                    {
                        Vector3 BoardPos = PlayerBoard.transform.position;
                        Vector3 PromptPos = LevelClearedPrompt.transform.position;

                        StartCoroutine(EasingFunctions.TranslateTo(LevelClearedPrompt, PromptPos + new Vector3(0f, 120f, 0f), 0.5f, 3, Easing.EaseOut));
                        RewardBox = OtherFunctions.CreateObjectFromResource("Prefabs/RewardBox_PFB", new Vector3(BoardPos.x + 420f, BoardPos.y - 720f, -500f));
                        RewardBox.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                        RewardEXP_Text = RewardBox.transform.Find("Canvas/EXP Text").gameObject;
                        RewardEXP_Text.transform.position = RewardBox.transform.position + new Vector3(108f, 0f, -5f);
                        RewardEXP_Text.GetComponent<TMP_Text>().text = $"+{EXP_Reward} EXP";
                        WriteMessage($"You earned {EXP_Reward} EXP!", true);
                        timeVal = 1f;
                        break;
                    }
                case 3:
                    {
                        if (EXP_Reward > 0)
                        {
                            Player1.EXP += 1;
                            EXP_Reward--;
                            SS_Animation_index = 2;
                            timeVal = 0.01f;
                        }
                        else { timeVal = 1f; }

                        RewardEXP_Text.GetComponent<TMP_Text>().text = $"+{EXP_Reward} EXP";
                        break;
                    }
                case 4:
                    {
                        if (worldNum == 2 && levelNum == 2) { ErasePlayerStats(); }
                        else { SavePlayerStats(); }
                        GM.GetComponent<GameManager_SCR>().ActivateLoad();
                        if (worldNum == 2 && levelNum == 2) { GM.GetComponent<GameManager_SCR>().StartTimeline(GameManager_SCR.TimelineScript.DemoBattleToMainMenu, 1.5f); }
                        else { GM.GetComponent<GameManager_SCR>().StartTimeline(GameManager_SCR.TimelineScript.DemoBattleToArcade, 1f); }
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.GameOver)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        ReadyToPlayText.transform.position = new Vector3(-1618f, 540f);
                        DarkOverlay = GameObject.Find("Dark Overlay");
                        for (int i = 0; i < techBorder.Length; i++) { techBorder[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f); }
                        ReadyToPlayText.GetComponent<TMP_Text>().text = $"GAME OVER";
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(ReadyBar, "#000000", 1f, 1f));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[0], new Vector3(0f, 668f), 1f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[1], new Vector3(1920f, 412f), 1f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(ReadyToPlayText, new Vector3(960f, 540f), 1f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 1f, 0.5f));
                        GM.GetComponent<GameManager_SCR>().StartTimeline(GameManager_SCR.TimelineScript.DemoBattleToMainMenu, 1.5f);
                        break;
                    }
            }
        }

        isCoroutineRunning = false;
    }
    
    private void FormTileSequence()
    {
        allowBattleControls = false;
        playerMode = Mode.FormSequence;

        Destroy(TileSelector);
        for (int i = 0; i < playTile.Length; i++)
        {
            bool dontDestroyTile = true;
            for (int j = 0; j < tileCounter; j++)
            {
                if (playTile[i].GetComponent<Playtile_SCR>().GetID() != tileSequence[j].GetComponent<Playtile_SCR>().GetID()) { continue; }
                else { dontDestroyTile = false; break; }
            }
            if (dontDestroyTile)
            {
                Destroy(playTile[i]);
                for (int tileIndex = 0; tileIndex < tileCounter; tileIndex++)
                {
                    prevTiles[tileIndex] = null;
                    Destroy(selectedTile[tileIndex]);
                }
            }
        }

        for (int i = 0; i < tileCounter; i++)
        {
            if (tileCounter == 0) { break; }
            StartCoroutine(EasingFunctions.TranslateTo(tileSequence[i], TilePosition[i, 2], 0.5f, 3, Easing.EaseInOut));
        }

        TLS = TimelineScript.CalculateTileSequence;
        timeVal = 1f;
        timeline_running = true;
    }

    private IEnumerator CheckEnemyStatus(bool commandUsed = false)
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] == null) { continue; }

            string enemyName = Enemies[i].GetComponent<Enemy_SCR>().GetName();
            int enemyHP = Enemies[i].GetComponent<Enemy_SCR>().GetHP();
            if (Enemies[i].GetComponent<Enemy_SCR>().GetHP() == 0 && Enemies[i].GetComponent<Enemy_SCR>().GetEnemyType() != Enemy_SCR.Enemy.Apple)
            {
                WriteMessage(GetDeathQuote(enemyName), true);
                Enemies[i].GetComponent<Enemy_SCR>().EnemyIsDefeated();
                yield return new WaitForSeconds(1.2f);
            }
        }
        if (!commandUsed)
        {
            TLS = TimelineScript.GoToEnemyTurn;
            timeVal = 1f;
            timeline_running = true;
        }
    }
    private bool EnemyDeadInEnemyTurn()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            GameObject Enemy = Enemies[i];
            if (Enemy != null && Enemy.GetComponent<Enemy_SCR>().GetHP() == 0) { return true; }
        }
        return false;
    }
    private IEnumerator CalculateTileSequence()
    {
        TileMethod tileMethod = TileMethod.Attack;
        bool isMultiAttacking = false;
        int ATK = Player1.ATK;
        int DEF = Player1.DEF;
        int HP = 0;
        int CP = 0;
        int multiplier = 1;

        for (int i = 0; i < tileCounter; i++)
        {
            if (tileSequence[i].GetComponent<Playtile_SCR>().isTileNullified())
            {
                StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
                if (i == tileCounter - 1 || !tileSequence[i + 1].GetComponent<Playtile_SCR>().isTileNullified()) { yield return new WaitForSeconds(1f); }
                continue;
            }
            int tileType = (int)tileSequence[i].GetComponent<Playtile_SCR>().GetTileType();
            switch (tileType)
            {
                //Attack +1 Tile
                case 0:
                    {
                        tileMethod = TileMethod.Attack;
                        ATK += 1;
                        break;
                    }
                //Attack +2 Tile
                case 1:
                    {
                        tileMethod = TileMethod.Attack;
                        ATK += 2;
                        break;
                    }
                //Attack +3 Tile
                case 2:
                    {
                        tileMethod = TileMethod.Attack;
                        ATK += 3;
                        break;
                    }
                //Attack +4 Tile
                case 3:
                    {
                        tileMethod = TileMethod.Attack;
                        ATK += 4;
                        break;
                    }
                //Multi-Attack +1 Tile
                case 4:
                    {
                        tileMethod = TileMethod.MultiAttack;
                        isMultiAttacking = true;
                        ATK += 1;
                        break;
                    }
                //Multi-Attack  +2 Tile
                case 5:
                    {
                        tileMethod = TileMethod.MultiAttack;
                        isMultiAttacking = true;
                        ATK += 2;
                        break;
                    }
                //Guard Tile
                case 6:
                    {
                        tileMethod = TileMethod.Guard;
                        break;
                    }
                //Heart Points +5 Tile
                case 7:
                    {
                        tileMethod = TileMethod.HealHP;
                        HP += 5;
                        break;
                    }
                //Command Points +1 Tile
                case 8:
                    {
                        tileMethod = TileMethod.HealCP;
                        CP += 1;
                        break;
                    }
                //Command Points +2 Tile
                case 9:
                    {
                        tileMethod = TileMethod.HealCP;
                        CP += 2;
                        break;
                    }
                //Command Tile
                case 10:
                    {
                        tileMethod = TileMethod.AddCommand;
                        break;
                    }
                //x2 Tile
                case 11:
                    {
                        tileMethod = TileMethod.Multiply;
                        multiplier *= 2;
                        break;
                    }
                //x3 Tile
                case 12:
                    {
                        tileMethod = TileMethod.Multiply;
                        multiplier *= 3;
                        break;
                    }
                //Element Swap Tile
                case 13:
                    {
                        tileMethod = TileMethod.SwapElement;
                        break;
                    }
                //Attack +0 Tile
                case 20:
                    {
                        tileMethod = TileMethod.Attack;
                        ATK += 0;
                        break;
                    }
                //Multi-Attack +0 Tile
                case 21:
                    {
                        tileMethod = TileMethod.MultiAttack;
                        isMultiAttacking = true;
                        ATK += 0;
                        break;
                    }
                //Command Points +0 Tile
                case 22:
                    {
                        tileMethod = TileMethod.HealCP;
                        CP += 0;
                        break;
                    }
                //Heart Points +2 Tile
                case 23:
                    {
                        tileMethod = TileMethod.HealHP;
                        HP += 2;
                        break;
                    }
                //x1 Tile
                case 24:
                    {
                        tileMethod = TileMethod.Multiply;
                        multiplier *= 1;
                        break;
                    }
            }

            if (i == tileCounter-1 || tileSequence[i + 1].GetComponent<Playtile_SCR>().isTileNullified() || !CheckTileMethod(tileMethod, tileSequence[i + 1]))
            {
                switch (tileMethod)
                {
                    case TileMethod.Attack:
                        {
                            DealDamage(ATK * multiplier, i, Player1.LUCK);
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    case TileMethod.MultiAttack:
                        {
                            for (int tries = 0; tries < 3; tries++)
                            {
                                DealDamage(ATK * multiplier, i, Player1.LUCK + 8);
                                yield return new WaitForSeconds(0.15f);
                            }
                            isMultiAttacking = false;
                            yield return new WaitForSeconds(0.85f);
                            break;
                        }
                    case TileMethod.Guard:
                        {
                            Player1.DEF_Plus = 2;
                            GameManager_SCR.PlaySound(7);
                            Vector3 BoardPos = PlayerBoard.transform.position;
                            OtherFunctions.CreateObjectFromResource("Prefabs/Defend_PFX", BoardPos + new Vector3(396f, -540f, -405f));
                            StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    case TileMethod.HealHP:
                        {
                            AddHP(HP * multiplier, i);
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    case TileMethod.HealCP:
                        {
                            AddCP(CP * multiplier, i);
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    case TileMethod.AddCommand:
                        {
                            Player1.AddCommand();
                            GameManager_SCR.PlaySound(37);
                            StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    case TileMethod.Multiply:
                        {
                            if (ATK > 0)
                            {
                                if (isMultiAttacking)
                                {
                                    for (int tries = 0; tries < 3; tries++)
                                    {
                                        DealDamage(ATK * multiplier, i, Player1.LUCK + 8);
                                        yield return new WaitForSeconds(0.15f);
                                    }
                                    isMultiAttacking = false;
                                    yield return new WaitForSeconds(0.853f);
                                }
                                else
                                {
                                    DealDamage(ATK * multiplier, i, Player1.LUCK);
                                    yield return new WaitForSeconds(1f);
                                    isMultiAttacking = false;
                                }
                            }
                            else if (DEF > 0)
                            {
                                AddDefense(DEF * multiplier, i);
                                yield return new WaitForSeconds(1f);
                            }
                            else if (HP > 0)
                            {
                                AddHP(HP * multiplier, i);
                                yield return new WaitForSeconds(1f);
                            }
                            else if (CP > 0)
                            {
                                AddCP(CP * multiplier, i);
                                yield return new WaitForSeconds(1f);
                            }
                            else
                            {
                                StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
                                yield return new WaitForSeconds(1f);
                            }
                            break;
                        }
                    case TileMethod.SwapElement:
                        {
                            SwapElement(i);
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                }
                ATK = Player1.ATK;
                DEF = Player1.DEF;
                HP = 0;
                CP = 0;
                multiplier = 1;
            }
            if (tileSequence[i].GetComponent<Playtile_SCR>().isTileNullified())
            {
                continue;
            }
        }

        StartCoroutine(CheckEnemyStatus());
    }

    private bool CheckTileMethod(TileMethod method, GameObject tile)
    {
        switch (method)
        {
            case TileMethod.Attack:
                {
                    return tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.ATKPlus0
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.ATKPlus1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.ATKPlus2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.ATKPlus3
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.ATKPlus4
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul3;
                }
            case TileMethod.MultiAttack:
                {
                    return tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.MulATKPlus0
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.MulATKPlus1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.MulATKPlus2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul3;
                }
            case TileMethod.HealHP:
                {
                    return tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.HPPlus2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.HPPlus5
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul3;
                }
            case TileMethod.HealCP:
                {
                    return tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.CPPlus0
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.CPPlus1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.CPPlus2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul3;
                }
            case TileMethod.AddCommand:
                {
                    return false;
                }
            case TileMethod.Multiply:
                {
                    return tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul1
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul2
                        || tile.GetComponent<Playtile_SCR>().GetTileType() == Playtile_SCR.Tile.Mul3;
                }
            case TileMethod.SwapElement:
                {
                    return false;
                }
            default: { return false; }
        }
    }

    private void DealDamage(int baseDamage, int index, int baseLUCK = 0)
    {
        GameObject Enemy;
        int enemyHP;
        float elementMultiplier;
        int tempEnemyCounter = 0;

        do
        {
            if (Player1.ContainsAilment(MainPlayer.StatusAilment.Confused)) { targetNum = UnityEngine.Random.Range(0, 6); }
            if (tempEnemyCounter > 0) { targetNum = ++targetNum % enemyCounter; }
            Enemy = Enemies[targetNum];
            while (Enemy == null) { targetNum = ++targetNum % enemyCounter; Enemy = Enemies[targetNum]; }
            enemyHP = Enemy.GetComponent<Enemy_SCR>().GetHP();
            elementMultiplier = DamageMultiplier[(int)Player1.mainElement, Enemies[targetNum].GetComponent<Enemy_SCR>().GetElement()];
            tempEnemyCounter++;
            if (tempEnemyCounter > enemyCounter)
            {
                for (int i = 0; i < tileCounter; i++)
                {
                    if (i > index) { break; }
                    if (i != index) { StartCoroutine(EasingFunctions.TranslateTo(tileSequence[i], tileSequence[index].transform.position, 0.4f)); }
                    StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
                }
                return;
            }
        }
        while (enemyHP == 0 && Enemy != null);

        baseDamage = (int)(baseDamage * Player1.ATK_Multiplier);
        //if (baseDamage > enemyHP)
        //{
        //    if (enemyHP == 0) { return; }

        //    int carryDamage = (baseDamage - enemyHP);
        //    baseDamage -= (baseDamage - enemyHP);
        //    do { targetNum = ++targetNum % enemyCounter; }
        //    while (Enemies[targetNum] == null);
        //    DealDamage(carryDamage, index);
        //}
        
        for (int i = 0; i < tileCounter; i++)
        {
            if (i > index) { break; }
            if (i != index) { StartCoroutine(EasingFunctions.TranslateTo(tileSequence[i], tileSequence[index].transform.position, 0.4f)); }
            StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
        }

        WriteMessage($"Player attacks!", true);

        GameObject AttackArrow = OtherFunctions.CreateObjectFromResource("Prefabs/AttackArrow_PFB", tileSequence[index].transform.position);
        AttackArrow.GetComponent<AttackArrow_SCR>().AcquireTarget(Enemy, (int)Player1.mainElement);

        if (Player1.ContainsAilment(MainPlayer.StatusAilment.Confused) && RandomChance(20))
        {
            float halfDistX = (tileSequence[index].transform.position.x - Enemy.transform.position.x) / 2f;
            float halfDistY = (tileSequence[index].transform.position.y - Enemy.transform.position.y) / 2f;
            Vector3 halfDisplacement = new Vector3(halfDistX, halfDistY, 0f);
            AttackArrow.GetComponent<AttackArrow_SCR>().TransferDamage(baseDamage, 0);
            StartCoroutine(AttackArrow.GetComponent<AttackArrow_SCR>().BetrayPlayer());
        }
        else
        {
            AttackArrow.GetComponent<AttackArrow_SCR>().TransferDamage(baseDamage, baseLUCK);
            StartCoroutine(EasingFunctions.TranslateTo(AttackArrow, Enemy.transform.position, 0.5f, 3, Easing.EaseIn));
        }
    }

    private void AddDefense(int baseDefense, int index)
    {
        Player1.DEF += baseDefense;
        for (int i = 0; i < tileSequence.Length; i++)
        {
            if (i > index) { break; }
            if (i != index) { StartCoroutine(EasingFunctions.TranslateTo(tileSequence[i], tileSequence[index].transform.position, 0.4f)); }
            StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
        }
    }
    private void AddHP(int baseHP, int index)
    {
        AddHPToPlayer(baseHP);
        for (int i = 0; i < tileSequence.Length; i++)
        {
            if (i > index) { break; }
            if (i != index) { StartCoroutine(EasingFunctions.TranslateTo(tileSequence[i], tileSequence[index].transform.position, 0.4f)); }
            StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
        }
    }
    private void AddCP(int baseCP, int index)
    {
        AddCPToPlayer(baseCP);
        for (int i = 0; i < tileSequence.Length; i++)
        {
            if (i > index) { break; }
            if (i != index) { StartCoroutine(EasingFunctions.TranslateTo(tileSequence[i], tileSequence[index].transform.position, 0.4f)); }
            StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[i], "#ffffff", 0.5f, 0f));
        }
    }

    private void SwapElement(int index)
    {
        int elementIndex = tileSequence[index].GetComponent<Playtile_SCR>().GetElementIndex();

        switch (elementIndex)
        {
            case 13: { Player1.ChangeElement(MainPlayer.Element.Null); break; }
            case 14: { Player1.ChangeElement(MainPlayer.Element.Fire); break; }
            case 15: { Player1.ChangeElement(MainPlayer.Element.Water); break; }
            case 16: { Player1.ChangeElement(MainPlayer.Element.Electric); break; }
            case 17: { Player1.ChangeElement(MainPlayer.Element.Wood); break; }
            case 18: { Player1.ChangeElement(MainPlayer.Element.Light); break; }
            case 19: { Player1.ChangeElement(MainPlayer.Element.Dark); break; }
        }
        StartCoroutine(EasingFunctions.ColorChangeFromHex(tileSequence[index], "#ffffff", 0.5f, 0f));
    }

    public void AddEXP(int exp) { EXP_Reward += exp; }

    public void AddHPToPlayer(int HP)
    {
        Player1.HP += HP;
        GameManager_SCR.PlaySound(8);
        Vector3 BoardPos = PlayerBoard.transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/HP_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 1018f, -101f));
        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 2);
    }
    public void AddCPToPlayer(int CP)
    {
        Player1.CP += CP;
        GameManager_SCR.PlaySound(9);
        Vector3 BoardPos = PlayerBoard.transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/CP_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 1018f, -101f));
        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 3);
    }
    public void AddATKToPlayer()
    {
        int currentLvl = (Player1.ATK_LevelNum == 6 ? Player1.ATK_LevelNum : ++Player1.ATK_LevelNum);
        if (currentLvl < 6) { Player1.ATK = Player1.ATK_Levels[currentLvl]; }
        Player1.AddStatusAilment(MainPlayer.StatusAilment.ATK_Up, 2);

        Vector3 BoardPos = PlayerBoard.transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/ATK_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 1018f, -101f));
        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 0);
    }
    public void AddDEFToPlayer()
    {
        int currentLvl = (Player1.DEF_LevelNum == 6 ? Player1.DEF_LevelNum : ++Player1.DEF_LevelNum);
        if (currentLvl < 6) { Player1.DEF = Player1.DEF_Levels[currentLvl]; }
        Player1.AddStatusAilment(MainPlayer.StatusAilment.DEF_Up, 2);

        Vector3 BoardPos = PlayerBoard.transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/DEF_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 1018f, -101f));
        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 1);
    }
    public void AddLUCKToPlayer()
    {
        int currentLvl = (Player1.LUCK_LevelNum == 3 ? Player1.LUCK_LevelNum : ++Player1.LUCK_LevelNum);
        if (currentLvl < 3) { Player1.LUCK = Player1.LUCK_Levels[currentLvl]; }
        Debug.Log($"LUCK: {Player1.LUCK}");
        Player1.AddStatusAilment(MainPlayer.StatusAilment.Lucky, 2);

        Vector3 BoardPos = PlayerBoard.transform.position;
        if (!GameObject.Find("LUCK_PFX")) { OtherFunctions.CreateObjectFromResource("Prefabs/LUCK_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 1018f, -101f)); }
        GameObject StatsText = OtherFunctions.CreateObjectFromResource("Prefabs/StatsConditionText_PFB", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
        OtherFunctions.ChangeSprite(StatsText, "Sprites/GameplayUI/StatsConditionText", 4);
    }

    public IEnumerator ReceiveDamage(int damage, bool ignoreDefense = false, GameObject enemy = null)
    {
        int fullDamage = 0;
        fullDamage = Mathf.Max(fullDamage, 0, damage - (Player1.DEF + Player1.DEF_Plus));
        Player1.HP -= fullDamage;
        shakeUI = true;
        HUD_vibration = fullDamage * 2f;
        if (enemy != null && Player1.ContainsAilment(MainPlayer.StatusAilment.Revenge)) { enemy.GetComponent<Enemy_SCR>().ReceiveDamage(Mathf.CeilToInt(fullDamage / 2f)); }
        if (enemy != null && Player1.ContainsAilment(MainPlayer.StatusAilment.TouchDamage)) { enemy.GetComponent<Enemy_SCR>().ReceiveDamage(1, true); }
        if (fullDamage == 0) { WriteMessage($"The attack had no effect on you!", true); }
        else
        {
            TilerIcon.GetComponent<Tiler_SCR>().ChangeEmote(Tiler_SCR.Emote.Hurt);
            TilerIcon.GetComponent<Tiler_SCR>().Vibrate(fullDamage * 2);
        }

        if (fullDamage > 0 && fullDamage <= 5) { GameManager_SCR.PlaySound(27); }
        else if (fullDamage > 5 && fullDamage <= 10) { GameManager_SCR.PlaySound(28); }
        else if (fullDamage > 10) { GameManager_SCR.PlaySound(29); }
        else { GameManager_SCR.PlaySound(30); }

        bool isDanger = (((float)Player1.HP / (float)Player1.MaxHP) <= 0.25f);
        if (isDanger) { GameManager_SCR.PlaySound(19); }
        yield return new WaitForSeconds(1f);
    }

    public int GetHPDrained(int damage)
    {
        int fullDamage = 0;
        fullDamage = Mathf.Max(fullDamage, 0, damage - (Player1.DEF + Player1.DEF_Plus));
        return fullDamage;
    }

    private void BattleControls()
    {
        bool relCoordFound = true;
        if (EnemyTarget == null) { EnemyTarget = Enemies[0]; targetNum = 0; }
        if (!CanMoveSelection()) { selectCooldownTime -= 1f * Time.deltaTime; }
        selectCooldownTime = Mathf.Clamp(selectCooldownTime, 0f, 10f);

        if (PlayerControls.GetButtonDown("ConfirmTS"))
        {
            if (playerMode == Mode.Select)
            {
                GameManager_SCR.PlaySound(3);
                CreateSelection(TilePosition[tileBlockX, tileBlockY], 104f, 112f, new Color(0f,0f,1f,1f), 1f, 8f);
                playerMode = Mode.Sequence;
                Debug.Log("Start sequence");
            }
            else if (playerMode == Mode.Sequence && tileCounter >= 4)
            {
                GameManager_SCR.PlaySound(3);
                FormTileSequence();
            }
            else if (playerMode == Mode.DecideCommand)
            {
                if (CommandPrompt.GetComponent<CommandPrompt_SCR>().GetChoice() == 0)
                {
                    bool hasEnoughCP = (Player1.CP >= Player1.CommandSlots[Player1.selectedCmdNum - 1].GetComponent<Command_SCR>().GetCost());

                    if (hasEnoughCP)
                    {
                        GameManager_SCR.PlaySound(14);
                        StartCoroutine(Player1.UseCommand(Player1.selectedCmdNum - 1));
                        allowBattleControls = false;
                        Player1.Reassign();
                        Player1.CommandSlots[Player1.GetSlotNumber()].GetComponent<Command_SCR>().EraseStats();
                        Player1.SetSlotNumber(0);
                        playerMode = prevPlayerMode;
                    }
                    else
                    {
                        GameManager_SCR.PlaySound(13);
                        return;
                    }
                }
                else if (CommandPrompt.GetComponent<CommandPrompt_SCR>().GetChoice() == 1)
                {
                    GameManager_SCR.PlaySound(14);
                    Player1.Reassign();
                    Player1.CommandSlots[Player1.GetSlotNumber()].GetComponent<Command_SCR>().EraseStats();
                    Player1.SetSlotNumber(0);
                    playerMode = prevPlayerMode;
                }
                else
                {
                    GameManager_SCR.PlaySound(12);
                    playerMode = prevPlayerMode;
                }
                Destroy(CommandPrompt);
            }
        }
        if (PlayerControls.GetButtonDown("SwitchTarget") && playerMode != Mode.DecideCommand)
        {
            int prevEnemyNum = targetNum;
            do { targetNum = ++targetNum % enemyCounter; }
            while (Enemies[targetNum] == null);
            
            EnemyTarget = Enemies[targetNum];
            if (Enemies[prevEnemyNum] != null)
            {
                if (Enemies[prevEnemyNum].GetComponent<Enemy_SCR>().isFlashing()) { Enemies[prevEnemyNum].GetComponent<Enemy_SCR>().StopFlashing(); }
            }
            Enemies[targetNum].GetComponent<Enemy_SCR>().Flash();
        }
        if (PlayerControls.GetButtonDown("Command1") && playerMode != Mode.DecideCommand)
        {
            const int cmdNum = 0;

            if (Player1.MetConditions(cmdNum))
            {
                prevPlayerMode = playerMode;
                Player1.SetSlotNumber(1);
                playerMode = Mode.DecideCommand;
                bool hasEnoughCP = (Player1.CP >= Player1.CommandSlots[cmdNum].GetComponent<Command_SCR>().GetCost());

                CommandPrompt = OtherFunctions.CreateObjectFromResource("Prefabs/CommandPrompt_PFB", new Vector3(960f, 540f, -500f));
                CommandPrompt.GetComponent<CommandPrompt_SCR>().TransferCommandInfo(Player1.GetCmdName(cmdNum), Player1.GetCmdDesc(cmdNum), Player1.GetCmdCost(cmdNum),
                                                                                    Player1.GetCmdAffect(cmdNum), Player1.GetCmdRarity(cmdNum), Player1.GetCmdColor(cmdNum), hasEnoughCP);
            }
            else { Player1.CommandAlert(cmdNum); }
        }
        if (PlayerControls.GetButtonDown("Command2") && playerMode != Mode.DecideCommand)
        {
            const int cmdNum = 1;

            if (Player1.MetConditions(cmdNum))
            {
                prevPlayerMode = playerMode;
                Player1.SetSlotNumber(2);
                playerMode = Mode.DecideCommand;
                bool hasEnoughCP = (Player1.CP >= Player1.CommandSlots[cmdNum].GetComponent<Command_SCR>().GetCost());

                CommandPrompt = OtherFunctions.CreateObjectFromResource("Prefabs/CommandPrompt_PFB", new Vector3(960f, 540f, -500f));
                CommandPrompt.GetComponent<CommandPrompt_SCR>().TransferCommandInfo(Player1.GetCmdName(cmdNum), Player1.GetCmdDesc(cmdNum), Player1.GetCmdCost(cmdNum),
                                                                                    Player1.GetCmdAffect(cmdNum), Player1.GetCmdRarity(cmdNum), Player1.GetCmdColor(cmdNum), hasEnoughCP);
            }
            else { Player1.CommandAlert(cmdNum); }
        }
        if (PlayerControls.GetButtonDown("ExitTS"))
        {
            if (playerMode == Mode.Sequence)
            {
                for (int i = 0; i < 4; i++)
                {
                    tileSequence[i] = null;
                    prevTiles[i] = null;
                    Destroy(selectedTile[i]);
                }
                tileCounter = 0;
                playerMode = Mode.Select;
            }
            else if (playerMode == Mode.DecideCommand)
            {
                GameManager_SCR.PlaySound(12);
                playerMode = prevPlayerMode;
                Destroy(CommandPrompt);
            }
        }
        if (PlayerControls.GetButton("SelectUpTS"))
        {
            if (playerMode != Mode.DecideCommand)
            {
                if (tileBlockY > 0 && CanMoveSelection())
                {
                    if (CheckUnselectedTile("up"))
                    {
                        GameManager_SCR.PlaySound(4);
                        if (Player1.messyControls)
                        {
                            if (Player1.messUpCounter == 2) { RandomizeTileCoordinate(ref relCoordFound); }
                            else { tileBlockY -= 1; }
                            Player1.messUpCounter++;
                        }
                        else { tileBlockY -= 1; }

                        selectCooldownTime = 0.2f;
                        if (playerMode == Mode.Sequence)
                        {
                            if (relCoordFound) { CreateSelection(TilePosition[tileBlockX, tileBlockY], 104f, 112f, new Color(0f, 0f, 1f, 1f), 1f, 8f); }
                        }
                    }
                }
            }
        }
        if (PlayerControls.GetButtonDown("SelectUpTS"))
        {
            if (playerMode == Mode.DecideCommand)
            {
                GameManager_SCR.PlaySound(11);
                CommandPrompt.GetComponent<CommandPrompt_SCR>().SelectUp();
            }
        }
        if (PlayerControls.GetButton("SelectDownTS"))
        {
            if (playerMode != Mode.DecideCommand)
            {
                if (tileBlockY < 3 && CanMoveSelection())
                {
                    if (CheckUnselectedTile("down"))
                    {
                        GameManager_SCR.PlaySound(4);
                        if (Player1.messyControls)
                        {
                            if (Player1.messUpCounter == 2) { RandomizeTileCoordinate(ref relCoordFound); }
                            else { tileBlockY += 1; }
                            Player1.messUpCounter++;
                        }
                        else { tileBlockY += 1; }

                        selectCooldownTime = 0.2f;
                        if (playerMode == Mode.Sequence)
                        {
                            if (relCoordFound) { CreateSelection(TilePosition[tileBlockX, tileBlockY], 104f, 112f, new Color(0f, 0f, 1f, 1f), 1f, 8f); }
                        }
                    }
                }
            }
        }
        if (PlayerControls.GetButtonDown("SelectDownTS"))
        {
            if (playerMode == Mode.DecideCommand)
            {
                GameManager_SCR.PlaySound(11);
                CommandPrompt.GetComponent<CommandPrompt_SCR>().SelectDown();
            }
        }
        if (PlayerControls.GetButton("SelectRightTS") && tileBlockX < 3 && CanMoveSelection() && playerMode != Mode.DecideCommand)
        {
            if (CheckUnselectedTile("right"))
            {
                GameManager_SCR.PlaySound(4);
                if (Player1.messyControls)
                {
                    if (Player1.messUpCounter == 2) { RandomizeTileCoordinate(ref relCoordFound); }
                    else { tileBlockX += 1; }
                    Player1.messUpCounter++;
                }
                else { tileBlockX += 1; }

                selectCooldownTime = 0.2f;
                if (playerMode == Mode.Sequence)
                {
                    if (relCoordFound) { CreateSelection(TilePosition[tileBlockX, tileBlockY], 104f, 112f, new Color(0f, 0f, 1f, 1f), 1f, 8f); }
                }
            }
        }
        if (PlayerControls.GetButton("SelectLeftTS") && tileBlockX > 0 && CanMoveSelection() && playerMode != Mode.DecideCommand)
        {
            if (CheckUnselectedTile("left"))
            {
                GameManager_SCR.PlaySound(4);
                if (Player1.messyControls)
                {
                    if (Player1.messUpCounter == 2) { RandomizeTileCoordinate(ref relCoordFound); }
                    else { tileBlockX -= 1; }
                    Player1.messUpCounter++;
                }
                else { tileBlockX -= 1; }

                selectCooldownTime = 0.2f;
                if (playerMode == Mode.Sequence)
                {
                    if (relCoordFound) { CreateSelection(TilePosition[tileBlockX, tileBlockY], 104f, 112f, new Color(0f, 0f, 1f, 1f), 1f, 8f); }
                }
            }
        }

        tileBlockX = Mathf.Clamp(tileBlockX, 0, 3);
        tileBlockY = Mathf.Clamp(tileBlockY, 0, 3);
        //Update tile selector position

        TileSelector.transform.position = new Vector3(TilePosition[tileBlockX, tileBlockY].x, TilePosition[tileBlockX, tileBlockY].y, -110f);
    }

    private bool CanMoveSelection() { return (selectCooldownTime == 0f); }

    public void CreateSelection(Vector3 pos, float width, float height, Color col, float scale = 1f, float padding = 0f)
    {
        if (ignoreAddingTiles) { ignoreAddingTiles = false; return; }
        tileSequence[tileCounter] = GetTile();
        prevTiles[tileCounter] = tileSequence[tileCounter];

        if (tileCounter > 1)
        {
            Debug.Log($"Prev ID: {prevTiles[tileCounter - 2].GetComponent<Playtile_SCR>().GetID()}");
            Debug.Log($"Now ID: {tileSequence[tileCounter].GetComponent<Playtile_SCR>().GetID()}");
            if (prevTiles[tileCounter - 2].GetComponent<Playtile_SCR>().GetID() == tileSequence[tileCounter].GetComponent<Playtile_SCR>().GetID())
            {
                Debug.Log("Delete tile!");
                for (int i = 0; i < 2; i++)
                {
                    tileSequence[tileCounter - i] = null;
                    prevTiles[tileCounter - i] = null;
                    Destroy(selectedTile[tileCounter - i]);
                }
                tileCounter--;
                return;
            }
        }

        selectedTile[tileCounter] = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", pos);
        selectedTile[tileCounter].AddComponent<SelectedTile_SCR>();
        selectedTile[tileCounter].transform.SetParent(tileSequence[tileCounter].transform);

        tileCounter++;
    }

    private void PlayerDisplay()
    {
        if (Player1 != null)
        {
            if (PlayerElement == null)
            {
                PlayerElement = GameObject.Find("Player Element");
                OtherFunctions.ChangeSprite(PlayerElement, "Sprites/GameplayUI/Elements", (int)Player1.mainElement);
            }
            else { OtherFunctions.ChangeSprite(PlayerElement, "Sprites/GameplayUI/Elements", (int)Player1.mainElement); }
            DisplayHiddenBars();
            FormatText();
        }
    }

    private void DisplayHiddenBars()
    {
        bool isDanger = (((float)Player1.HP / (float)Player1.MaxHP) <= 0.25f);
        bool almostTimeOver = CircleTimer.fillAmount <= 0.25f;

        HP_HideBar.fillAmount = 1f - ((float)Player1.HP / (float)Player1.MaxHP);
        CP_HideBar.fillAmount = 1f - ((float)Player1.CP / (float)Player1.MaxCP);
        CircleTimer.fillAmount = Player1.penaltyTime / Player1.MaxPenaltyTime;
        EXPBar.fillAmount = ((float)Player1.EXP / (float)Player1.MaxEXP);

        if (isDanger)
        {
            float damageTint = ((Mathf.Sin(2f * DateTime.Now.Millisecond / 150f) + 1));
            damageTint = Mathf.Clamp(damageTint, 0.1f, 1f);
            HPBar.GetComponent<SpriteRenderer>().color = new Color(1f, damageTint, damageTint, 1f);
        }
        else { HPBar.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); }
        if (almostTimeOver && Player1.penaltyTime > 0f) { CircleTimer.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f); }
        else if (GM.GetComponent<GameManager_SCR>().PenaltyTimerEnabled()) { CircleTimer.GetComponent<Image>().color = new Color(0f, 138f/255f, 1f, 1f); }
    }
    private void FormatText()
    {
        Timer_Text.text = $"{Mathf.CeilToInt(Player1.penaltyTime).ToString()}";
        HP_Text.text = $"{Player1.HP.ToString()}/{Player1.MaxHP.ToString()}";
        CP_Text.text = $"{Player1.CP.ToString()}/{Player1.MaxCP.ToString()}";
        LevelNum_Text.text = $"Lv. {Player1.Level.ToString()}";
    }

    public IEnumerator ResumeTurn()
    {
        StartCoroutine(CheckEnemyStatus());
        yield return new WaitForSeconds(1.5f);
        allowBattleControls = true;
        WriteMessage($"Form a 4-tile sequence!", false);
    }
    private void StartBattle()
    {
        timeline_running = true;
    }

    public void FadeOutTiles()
    {
        for (int i = 0; i < playTile.Length; i++) { StartCoroutine(EasingFunctions.ColorChangeFromHex(playTile[i], "#ffffff", 0.5f, 0.5f)); }
    }
    public void FadeInTiles()
    {
        for (int i = 0; i < playTile.Length; i++) { StartCoroutine(EasingFunctions.ColorChangeFromHex(playTile[i], "#ffffff", 0.5f, 1f)); }
    }

    private void SpawnEnemy()
    {
        List<List<EnemyStats>> tempEnemyList = new List<List<EnemyStats>>();
        string enemyName;
        char firstChar;
        string enemyResource = "";
        int numOfEmeniesToSpawn = 0;

        if (worldNum == 1)
        {
            switch (levelNum)
            {
                case 1: { tempEnemyList = World1_1_Waves; break; }
                case 2: { tempEnemyList = World1_2_Waves; break; }
                case 3: { tempEnemyList = World1_3_Waves; break; }
            }
        }
        else if (worldNum == 2)
        {
            switch (levelNum)
            {
                case 1: { tempEnemyList = World2_1_Waves; break; }
                case 2: { tempEnemyList = World2_2_Waves; break; }
            }
        }

        enemyResource = LookupEnemy(tempEnemyList[waveNum]);
        numOfEmeniesToSpawn = tempEnemyList[waveNum].Count;

        if (inBossBattle) { Enemies[enemyCounter] = OtherFunctions.CreateObjectFromResource("Prefabs/Enemy_PFB", BossLocations[1].transform.position); }
        else  { Enemies[enemyCounter] = OtherFunctions.CreateObjectFromResource("Prefabs/Enemy_PFB", SpawnLocations[enemyCounter].transform.position); }

        OtherFunctions.ChangeSprite(Enemies[enemyCounter], enemyResource, 0);
        Enemies[enemyCounter].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        Enemies[enemyCounter].GetComponent<Enemy_SCR>().AssignStats((int)tempEnemyList[waveNum][enemyCounter].GetEnemy(), tempEnemyList[waveNum][enemyCounter].GetTurnCounter(), enemyCounter + 1);
        enemyName = Enemies[enemyCounter].GetComponent<Enemy_SCR>().GetName();
        firstChar = enemyName.ToLower().ToCharArray()[0];

        if (vowels.IndexOf(firstChar) != -1) { WriteMessage($"An {enemyName} has appeared!", true); }
        else { WriteMessage($"A {enemyName} has appeared!", true); }
        StartCoroutine(TypingMessage);

        StartCoroutine(EasingFunctions.ColorChangeFromHex(Enemies[enemyCounter], "#ffffff", 1f));

        enemyCounter++;
        if (enemyCounter >= numOfEmeniesToSpawn) { allEnemiesSpawned = true; }
    }

    private string GetDeathQuote(string enemyName)
    {
        int quoteNum = UnityEngine.Random.Range(0, 6);

        switch (quoteNum)
        {
            case 0: { return $"The {enemyName} was defeated!"; }
            case 1: { return $"The {enemyName} fainted!"; }
            case 2: { return $"The {enemyName} died!"; }
            case 3: { return $"The {enemyName} was terminated!"; }
            case 4: { return $"The {enemyName} lost the battle!"; }
            case 5: { return $"The {enemyName} became tame!"; }
            default: { return $"The {enemyName} was defeated!"; }
        }
    }

    public void WriteMessage(string msg, bool typewriterMode)
    {
        if (TypingMessage != null) { StopCoroutine(TypingMessage); }
        TypingMessage = TypeMessage(msg, typewriterMode);
        StartCoroutine(TypingMessage);
    }
    private IEnumerator TypeMessage(string msg, bool typewriterMode)
    {
        //Clear message in message box
        ClearMessage();

        if (typewriterMode)
        {
            float time = 0.4f;
            for (int i = 0; i < msg.Length; i++)
            {
                if (time > 0.05f)
                {
                    GameManager_SCR.PlaySound(15);
                    time = 0f;
                }
                Message_Text.text += msg[i];
                time += 1f * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            GameManager_SCR.StopSound(15);
        }
        else { Message_Text.text = msg; }
    }

    public void ClearMessage() { Message_Text.text = ""; }

    private string LookupEnemy(List<EnemyStats> enemyList)
    {
        Enemy enemy = enemyList[enemyCounter].GetEnemy();
        switch (enemy)
        {
            case Enemy.Froopa: { return "Sprites/Enemies/Froopa"; }
            case Enemy.Nutbug: { return "Sprites/Enemies/Nutbug"; }
            case Enemy.Reshroom: { return "Sprites/Enemies/Reshroom"; }
            case Enemy.Apple: { return "Sprites/Enemies/Apple"; }
            case Enemy.Snapple: { return "Sprites/Enemies/Snapple"; }
            case Enemy.Punchey: { return "Sprites/Enemies/Punchey"; }
            case Enemy.Furb: { return "Sprites/Enemies/Furb"; }
            case Enemy.Slugshroom: { return "Sprites/Enemies/Slugshroom"; }
            case Enemy.Octavine: { return "Sprites/Enemies/Octavine"; }
            case Enemy.Charco: { return "Sprites/Enemies/Charco"; }
            case Enemy.Pyroma: { return "Sprites/Enemies/Pyroma"; }
            case Enemy.EyeDye1: { return "Sprites/Enemies/EyeDye1"; }
            case Enemy.EyeDye2: { return "Sprites/Enemies/EyeDye2"; }
            case Enemy.EyeDye3: { return "Sprites/Enemies/EyeDye3"; }
            case Enemy.EyeDye4: { return "Sprites/Enemies/EyeDye4"; }
            case Enemy.EyeDye5: { return "Sprites/Enemies/EyeDye5"; }
            case Enemy.EyeDye6: { return "Sprites/Enemies/EyeDye6"; }
            case Enemy.Pokerface: { return "Sprites/Enemies/Pokerface"; }
            case Enemy.MancalaSnake: { return "Sprites/Enemies/MancalaSnake"; }
            case Enemy.Devol: { return "Sprites/Enemies/Devol"; }
            case Enemy.Eighter: { return "Sprites/Enemies/Eighter_Phase1"; }
            case Enemy.Lemonster: { return "Sprites/Enemies/Lemonster"; }
            case Enemy.Bubblegoo: { return "Sprites/Enemies/Bubblegoo"; }
            case Enemy.Honeygoo: { return "Sprites/Enemies/Honeygoo"; }
            case Enemy.Margarette: { return "Sprites/Enemies/Margarette"; }
            case Enemy.Freebee: { return "Sprites/Enemies/Freebee"; }
            default: { return ""; }
        }
    }
    private void CreatePlayerGrid(List<TileAlgorithm_SCR.Tile> playerGrid, bool removeGridFromSequence = true)
    {
        //Create 4x4 grid (16 tiles) in Player Board
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int index = (i * 4) + j;
                playTile[index] = OtherFunctions.CreateObjectFromResource("Prefabs/Playtile_PFB", new Vector3(TilePosition[j, i].x, TilePosition[j, i].y, -106f));
                playTile[index].GetComponent<Playtile_SCR>().SetID(index+1);
                playTile[index].GetComponent<Playtile_SCR>().AssignTileType((int)playerGrid[index]);
                playTile[index].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                playTile[index].transform.SetParent(PlayerBoard.transform);

                StartCoroutine(EasingFunctions.ColorChangeFromHex(playTile[index], "#ffffff", 0.5f, 0.5f));
            }
        }
        if (removeGridFromSequence) { TileAlgorithm.GetComponent<TileAlgorithm_SCR>().RemoveGridFromSequence(); }
    }
    public void SetPlaytile(int index, int tileTypeNum, int frameIndex = 0)
    {
        playTile[index].GetComponent<Playtile_SCR>().AssignTileType(tileTypeNum);
        Playtile_SCR.Tile currentTileType = playTile[index].GetComponent<Playtile_SCR>().GetTileType();

        if (currentTileType == Playtile_SCR.Tile.ElementSwap) { playTile[index].GetComponent<Playtile_SCR>().AssignTileType(tileTypeNum, frameIndex); }
        else if (currentTileType == Playtile_SCR.Tile.ATKPlus0) { playTile[index].GetComponent<Playtile_SCR>().AssignTileType(20, 0); }
        else if (currentTileType == Playtile_SCR.Tile.MulATKPlus0) { playTile[index].GetComponent<Playtile_SCR>().AssignTileType(21, 0); }
        else if (currentTileType == Playtile_SCR.Tile.CPPlus0) { playTile[index].GetComponent<Playtile_SCR>().AssignTileType(22, 0); }
        else if (currentTileType == Playtile_SCR.Tile.HPPlus2) { playTile[index].GetComponent<Playtile_SCR>().AssignTileType(23, 0); }
        else if (currentTileType == Playtile_SCR.Tile.Mul1) { playTile[index].GetComponent<Playtile_SCR>().AssignTileType(24, 0); }
    }

    public GameObject GetTile()
    {
        int index = (tileBlockY * 4) + tileBlockX;
        index = Mathf.Clamp(index, 0, 15);
        return playTile[index];
    }
    public GameObject[] GetPlayerGrid()
    {
        return playTile;
    }
    public GameObject GetPlayerBoard()
    {
        return PlayerBoard;
    }

    public void DestroyGrid()
    {
        for (int i = 0; i < playTile.Length; i++)
        {
            if (playTile[i] != null) { Destroy(playTile[i]); }
        }
    }

    private bool CheckUnselectedTile(string direction)
    {
        if (tileCounter < 4) { return true; }
        int blockH = tileBlockX;
        int blockV = tileBlockY;
        direction = direction.ToLower();

        if (direction == "up") { blockV = tileBlockY - 1; }
        else if (direction == "down") { blockV = tileBlockY + 1; }
        else if (direction == "right") { blockH = tileBlockX + 1; }
        else if (direction == "left") { blockH = tileBlockX - 1; }

        blockH = Mathf.Clamp(blockH, 0, 3);
        blockV = Mathf.Clamp(blockV, 0, 3);

        int index = (blockV * 4) + blockH;
        index = Mathf.Clamp(index, 0, 15);
        GameObject unselectedTile = playTile[index];
        if (prevTiles[tileCounter - 2].GetComponent<Playtile_SCR>().GetID() == unselectedTile.GetComponent<Playtile_SCR>().GetID())
        {
            ignoreAddingTiles = true;
            for (int i = 0; i < 1; i++)
            {
                tileSequence[tileCounter - 1] = null;
                prevTiles[tileCounter - 1] = null;
                Destroy(selectedTile[tileCounter - 1]);
            }
            tileCounter--;
            return true;
        }
        else { return false; }
    }

    private bool OnLastWave()
    {
        if (worldNum == 1)
        {
            switch (levelNum)
            {
                case 1: { return waveNum == World1_1_Waves.Count - 1; }
                case 2: { return waveNum == World1_2_Waves.Count - 1; }
                case 3: { return waveNum == World1_3_Waves.Count - 1; }
            }
        }
        else if (worldNum == 2)
        {
            switch (levelNum)
            {
                case 1: { return waveNum == World2_1_Waves.Count - 1; }
                case 2: { return waveNum == World2_2_Waves.Count - 1; }
            }
        }
        return false;
    }

    private void ResetVariablesInTurn()
    {
        EnemyTarget = Enemies[targetNum];
        while (EnemyTarget == null) { targetNum = ++targetNum % enemyCounter; EnemyTarget = Enemies[targetNum]; }
        playerMode = Mode.Select;
        Player1.DEF = 0;
        Player1.penaltyTime = Player1.MaxPenaltyTime;
        Player1.DEF_Plus = 0;
        Array.Clear(tileSequence, 0, tileSequence.Length);
        Array.Clear(prevTiles, 0, prevTiles.Length);
        Array.Clear(selectedTile, 0, selectedTile.Length);
        tileCounter = 0;
        tileBlockX = 0;
        tileBlockY = 0;
    }
    private void ResetVariablesInWave()
    {
        playerMode = Mode.Select;
        Player1.DEF = 0;
        allEnemiesSpawned = false;
        EXP_Reward = 0;
        targetNum = 0;
        enemyCounter = 0;
        Player1.penaltyTime = Player1.MaxPenaltyTime;
        Player1.DEF_Plus = 0;
        Array.Clear(tileSequence, 0, tileSequence.Length);
        Array.Clear(prevTiles, 0, prevTiles.Length);
        Array.Clear(selectedTile, 0, selectedTile.Length);
        tileCounter = 0;
        tileBlockX = 0;
        tileBlockY = 0;
    }

    public int GetNumberOfEnemies() { return numOfEnemies; }
    public int GetPlayerElement() { return (int)Player1.mainElement; }
    public MainPlayer GetPlayer() { return Player1; }
    public GameObject GetEnemyTarget() { return EnemyTarget; }
    public void GetEnemies(ref List<GameObject> enemyList)
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            GameObject enemy = Enemies[i];
            if (enemy != null) { enemyList.Add(enemy); }
        }
    }
    private bool AllEnemiesDefeated() { return numOfEnemies == 0; }
    private void CreateWaveNodes()
    {
        GameObject WaveNodesParent = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(1357.5f, 180f, 0f));
        WaveNodesParent.name = $"WaveNodes";
        List<List<EnemyStats>> tempEnemyList = new List<List<EnemyStats>>();

        if (worldNum == 1)
        {
            switch (levelNum)
            {
                case 1: { tempEnemyList = World1_1_Waves; break; }
                case 2: { tempEnemyList = World1_2_Waves; break; }
                case 3: { tempEnemyList = World1_3_Waves; break; }
            }
        }
        else if (worldNum == 2)
        {
            switch (levelNum)
            {
                case 1: { tempEnemyList = World2_1_Waves; break; }
                case 2: { tempEnemyList = World2_2_Waves; break; }
            }
        }

        for (int i = 0; i < tempEnemyList.Count; i++)
        {
            WaveNodes.Add(OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(1357.5f, 180f, -5f)));

            if (i != tempEnemyList.Count-1)
            {
                OtherFunctions.ChangeSprite(WaveNodes[i], "Sprites/TinyPixel");
                WaveNodes[i].transform.localScale = new Vector3(16f, 16f, 1f);
            }
            else { OtherFunctions.ChangeSprite(WaveNodes[i], "Sprites/GameplayUI/BossIcon"); }

            WaveNodes[i].name = $"WaveNode {i + 1}";
            WaveNodes[i].transform.SetParent(WaveNodesParent.transform);
        }

        float spacing = 96f;
        float middleNum = WaveNodes.Count / 2;
        if (WaveNodes.Count % 2 != 0) //Odd number
        {
            int middleInt = (int)middleNum - 1;
            for (int i = middleInt; i >= 0; i--) { WaveNodes[i].transform.position -= new Vector3(spacing * ((middleInt + 1) - i), 0f, 0f); }
            for (int i = middleInt + 2; i < WaveNodes.Count; i++) { WaveNodes[i].transform.position += new Vector3(spacing * (i - (middleInt + 1)), 0f, 0f); }
        }
        //Even number
        else
        {
            int middleInt = (int)middleNum - 1;
            for (int i = middleInt; i >= 0; i--) { WaveNodes[i].transform.position -= new Vector3(spacing * (middleInt - i) + (spacing / 2f), 0f, 0f); }
            for (int i = middleInt + 1; i < WaveNodes.Count; i++) { WaveNodes[i].transform.position += new Vector3(spacing * (i - middleInt) - (spacing / 2f), 0f, 0f); }
        }
        Vector3 nodeOnePos = WaveNodes[0].transform.position;
        TilerIcon = OtherFunctions.CreateObjectFromResource("Prefabs/Tiler_PFB", new Vector3(nodeOnePos.x, nodeOnePos.y, -110f));
    }

    private void LevelUpCap()
    {
        if (Player1.EXP >= Player1.MaxEXP)
        {
            GameManager_SCR.PlaySound(38);
            Vector3 BoardPos = PlayerBoard.transform.position;

            Player1.Level++;
            Player1.MaxHP += 4;
            Player1.MaxCP += 2;
            Player1.MaxEXP = Player1.EXP_Cap[Player1.Level-1];
            OtherFunctions.CreateObjectFromResource("Prefabs/LevelUp_PFX", new Vector3(BoardPos.x + 420f, BoardPos.y - 320f, -500f));
            Player1.EXP = 0;
        }
        Player1.EXP %= Player1.MaxEXP;
    }
    private bool RandomChance(int percentage) { return (UnityEngine.Random.Range(0, 100) < percentage); }

    private void RandomizeTileCoordinate(ref bool initiatedRelCoord)
    {
        int[] numArray = new int[2] { -1, 1 };
        int number = numArray[UnityEngine.Random.Range(0, 2)];
        if (RandomChance(50))
        {
            bool outOfRange = (tileBlockX + number > 3) || (tileBlockX + number < 0);
            if (!outOfRange) { tileBlockX += number; }
            else { initiatedRelCoord = false; }
        }
        else
        {
            bool outOfRange = (tileBlockY + number > 3) || (tileBlockY + number < 0);
            if (!outOfRange) { tileBlockY += number; }
            else { initiatedRelCoord = false; }
        }
        Player1.messUpCounter %= 2;
    }

    private void SetBattleBackground()
    {
        switch (worldNum)
        {
            case 1:
                {
                    OtherFunctions.ChangeSprite(BattleBkg, $"Sprites/Theme Backgrounds/junglebackground{levelNum}");
                    break;
                }
            case 2:
                {
                    OtherFunctions.ChangeSprite(BattleBkg, $"Sprites/Theme Backgrounds/casinobackground{levelNum}");
                    break;
                }
        }
        BattleBkg.GetComponent<PanBackground_SCR>().ActivateScroll();
    }

    public void PlayBossMusic()
    {
        if (worldNum == 1)
        {
            switch (levelNum)
            {
                case 1: { GM.GetComponent<GameManager_SCR>().ChangeBGM("Audio/BackgroundMusic/Boss1_BGM"); break; }
                case 2: { GM.GetComponent<GameManager_SCR>().ChangeBGM("Audio/BackgroundMusic/Boss2_BGM"); break; }
                case 3: { GM.GetComponent<GameManager_SCR>().ChangeBGM("Audio/BackgroundMusic/Boss3_BGM"); break; }
            }
        }
        else if (worldNum == 2)
        {
            switch (levelNum)
            {
                case 1: { GM.GetComponent<GameManager_SCR>().ChangeBGM("Audio/BackgroundMusic/Boss4_BGM"); break; }
                case 2: { GM.GetComponent<GameManager_SCR>().ChangeBGM("Audio/BackgroundMusic/Boss5_BGM"); break; }
            }
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "BattleScreen_SCN":
                {
                    for (int i = 0; i < SpawnLocations.Length; i++) { SpawnLocations[i] = GameObject.Find($"SpawnLocation {i + 1}"); }
                    for (int i = 0; i < BossLocations.Length; i++) { BossLocations[i] = GameObject.Find($"BossLocation {i + 1}"); }
                    if (GameObject.Find("Battle Background")) { BattleBkg = GameObject.Find("Battle Background"); }
                    if (GameObject.Find("GameManager"))
                    {
                        GM = GameObject.Find("GameManager");
                        worldNum = GM.GetComponent<GameManager_SCR>().GetWorldNumber();
                        levelNum = GM.GetComponent<GameManager_SCR>().GetLevelNumber();
                    }
                    if (GameObject.Find("Player1 Board")) { PlayerBoard = GameObject.Find("Player1 Board"); }
                    if (GameObject.Find("BoardOverlay")) { BoardOverlay = GameObject.Find("BoardOverlay"); }
                    if (GameObject.Find("HP FullBar")) { HPBar = GameObject.Find("HP FullBar"); }
                    if (GameObject.Find("CP FullBar")) { CPBar = GameObject.Find("CP FullBar"); }
                    if (GameObject.Find("HP DamageBar")) { HP_HideBar = GameObject.Find("HP DamageBar").GetComponent<Image>(); }
                    if (GameObject.Find("CP HideBar")) { CP_HideBar = GameObject.Find("CP HideBar").GetComponent<Image>(); }
                    if (GameObject.Find("Timer")) { Timer = GameObject.Find("Timer"); }
                    if (GameObject.Find("EXP Bar")) { EXPBar = GameObject.Find("EXP Bar").GetComponent<Image>(); }
                    if (GameObject.Find("CircleTimer")) { CircleTimer = GameObject.Find("CircleTimer").GetComponent<Image>(); }
                    if (GameObject.Find("Timer")) { Timer_Text = GameObject.Find("Time Text").GetComponent<TMP_Text>(); }
                    if (GameObject.Find("HP Text")) { HP_Text = GameObject.Find("HP Text").GetComponent<TMP_Text>(); }
                    if (GameObject.Find("CP Text")) { CP_Text = GameObject.Find("CP Text").GetComponent<TMP_Text>(); }
                    if (GameObject.Find("Level# Text")) { LevelNum_Text = GameObject.Find("Level# Text").GetComponent<TMP_Text>(); }
                    if (GameObject.Find("Message Text")) { Message_Text = GameObject.Find("Message Text").GetComponent<TMP_Text>(); }
                    if (GameObject.Find("ReadyBar")) { ReadyBar = GameObject.Find("ReadyBar"); }
                    if (GameObject.Find("TechBorder1")) { techBorder[0] = GameObject.Find("TechBorder1"); }
                    if (GameObject.Find("TechBorder2")) { techBorder[1] = GameObject.Find("TechBorder2"); }
                    if (GameObject.Find("ReadyToPlay Text")) { ReadyToPlayText = GameObject.Find("ReadyToPlay Text"); }
                    break;
                }
        }
    }
}
