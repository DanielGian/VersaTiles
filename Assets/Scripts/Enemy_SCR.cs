using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EaseFunctions;
using TMPro;

public class Enemy_SCR : MonoBehaviour
{
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
        Freebee,
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
    public enum Card
    {
        Zero,
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
    }
    public enum TimelineScript
    {
        None = 0,
        Flash,
        TakenDamage,
        BossExplosion
    }
    private List<Color> EnemyColor = new List<Color>
    {
        new Color(0f, 128f/255f, 94f/255f, 1f),             //Froopa's primary color
        new Color(199f/255f, 170f/255f, 135f/255f, 1f),     //Nutbug's primary color
        new Color(133f/255f ,166f/255f, 1f, 1f),            //Reshroom's primary color
        new Color(196f/255f, 35f/255f, 35f/255f,1f),        //Snapple's primary color
        new Color(122f/255f, 80f/255f, 58f/255f,1f),        //Punchey's primary color
        new Color(155f/255f, 55f/255f, 1f,1f),              //Furb's primary color
        new Color(134f/255f, 181f/255f, 87f/255f,1f),       //Slugshroom's primary color
        new Color(0f, 91f/255f, 84f/255f,1f),               //Octavine's primary color
        new Color(41f/255f, 39f/255f, 38f/255f,1f),         //Charco's primary color
        new Color(1f, 251f/255f, 217f/255f,1f),             //Pyroma's primary color
        new Color(1f,1f,1f,1f),                             //Eye Dye's primary color
        new Color(1f, 82f/255f, 82f/255f, 1f),              //Pokerface's primary color
        new Color(1f, 197f/255f, 71f/255f, 1f),             //Mancala Snake's primary color
        new Color(254f/255f, 42f/255f, 139f/255f, 1f),      //Devol's primary color
        new Color(0.1f, 0.1f, 0.1f, 1f),                    //Eighter's primary color
        new Color(1f, 1f, 0f, 1f),                          //Lemonster's primary color
        new Color(99f/255f, 181f/255f, 1f, 1f),             //Bubblegoo's primary color
        new Color(1f, 216f/255f, 99f/255f, 1f),             //Honeygoo's primary color
        new Color(205f/255f, 1f, 105f/255f, 1f),            //Margarette's primary color
        new Color(1f, 208f/255f, 0f, 1f),                   //Freebee's primary color
        new Color(1f, 221f/255f, 0f, 1f),                   //Cardee Bee's primary color
    };
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
    private GameObject Obj;
    private GameObject StatsBar;
    private GameObject EnemyName_Text;
    private GameObject EnemyHP_Text;
    private GameObject EnemyHPBar;
    private GameObject EnemyHP_HideBar;
    private GameObject EnemyElement;
    private Image EnemyHP_HideBarMask;
    private GameObject Selection;
    private GameObject[] SelectTick = new GameObject[4];
    private GameObject TurnCounter;
    private GameObject TurnCounter_Text;
    private bool isDefeated = false;
    private GameObject GM;
    private GameObject BM;
    private SpriteRenderer SPR;
    private Material flashMat;
    private Material defaultMat;
    private List<StatusAilment> AilmentList = new List<StatusAilment>();
    private List<GameObject> AilmentBoxes = new List<GameObject>();
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
    private int ID = 0;
    [SerializeField] private string enemyName;
    [SerializeField] private Element mainElement;
    [SerializeField] private int HP;
    [SerializeField] private int MaxHP;
    [SerializeField] private int ATK;
    private int origATK;
    public readonly int[] ATK_Levels = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
    [SerializeField] private int ATK_LevelNum = 3;
    [SerializeField] private int DEF;
    private int origDEF;
    public readonly int[] DEF_Levels = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
    [SerializeField] private int DEF_LevelNum = 3;
    [SerializeField] private int LUCK = 0;
    private readonly int[] LUCK_Levels = new int[4] { 0, 10, 30, 50 };
    [SerializeField] private int LUCK_LevelNum = 0;
    [SerializeField] private int EXP;
    [SerializeField] private int turnsLeft;
    private const int maxNumOfStatusAilments = 3;
    private Enemy enemyType = Enemy.EyeDye1;
    private bool hasTakenDamage = false;
    private bool constantVibrate = false;
    private bool damagedInSleep = false;
    private bool isBadApple = false;
    private bool isFlinched = false;
    private bool isFlinchable = true;
    private bool bossEnemy = false;
    private bool sunlightCasted = false;
    private bool attackMode = true;

    //Pokerface variables
    private List<Card> Cards = new List<Card>();
    private int currentDay = 0;
    private const int promotionDay = 4;

    //Eighter variables
    private int hitCounter = 0;
    private int phaseNum = 0;
    private readonly int[] hitThreshold = new int[4] { 36, 30, 24, 18 };
    private readonly int[] damageThreshold = new int[4] { 1, 6, 10, 14 };
    private bool thresholdReached = false;

    //Timeline Variables
    private int SS_Animation_index = 0;
    private float timeVal = 2f;
    private readonly float timeValMax = 60f;
    private bool timeline_running = true;
    private TimelineScript TLS = TimelineScript.None;
    private bool isCoroutineRunning = false;
    private int flashCycle = 0;
    private const int flashCycleLimit = 4;
    private float vibration = 0f;
    private Vector3 staticPos;

    private void Awake()
    {
        Obj = this.gameObject;
        GM = GameObject.Find("GameManager");
        BM = GameObject.Find("BattleManager");
        SPR = Obj.GetComponent<SpriteRenderer>();
        flashMat = Resources.Load<Material>("Materials/Flash_MAT");
        defaultMat = SPR.material;
        staticPos = Obj.transform.position;
        StatsBar = OtherFunctions.CreateObjectFromResource("Prefabs/Enemy_StatsBar_PFB", Obj.transform.position + new Vector3(-132f, -144f, -105f));
        StatsBar.transform.Find("Stats").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        StatsBar.transform.Find("EnemyHPBar/EnemyHP FullBar").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        TurnCounter = OtherFunctions.CreateObjectFromResource("Prefabs/TurnCounter_PFB", Obj.transform.position + new Vector3(0f, 0f, -105f));
        TurnCounter.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HP = Mathf.Clamp(HP, 0, MaxHP);
        bool isDanger = (((float)HP / (float)MaxHP) <= 0.25f);

        EnemyHP_HideBarMask.fillAmount = 1f - ((float)HP / (float)MaxHP);

        if (isDanger)
        {
            float damageTint = ((Mathf.Sin(2f * DateTime.Now.Millisecond / 150f) + 1));
            damageTint = Mathf.Clamp(damageTint, 0.1f, 1f);
            EnemyHPBar.GetComponent<SpriteRenderer>().color = new Color(1f, damageTint, damageTint, 1f);
        }
        else { EnemyHPBar.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); }

        if (isDefeated)
        {
            vibration += 1f;
            SPR.material.color -= new Color(2f * Time.deltaTime, 2f * Time.deltaTime, 2f * Time.deltaTime, 0f);
            if (SPR.material.color.r <= 0f)
            {
                if (!bossEnemy)
                {
                    for (int starCount = 0; starCount < 16; starCount++)
                    {
                        float speed = (starCount < 8 ? 12f : 24f);
                        GameObject Star = OtherFunctions.CreateObjectFromResource("Prefabs/StarFX_PFB", staticPos - new Vector3(0f, 0f, -10f));
                        Star.GetComponent<StarFX_SCR>().AssignIndex(starCount, speed);

                    }
                    GameManager_SCR.PlaySound(40);
                    BM.GetComponent<BattleManager_SCR>().AddEXP(EXP);
                    Destroy(TurnCounter);
                    Destroy(StatsBar);
                    Destroy(Obj);
                }
                else
                {
                    timeline_running = true;
                    timeVal = 0f;
                    TLS = TimelineScript.BossExplosion;
                }
            }
        }

        timeVal = Mathf.Clamp(timeVal, 0f, timeValMax);
        if (timeline_running)
        {
            if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
            else { StartCoroutine(ExecuteTimeline(TLS)); }
        }

        if (hasTakenDamage)
        {
            float randX = UnityEngine.Random.Range(-vibration, vibration);
            float randY = UnityEngine.Random.Range(-vibration, vibration);
            Obj.transform.position = staticPos + new Vector3(randX, randY, 0f);

            if (HP > 0)
            {
                if (vibration > 0f) { vibration -= 0.25f; }
                else { vibration = 0; hasTakenDamage = false; }
            }
        }
        else { Obj.transform.position = staticPos; }
        FormatText();
    }
    IEnumerator ExecuteTimeline(TimelineScript timelinescript)
    {
        if (isCoroutineRunning) { yield break; }
        isCoroutineRunning = true;

        if (timelinescript == TimelineScript.Flash)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        SPR.material = flashMat;
                        timeVal = 0.2f;
                        break;
                    }
                case 2:
                    {
                        SPR.material = defaultMat;
                        SS_Animation_index = 0;
                        timeVal = 0.2f;
                        flashCycle++;
                        if (flashCycle >= flashCycleLimit)
                        {
                            flashCycle = 0;
                            TLS = TimelineScript.None;
                            BM.GetComponent<BattleManager_SCR>().ClearMessage();
                            Destroy(Selection);
                        }
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.BossExplosion)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        float i = 0f;
                        for (int starCount = 0; starCount < 144; starCount++)
                        {
                            float speed = (starCount < 8 ? 12f : 24f);
                            float size = UnityEngine.Random.Range(1f, 5f);
                            GameObject Star = OtherFunctions.CreateObjectFromResource("Prefabs/StarFX_PFB", staticPos - new Vector3(0f, 0f, -10f));
                            Star.transform.localScale = new Vector3(size, size, 1f);
                            Star.GetComponent<StarFX_SCR>().AssignIndex((float)starCount + i, speed);
                            if (starCount % 16 == 0 && starCount != 0)
                            {
                                int boomSFX = UnityEngine.Random.Range(43, 45);
                                GameManager_SCR.PlaySound(boomSFX);
                                yield return new WaitForSeconds(0.18f);
                                i += 0.5f;
                            }
                        }
                        timeVal = 2.5f;
                        break;
                    }
                case 2:
                    {
                        GameManager_SCR.PlaySound(41);
                        BM.GetComponent<BattleManager_SCR>().AddEXP(EXP);
                        OtherFunctions.CreateObjectFromResource("Prefabs/BossExplosion_PFX", staticPos - new Vector3(0f, 0f, -10f));
                        Destroy(TurnCounter);
                        Destroy(StatsBar);
                        Destroy(Obj);
                        break;
                    }
            }
        }

        isCoroutineRunning = false;
    }

    public void AssignStats(int enemyID, int turnNum, int spawnNum)
    {
        StatsBar.name = $"StatsBar {spawnNum}";
        EnemyHPBar = StatsBar.transform.Find("EnemyHPBar").gameObject;
        EnemyName_Text = StatsBar.transform.Find("Stats/Enemy Name").gameObject;
        EnemyName_Text.transform.position = StatsBar.transform.position + new Vector3(0f, 48f, -5f);
        EnemyName_Text.transform.SetParent(StatsBar.transform.Find("Stats").gameObject.transform);
        EnemyHP_Text = StatsBar.transform.Find("Stats/Enemy HP").gameObject;
        EnemyHP_Text.transform.position = StatsBar.transform.position + new Vector3(105f, 14f, -5f);
        EnemyHP_Text.transform.SetParent(StatsBar.transform.Find("Stats").gameObject.transform);
        EnemyHP_HideBar = StatsBar.transform.Find("EnemyHPBar/EnemyHP FullBar/EnemyHP DamageBar").gameObject;
        EnemyHP_HideBar.transform.position = StatsBar.transform.position + new Vector3(59f, -12f, -5f);
        EnemyHP_HideBar.transform.SetParent(StatsBar.transform.Find("EnemyHPBar/EnemyHP FullBar/EnemyHP DamageBar").gameObject.transform);
        EnemyHP_HideBarMask = EnemyHP_HideBar.GetComponent<Image>();
        EnemyElement = StatsBar.transform.Find("Element_PFB").gameObject;

        float spriteHalfWidth = SPR.bounds.extents.x;
        float spriteHalfHeight = SPR.bounds.extents.y;
        float offsetY = 0f;
        switch (enemyID)
        {
            case 18: { offsetY = -72f; break; }
            case 19: { offsetY = -108f; break; }
        }
        TurnCounter.transform.position += new Vector3(spriteHalfWidth, spriteHalfHeight + offsetY, 0f);
        TurnCounter.name = $"TurnCounter {spawnNum}";
        TurnCounter_Text = TurnCounter.transform.Find("Canvas/TurnCounterText").gameObject;
        TurnCounter_Text.transform.position = TurnCounter.transform.position + new Vector3(spriteHalfWidth, spriteHalfHeight - 5f + offsetY, -5f);
        TurnCounter_Text.transform.SetParent(TurnCounter.transform.Find("Canvas").gameObject.transform);


        switch (enemyID)
        {
            //Froopa
            case 0:
                {
                    EnemyStats("Froopa", Element.Wood, 10, 10, 2, 0, 12, turnNum, Enemy.Froopa, EnemyColor[0]);
                    break;
                }
            //Nutbug
            case 1:
                {
                    EnemyStats("Nutbug", Element.Wood, 8, 8, 2, 1, 8, turnNum, Enemy.Nutbug, EnemyColor[1]);
                    break;
                }
            //Reshroom
            case 2:
                {
                    EnemyStats("Reshroom", Element.Water, 12, 12, 4, 0, 15, turnNum, Enemy.Reshroom, EnemyColor[2]);
                    AddStatusAilment(StatusAilment.Sleep, -1);
                    break;
                }
            //Apple
            case 3:
                {
                    EnemyStats("Apple", Element.Wood, 1, 1, 1, 0, 1, turnNum, Enemy.Apple, EnemyColor[3]);
                    break;
                }
            //Snapple
            case 4:
                {
                    EnemyStats("Snapple", Element.Wood, 40, 40, 4, 1, 72, turnNum, Enemy.Snapple, EnemyColor[3], true);
                    break;
                }
            //Punchey
            case 5:
                {
                    EnemyStats("Punchey", Element.Wood, 15, 15, 4, 0, 32, turnNum, Enemy.Punchey, EnemyColor[4]);
                    break;
                }
            //Furb
            case 6:
                {
                    EnemyStats("Furb", Element.Dark, 8, 8, 2, 0, 18, turnNum, Enemy.Furb, EnemyColor[5]);
                    break;
                }
            //Slugshroom
            case 7:
                {
                    EnemyStats("Slugshroom", Element.Wood, 16, 16, 2, 0, 33, turnNum, Enemy.Slugshroom, EnemyColor[6]);
                    break;
                }
            //Octavine
            case 8:
                {
                    EnemyStats("Octavine", Element.Wood, 54, 54, 3, 1, 92, turnNum, Enemy.Octavine, EnemyColor[7], true);
                    break;
                }
            //Charco
            case 9:
                {
                    EnemyStats("Charco", Element.Fire, 15, 15, 4, 2, 50, turnNum, Enemy.Charco, EnemyColor[8]);
                    break;
                }
            //Pyroma
            case 10:
                {
                    EnemyStats("Pyroma", Element.Fire, 96, 96, 5, 0, 110, turnNum, Enemy.Pyroma, EnemyColor[9], true);
                    break;
                }
            //Eye Dye 1
            case 11:
                {
                    EnemyStats("Eye Dye", Element.Null, 9, 9, 1, 0, 21, turnNum, Enemy.EyeDye1, EnemyColor[10]);
                    break;
                }
            //Eye Dye 2
            case 12:
                {
                    EnemyStats("Eye Dye", Element.Null, 9, 9, 2, 0, 21, turnNum, Enemy.EyeDye2, EnemyColor[10]);
                    break;
                }
            //Eye Dye 3
            case 13:
                {
                    EnemyStats("Eye Dye", Element.Null, 9, 9, 3, 0, 21, turnNum, Enemy.EyeDye3, EnemyColor[10]);
                    break;
                }
            //Eye Dye 4
            case 14:
                {
                    EnemyStats("Eye Dye", Element.Null, 9, 9, 4, 0, 21, turnNum, Enemy.EyeDye4, EnemyColor[10]);
                    break;
                }
            //Eye Dye 5
            case 15:
                {
                    EnemyStats("Eye Dye", Element.Null, 9, 9, 5, 0, 21, turnNum, Enemy.EyeDye5, EnemyColor[10]);
                    break;
                }
            //Eye Dye 6
            case 16:
                {
                    EnemyStats("Eye Dye", Element.Null, 9, 9, 6, 0, 21, turnNum, Enemy.EyeDye6, EnemyColor[10]);
                    break;
                }
            //Pokerface
            case 17:
                {
                    EnemyStats("Pokerface", Element.Fire, 132, 132, 0, 0, 144, turnNum, Enemy.Pokerface, EnemyColor[11], true);
                    CreateCards();
                    break;
                }
            //Mancala Snake
            case 18:
                {
                    EnemyStats("Mancala Snake", Element.Light, 16, 16, 3, 2, 45, turnNum, Enemy.MancalaSnake, EnemyColor[12]);
                    break;
                }
            //Devol
            case 19:
                {
                    EnemyStats("Devol", Element.Dark, 28, 28, 3, 0, 66, turnNum, Enemy.Devol, EnemyColor[13]);
                    break;
                }
            //Eighter
            case 20:
                {
                    EnemyStats("Eighter", Element.Null, 120, 120, 8, 8, 1, turnNum, Enemy.Eighter, EnemyColor[14], true);
                    break;
                }
            //Lemonster
            case 21:
                {
                    EnemyStats("Lemonster", Element.Water, 18, 18, 5, 0, 39, turnNum, Enemy.Lemonster, EnemyColor[15]);
                    break;
                }
            //Bubblegoo
            case 22:
                {
                    EnemyStats("Bubblegoo", Element.Water, 20, 20, 2, 1, 32, turnNum, Enemy.Bubblegoo, EnemyColor[16]);
                    break;
                }
            //Honeygoo
            case 23:
                {
                    EnemyStats("Honeygoo", Element.Electric, 20, 20, 2, 1, 32, turnNum, Enemy.Honeygoo, EnemyColor[17]);
                    break;
                }
            //Margarette
            case 24:
                {
                    EnemyStats("Margarette", Element.Water, 30, 30, 2, 1, 64, turnNum, Enemy.Margarette, EnemyColor[18]);
                    break;
                }
            //Freebee
            case 25:
                {
                    EnemyStats("Freebee", Element.Electric, 12, 12, 2, 0, 25, turnNum, Enemy.Freebee, EnemyColor[19]);
                    break;
                }
        }
    }
    private void EnemyStats(string eName, Element element, int hp, int maxhp, int atk, int def, int exp, int turnNum, Enemy eType, Color barColor, bool isBossEnemy = false)
    {
        enemyName = eName;
        EnemyName_Text.GetComponent<TMP_Text>().text = eName;
        Obj.name = eName;
        mainElement = element;
        OtherFunctions.ChangeSprite(EnemyElement, "Sprites/GameplayUI/Elements", (int)mainElement);
        HP = hp;
        MaxHP = maxhp;
        EnemyHP_Text.GetComponent<TMP_Text>().text = $"{HP.ToString()}/{MaxHP.ToString()}";
        ATK = atk;
        origATK = atk;
        ATK_LevelNum = 3;
        DEF = def;
        origDEF = def;
        DEF_LevelNum = 3;
        EXP = exp;
        LUCK_LevelNum = 0;
        turnsLeft = turnNum;
        enemyType = eType;
        bossEnemy = isBossEnemy;
        StatsBar.GetComponent<SpriteRenderer>().color = barColor;
        TurnCounter.GetComponent<SpriteRenderer>().color = new Color(1f, 126f / 255f, 0f);
    }
    private void FormatText()
    {
        EnemyHP_Text.GetComponent<TMP_Text>().text = $"{HP.ToString()}/{MaxHP.ToString()}";
        TurnCounter_Text.GetComponent<TMP_Text>().text = $"{turnsLeft}";
    }

    public void Flash()
    {
        float spriteWidth = Obj.GetComponent<SpriteRenderer>().bounds.size.x;
        float spriteHeight = Obj.GetComponent<SpriteRenderer>().bounds.size.y;
        TLS = TimelineScript.Flash;
        timeVal = 0f;
        CreateSelection(spriteWidth, spriteHeight, new Color(1f, 1f, 1f, 1f), 2f);
        WriteMessage($"You're now targeting {enemyName}.", false);
    }
    public IEnumerator FlashTurn()
    {
        for (int iteration = 0; iteration < 8; iteration++)
        {
            Debug.Log($"Flash {iteration}");
            if (iteration % 2 == 0) { SPR.material = flashMat; }
            else { SPR.material = defaultMat; }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool isFlashing() { return TLS == TimelineScript.Flash; }

    public void StopFlashing()
    {
        SPR.material = defaultMat;
        SS_Animation_index = 0;
        timeVal = 0.2f;
        flashCycle = 0;
        TLS = TimelineScript.None;
        if (Selection != null) { Destroy(Selection); }
    }

    public bool ContainsAilment(StatusAilment SA)
    {
        for (int i = 0; i < GetAilmentCount(); i++)
        {
            if (AilmentList[i] == SA) { return true; }
        }
        return false;
    }
    public int GetAilmentCount() { return AilmentBoxes.Count; }
    public void AddStatusAilment(StatusAilment SA, int turnNum, bool _dontDeductTurn = false)
    {
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
            AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().PreventDefault(_dontDeductTurn);
        }
        else
        {
            AilmentList.Add(SA);
            AilmentBoxes.Add(OtherFunctions.CreateObjectFromResource("Prefabs/StatusAilmentIcon_PFB", Obj.transform.position + new Vector3(-96f, 96f + (index * 56f), -10f)));
            LookupAilment(index, SA);
            AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetTurns(turnNum);
            AilmentBoxes[index].transform.SetParent(Obj.transform);
            AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().PreventDefault(_dontDeductTurn);
        }
    }
    public void DecrementAilmentTurn()
    {
        bool containsStopAilment = ContainsAilment(StatusAilment.Stop);
        for (int i = 0; i < AilmentBoxes.Count; i++)
        {
            if (containsStopAilment)
            {
                if (AilmentList[i] == StatusAilment.Stop)
                {
                    AilmentBoxes[i].GetComponent<StatusAilmentIcon_SCR>().DecrementTurn();
                    break;
                }
            }
            else { AilmentBoxes[i].GetComponent<StatusAilmentIcon_SCR>().DecrementTurn(); }
        }
        for (int i = 0; i < AilmentBoxes.Count; i++)
        {
            bool repeatLoop = false;
            if (AilmentBoxes[i].GetComponent<StatusAilmentIcon_SCR>().isTurnZero()) { RemoveStatusAilment(i); repeatLoop = true; }
            if (repeatLoop) { i--; }
        }

    }
    public void RemoveStatusAilment(int boxNum)
    {
        StatusAilment SA = (StatusAilment)AilmentBoxes[boxNum].GetComponent<StatusAilmentIcon_SCR>().GetStatusAilment();
        switch (SA)
        {
            case StatusAilment.ATK_Up:
                {
                    ATK = origATK;
                    ATK_LevelNum = 3;
                    break;
                }
            case StatusAilment.DEF_Up:
                {
                    DEF = origDEF;
                    DEF_LevelNum = 3;
                    break;
                }
            case StatusAilment.Lucky:
                {
                    LUCK = LUCK + LUCK_Levels[0];
                    LUCK_LevelNum = 0;
                    break;
                }
        }
        Destroy(AilmentBoxes[boxNum]);
        AilmentList.RemoveAt(boxNum);
        AilmentBoxes.RemoveAt(boxNum);
        for (int i = 0; i < AilmentBoxes.Count; i++)
        {
            AilmentBoxes[i].transform.position = Obj.transform.position + new Vector3(-96f, 96f + (i * 56f), -10f);
        }
    }
    private void LookupAilment(int index, StatusAilment SA)
    {
        switch (SA)
        {
            case StatusAilment.ATK_Up:
                {
                    GameManager_SCR.PlaySound(32);
                    AddATKToEnemy();
                    AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(0);
                    OtherFunctions.CreateObjectFromResource("Prefabs/EnemyATK_PFX", Obj.transform.position + new Vector3(0f, -64f, 0f));
                    OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 0);
                    break;
                }
            case StatusAilment.DEF_Up:
                {
                    GameManager_SCR.PlaySound(33);
                    AddDEFToEnemy();
                    AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(1);
                    OtherFunctions.CreateObjectFromResource("Prefabs/EnemyDEF_PFX", Obj.transform.position + new Vector3(0f, -64f, 0f));
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
                    break;
                }
            case StatusAilment.Confused:
                {
                    GameManager_SCR.PlaySound(24);
                    AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(5);
                    OtherFunctions.CreateObjectFromResource("Prefabs/EnemyConfused_PFX", Obj.transform.position);
                    OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 5);
                    break;
                }
            case StatusAilment.Revenge:
                {
                    AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(6);
                    OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 6);
                    break;
                }
            case StatusAilment.HP_Regen:
                {
                    AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(10);
                    OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 10);
                    break;
                }
            case StatusAilment.CP_Regen:
                {
                    AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(11);
                    OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 11);
                    break;
                }
            case StatusAilment.Stop:
                {
                    AilmentBoxes[index].GetComponent<StatusAilmentIcon_SCR>().SetStatusAilment(12);
                    OtherFunctions.CreateObjectFromResource("Prefabs/Stop_PFX", Obj.transform.position);
                    OtherFunctions.ChangeSprite(AilmentBoxes[index], "Sprites/GameplayUI/StatusAilmentIcons", 12);
                    break;
                }
        }
    }
    public void ExecuteStatusAilments(int boxNum)
    {
        if (ContainsAilment(StatusAilment.Stop)) { return; }
        StatusAilment SA = (StatusAilment)AilmentBoxes[boxNum].GetComponent<StatusAilmentIcon_SCR>().GetStatusAilment();
        switch (SA)
        {
            case StatusAilment.Poisoned:
                {
                    ReceiveDamage(3);
                    break;
                }
            case StatusAilment.HP_Regen:
                {
                    StartCoroutine(AddHPToEnemy(4));
                    OtherFunctions.CreateObjectFromResource("Prefabs/EnemyHP_PFX", Obj.transform.position + new Vector3(0f, -64f, 0f));
                    break;
                }
            case StatusAilment.CP_Regen:
                {
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

    private void AddATKToEnemy()
    {
        int currentLvl = (ATK_LevelNum == 6 ? ATK_LevelNum : ++ATK_LevelNum);
        if (currentLvl < 6) { ATK = ATK + ATK_Levels[currentLvl]; }
        Debug.Log($"Attack: {ATK}");
    }
    private void AddDEFToEnemy()
    {
        int currentLvl = (DEF_LevelNum == 6 ? DEF_LevelNum : ++DEF_LevelNum);
        if (currentLvl < 6) { DEF = DEF + DEF_Levels[currentLvl]; }
        Debug.Log($"Defense: {DEF}");
    }

    public string GetName() { return enemyName; }
    public Enemy GetEnemyType() { return enemyType; }
    public int GetElement() { return (int)mainElement; }
    public int GetHP() { return HP; }
    public int GetDefense() { return DEF; }
    public void ReceiveDamage(int damage, bool ignoreDefense = false, bool flinchEnemy = false, float shakiness = -1, bool isLucky = false)
    {
        int fullDamage = 0;
        int playerElementNum = BM.GetComponent<BattleManager_SCR>().GetPlayerElement();
        float extraElementBuff = 0f;
        if (BM.GetComponent<BattleManager_SCR>().GetPlayer().SunlightActive())
        {
            if (playerElementNum == 1 || playerElementNum == 5) { extraElementBuff = 0.5f; }
            else if (playerElementNum == 2 || playerElementNum == 6) { extraElementBuff = -0.5f; }
        }
        float elementMultiplier = DamageMultiplier[playerElementNum, (int)mainElement] + extraElementBuff;

        if (!ignoreDefense) { fullDamage = Mathf.Max(fullDamage, 0, (int)(damage * elementMultiplier) - DEF); }
        else { fullDamage = Mathf.Max(fullDamage, 0, (int)(damage)); }
        HP -= fullDamage;

        if (ContainsAilment(StatusAilment.Revenge)) { StartCoroutine(BM.GetComponent<BattleManager_SCR>().ReceiveDamage(Mathf.CeilToInt(fullDamage / 2f))); }
        hasTakenDamage = true;
        if (flinchEnemy)
        {
            if (isFlinchable)
            {
                if (turnsLeft == 1) { isFlinched = true; }
                else
                {
                    isFlinchable = false;
                    WriteMessage($"The {enemyName} cannot be flinched!", true);
                }
            }
            else { WriteMessage($"The {enemyName} cannot be flinched!", true); }
        }
        if (fullDamage > 0 && fullDamage <= 5) { GameManager_SCR.PlaySound(27); }
        else if (fullDamage > 5 && fullDamage <= 10) { GameManager_SCR.PlaySound(28); }
        else if (fullDamage > 10) { GameManager_SCR.PlaySound(29); }
        else { GameManager_SCR.PlaySound(30); }

        if (enemyType == Enemy.Apple) { isBadApple = true; }
        else if (enemyType == Enemy.Eighter)
        {
            if (!Broken()) { CheckDefenseCondition(fullDamage); }
        }

        for (int i = 0; i < AilmentList.Count; i++)
        {
            if (AilmentList[i] == StatusAilment.Sleep) { damagedInSleep = true; break; }
        }
        GameObject HitFX = OtherFunctions.CreateObjectFromResource("Prefabs/Hit_PFX", new Vector3(Obj.transform.position.x, Obj.transform.position.y, -500f));
        var startColor = HitFX.GetComponent<ParticleSystem>().main;
        HitFX.transform.localScale = new Vector3(fullDamage * 0.25f, fullDamage * 0.25f, 1f);
        startColor.startColor = GetElementColor(playerElementNum);

        CreateDamageIcon(fullDamage);

        if (shakiness == -1) { vibration = fullDamage * 2f; }
        else { vibration = shakiness; }
        timeVal = 0f;
    }
    public void ReceiveConfusedDamage(int damage, float shakiness = -1)
    {
        int fullDamage = 0;
        float extraElementBuff = 0f;
        if (BM.GetComponent<BattleManager_SCR>().GetPlayer().SunlightActive())
        {
            if (mainElement == Element.Fire || mainElement == Element.Light) { extraElementBuff = 0.5f; }
        }
        int emenyElementNum = BM.GetComponent<BattleManager_SCR>().GetPlayerElement();
        float elementMultiplier = DamageMultiplier[emenyElementNum, (int)mainElement] + extraElementBuff;

        fullDamage = Mathf.Max(fullDamage, 0, (int)(damage * elementMultiplier) - DEF);
        HP -= fullDamage;

        if (ContainsAilment(StatusAilment.Revenge)) { HP -= Mathf.CeilToInt(fullDamage / 2f); }
        hasTakenDamage = true;
        if (enemyType == Enemy.Apple) { isBadApple = true; }

        GameObject HitFX = OtherFunctions.CreateObjectFromResource("Prefabs/Hit_PFX", new Vector3(Obj.transform.position.x, Obj.transform.position.y, -500f));
        var startColor = HitFX.GetComponent<ParticleSystem>().main;
        HitFX.transform.localScale = new Vector3(fullDamage * 0.25f, fullDamage * 0.25f, 1f);
        startColor.startColor = GetElementColor(emenyElementNum);

        CreateDamageIcon(fullDamage);

        if (shakiness == -1) { vibration = fullDamage * 2f; }
        else { vibration = shakiness; }
        timeVal = 0f;
    }

    public void EnemyIsDefeated()
    {
        if (!isDefeated)
        {
            isDefeated = true;
            SPR.material = flashMat;
        }
    }

    public bool DecrementTurn()
    {
        if (ContainsAilment(StatusAilment.Stop)) { return false; }
        turnsLeft--;
        if (enemyType == Enemy.Pokerface) { currentDay++; }
        if (turnsLeft == 0) { return true; }
        return false;
    }

    public float ChooseMove()
    {
        if (!isFlinched || enemyType == Enemy.Apple)
        {
            StartCoroutine(FlashTurn());
            if (enemyType != Enemy.Apple && ContainsAilment(StatusAilment.Confused) && RandomChance(50))
            {
                StartCoroutine(ConfusedHit()); return 2.5f;
            }
            else
            {
                switch (enemyType)
                {
                    //Froopa
                    case Enemy.Froopa: { StartCoroutine(Tackle()); return 2.5f; }
                    //Nutbug
                    case Enemy.Nutbug: { StartCoroutine(Ram()); return 2.5f; }
                    //Reshroom
                    case Enemy.Reshroom:
                        {
                            bool wakeUpRNG = RandomChance(50);
                            if (damagedInSleep && wakeUpRNG) { StartCoroutine(SleepAttack(true)); return 3.25f; }
                            else { StartCoroutine(SleepAttack(false)); return 1.5f; }
                        }
                    //Apple
                    case Enemy.Apple:
                        {
                            if (isBadApple) { StartCoroutine(Reveal()); return 6f; }
                            else { StartCoroutine(AppleWaiting()); return 4.25f; }
                        }
                    //Apple
                    case Enemy.Snapple:
                        {
                            if (RandomChance(50)) { StartCoroutine(Bite()); return 2f; }
                            else if (RandomChance(40))
                            {
                                int numOfHits = UnityEngine.Random.Range(2, 6);
                                StartCoroutine(SeedBarrage(numOfHits)); return 2f + (0.2f * numOfHits);
                            }
                            else { StartCoroutine(JuicyBite()); return 4f; }
                        }
                    //Punchey
                    case Enemy.Punchey:
                        {
                            StartCoroutine(SoftPunch()); return 2f;
                        }
                    //Furb
                    case Enemy.Furb:
                        {
                            bool poisonPlayerRNG = RandomChance(45);
                            StartCoroutine(PoisonBite(poisonPlayerRNG)); return 2.5f;
                        }
                    //Slugshroom
                    case Enemy.Slugshroom:
                        {
                            if (RandomChance(80))
                            {
                                bool poisonPlayerRNG = RandomChance(20);
                                StartCoroutine(SickLick(poisonPlayerRNG));
                                return 2.5f;
                            }
                            else { StartCoroutine(Sneeze()); return 2f; }
                        }
                    //Octavine
                    case Enemy.Octavine:
                        {
                            if (RandomChance(60))
                            {
                                StartCoroutine(TentacleSlap());
                                return 2.5f;
                            }
                            if (RandomChance(30))
                            {
                                StartCoroutine(TentacleDoubleSlap());
                                return 3f;
                            }
                            else { StartCoroutine(SoftSpore()); return 3.5f; }
                        }
                    //Charco
                    case Enemy.Charco:
                        {
                            if (RandomChance(50)) { StartCoroutine(Chomp()); return 2f; }
                            else
                            {
                                bool burnPlayerRNG = RandomChance(40);
                                StartCoroutine(FireBreath(burnPlayerRNG));
                                if (burnPlayerRNG) { return 3.5f; }
                                else { return 2.5f; }
                            }
                        }
                    //Pyroma
                    case Enemy.Pyroma:
                        {
                            if (!sunlightCasted)
                            {
                                sunlightCasted = true;
                                StartCoroutine(Sunlight()); return 7.5f;
                            }
                            else
                            {
                                if (RandomChance(33)) { StartCoroutine(FlyingRam()); return 2.5f; }
                                else if (RandomChance(33)) { StartCoroutine(ExplosiveSpore()); return 5.5f; }
                                else
                                {
                                    bool burnPlayerRNG = RandomChance(40);
                                    StartCoroutine(FireBreath(burnPlayerRNG));
                                    if (burnPlayerRNG) { return 3.5f; }
                                    else { return 2.5f; }
                                }
                                
                            }
                        }
                    //Eye Dye 1
                    case Enemy.EyeDye1: { StartCoroutine(DieAttack()); return 2f; }
                    //Eye Dye 2
                    case Enemy.EyeDye2: { StartCoroutine(DieAttack()); return 2f; }
                    //Eye Dye 3
                    case Enemy.EyeDye3: { StartCoroutine(DieAttack()); return 2f; }
                    //Eye Dye 4
                    case Enemy.EyeDye4: { StartCoroutine(DieAttack()); return 2f; }
                    //Eye Dye 5
                    case Enemy.EyeDye5: { StartCoroutine(DieAttack()); return 2f; }
                    //Eye Dye 6
                    case Enemy.EyeDye6: { StartCoroutine(DieAttack()); return 2f; }
                    //Pokerface
                    case Enemy.Pokerface:
                        {
                            if (currentDay >= promotionDay)
                            {
                                currentDay = 0;
                                if (RandomChance(40)) { StartCoroutine(ATK_Promotion()); return 3.5f; }
                                else if(RandomChance(40)) { StartCoroutine(DEF_Promotion()); return 3.5f; }
                                else if (RandomChance(40)) { StartCoroutine(Revenge_Promotion()); return 3.5f; }
                                else { StartCoroutine(Lovely_Promotion()); return 3.5f; }
                            }
                            else { StartCoroutine(DrawCard()); return 4f; }
                        }
                    //Mancala Snake
                    case Enemy.MancalaSnake:
                        {
                            if (RandomChance(75)) { StartCoroutine(ThrowStone()); return 2.5f; }
                            else { StartCoroutine(SpillStones()); return 4f; }
                        }
                    //Devol
                    case Enemy.Devol:
                        {
                            if (RandomChance(50)) { StartCoroutine(Claw()); return 2.5f; }
                            else { StartCoroutine(HP_Drain()); return 2.5f; }
                        }
                    //Eighter
                    case Enemy.Eighter:
                        {
                            if (Broken()) { StartCoroutine(Immovable()); return 1.5f; }
                            else { StartCoroutine(Rollover()); return 2.5f; }
                        }
                    //Lemonster
                    case Enemy.Lemonster:
                        {
                            StartCoroutine(Bite()); return 2.5f;
                        }
                    //Bubblegoo
                    case Enemy.Bubblegoo: { StartCoroutine(Throw()); return 2.5f; }
                    //Honeygoo
                    case Enemy.Honeygoo: { StartCoroutine(Throw()); return 2.5f; }
                    //Margarette
                    case Enemy.Margarette:
                        {
                            if (RandomChance(20))
                            {
                                bool freezePlayerRNG = RandomChance(40);
                                StartCoroutine(Freeze(freezePlayerRNG)); return 3.5f;
                            }
                            else if (RandomChance(20)) { StartCoroutine(StrongRefresh()); return 3.5f; }
                            else { StartCoroutine(Kiss()); return 3f; }
                        }
                    //Freebee
                    case Enemy.Freebee:
                        {
                            if (RandomChance(25)) { StartCoroutine(RapidSting()); return 2.5f; }
                            else { StartCoroutine(Sting()); return 2f; }
                        }
                    default: { return 1f; }
                }
            }
        }
        else
        {
            isFlinched = false;
            isFlinchable = false;
            WriteMessage($"The {enemyName} was flinched and cannot move!", true);
            turnsLeft = 1;
            return 1f;
        }
    }
    private IEnumerator AddHPToEnemy(int hp)
    {
        HP += hp;
        GameManager_SCR.PlaySound(8);
        CreateHPIcon(hp);
        OtherFunctions.CreateObjectFromResource("Prefabs/EnemyHP_PFX", Obj.transform.position);
        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#FF78C8", 0.5f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#FFFFFF", 0.5f, 1f));
    }
    private void DealDamage(int baseDamage, int luckyRate, bool ignoreDefense = false, bool isConfused = false)
    {
        var player = BM.GetComponent<BattleManager_SCR>().GetPlayer();
        int finalDamage = 0;

        if (isConfused)
        {
            ReceiveDamage(baseDamage);
        }
        else
        {
            float extraElementBuff = 0f;
            if (BM.GetComponent<BattleManager_SCR>().GetPlayer().SunlightActive())
            {
                if (mainElement == Element.Fire || mainElement == Element.Light) { extraElementBuff = 0.5f; }
            }
            float elementMultiplier = DamageMultiplier[(int)mainElement, BM.GetComponent<BattleManager_SCR>().GetPlayerElement()] + extraElementBuff;

            float luckyMultiplier = 1f;
            bool randomChance = RandomChance(luckyRate);
            if (randomChance) { luckyMultiplier = 2f; }

            finalDamage = (int)(baseDamage * elementMultiplier * luckyMultiplier);
            StartCoroutine(BM.GetComponent<BattleManager_SCR>().ReceiveDamage(finalDamage, ignoreDefense, Obj));
        }
    }

    private Color GetElementColor(int colorNum)
    {
        switch (colorNum)
        {
            case 0: { return new Color(218f / 255f, 218f / 255f, 218f / 255f, 1f); }
            case 1: { return new Color(1f, 144f / 255f, 0f, 1f); }
            case 2: { return new Color(0f, 156f / 255f, 1f, 1f); }
            case 3: { return new Color(1f, 234f / 255f, 0f, 1f); }
            case 4: { return new Color(21f / 255f, 222f / 255f, 0f, 1f); }
            case 5: { return new Color(1f, 238f / 255f, 170f / 255f, 1f); }
            case 6: { return new Color(106f / 255f, 0f, 196f / 255f, 1f); }
            default: { return new Color(1f, 1f, 1f, 1f); }
        }
    }

    private void WriteMessage(string msg, bool typewriterMode) { BM.GetComponent<BattleManager_SCR>().WriteMessage(msg, typewriterMode); }
    private void ClearMessage() { BM.GetComponent<BattleManager_SCR>().ClearMessage(); }


    private void CreateSelection(float width, float height, Color col, float scale = 1f, float padding = 0f)
    {
        Vector3 pos = Obj.transform.position;
        Selection = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", pos);

        for (int i = 0; i < SelectTick.Length; i++)
        {
            if (SelectTick[i] == null)
            {
                Vector3 relPos = new Vector3(0f, 0f, 0f);
                if (i == 0) { relPos = new Vector3((-width / 2f) - padding, (height / 2f) + padding, -2f); }        //Top-left corner
                else if (i == 1) { relPos = new Vector3((width / 2f) + padding, (height / 2f) + padding, -2f); }    //Top-right corner
                else if (i == 2) { relPos = new Vector3((-width / 2f) - padding, (-height / 2f) - padding, -2f); }  //Bottom-left corner
                else if (i == 3) { relPos = new Vector3((width / 2f) + padding, (-height / 2f) - padding, -2f); }   //Bottom-right corner

                SelectTick[i] = OtherFunctions.CreateObjectFromResource("Prefabs/SelectTick_PFB", pos + relPos);
                SelectTick[i].GetComponent<SelectTick_SCR>().SetTickBorder(i);
                SelectTick[i].transform.localScale = new Vector3(scale, scale, 1f);
                SelectTick[i].transform.SetParent(Selection.transform);
                SelectTick[i].GetComponent<SpriteRenderer>().color = col;
            }
        }

        Selection.transform.SetParent(Obj.transform);
    }

    private void CreateDamageIcon(int damage)
    {
        GameObject DamageIcon = OtherFunctions.CreateObjectFromResource("Prefabs/DamageIcon_PFB", new Vector3(Obj.transform.position.x, Obj.transform.position.y, -105f));
        DamageIcon.GetComponent<DamageIcon_SCR>().AssignDamage(damage);
    }
    private void CreateHPIcon(int hp)
    {
        GameObject HPIcon = OtherFunctions.CreateObjectFromResource("Prefabs/HeartIcon_PFB", new Vector3(Obj.transform.position.x, Obj.transform.position.y, -105f));
        HPIcon.GetComponent<HeartIcon_SCR>().AssignHP(hp);
    }

    private bool RandomChance(int percentage) { return (UnityEngine.Random.Range(0, 100) < percentage); }
    private IEnumerator Tackle()
    {
        GameManager_SCR.PlaySound(16);
        bool tripRNG = RandomChance(20);
        WriteMessage($"The {enemyName} tried to tackle you!", true);
        yield return new WaitForSeconds(1f);
        if (tripRNG) { WriteMessage($"Unfortunately, the {enemyName} tripped on himself!", true); }
        else { DealDamage(ATK, LUCK); }
        turnsLeft = UnityEngine.Random.Range(1, 3);
    }
    private IEnumerator Ram()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} rammed its hard body at you!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = 2;
    }
    private IEnumerator SleepAttack(bool isWoke)
    {
        if (isWoke)
        {
            ClearMessage();
            GameManager_SCR.PlaySound(45);
            OtherFunctions.CreateObjectFromResource("Prefabs/BubbleSign_PFB", Obj.transform.position + new Vector3(0f, 16f, 0f));
            yield return new WaitForSeconds(1f);
            WriteMessage($"The {enemyName} woke up!", true);
            yield return new WaitForSeconds(0.75f);
            GameManager_SCR.PlaySound(16);
            WriteMessage($"The {enemyName} fired a slimy bubble!", true);
            yield return new WaitForSeconds(1f);
            DealDamage(ATK, LUCK);
        }
        else
        {
            WriteMessage($"The {enemyName} continues to rest!", true);
            StartCoroutine(AddHPToEnemy(4));
            yield return new WaitForSeconds(1f);
        }
        damagedInSleep = false;
        turnsLeft = 1;
    }
    private IEnumerator AppleWaiting()
    {
        WriteMessage($"There's an {enemyName} on the ground!", true);
        yield return new WaitForSeconds(2f);
        WriteMessage($"It sure looks ripe and alluring!", true);
        yield return new WaitForSeconds(1.5f);
        turnsLeft = 1;
    }
    private IEnumerator Reveal()
    {
        hasTakenDamage = true;
        constantVibrate = true;
        vibration = 10f;
        yield return new WaitForSeconds(1f);
        WriteMessage($"The {enemyName} seems to be moving!", true);
        yield return new WaitForSeconds(1f);
        WriteMessage($"Wait a minute!?", true);
        yield return new WaitForSeconds(1f);
        WriteMessage($"This {enemyName} is a luring trap!", true);
        yield return new WaitForSeconds(1f);
        constantVibrate = false;
        OtherFunctions.ChangeSprite(Obj, "Sprites/Enemies/Snapple");
        AssignStats(4, 2, 0);
        WriteMessage($"The Snapple makes a surprising appearance!", true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(AudioFade_SCR.Fade(GM.GetComponent<GameManager_SCR>().GetAudioSrc(), 0.5f, 1f));
        BM.GetComponent<BattleManager_SCR>().PlayBossMusic();
    }
    private IEnumerator Bite()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} takes a quick bite on you!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = 1;
    }
    private IEnumerator JuicyBite()
    {
        GameManager_SCR.PlaySound(17);
        int[] victimTile = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        for (int i = victimTile.Length - 1; i > 0; i--)
        {
            // might have a range error based random number
            int j = Mathf.FloorToInt((UnityEngine.Random.value * (i + 1)) % victimTile.Length);
            int temp = victimTile[i];
            victimTile[i] = victimTile[j];
            victimTile[j] = temp;
        }

        WriteMessage($"The {enemyName} takes a nice, juicy bite on you!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        GameObject[] tempGrid = BM.GetComponent<BattleManager_SCR>().GetPlayerGrid();
        for (int i = 0; i < 6; i++)
        {
            GameObject Fang1 = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", tempGrid[victimTile[i]].transform.position);
            Fang1.AddComponent<BiteAnimation_SCR>();
            Fang1.GetComponent<BiteAnimation_SCR>().SetFrameIndex(0);
            GameObject Fang2 = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", tempGrid[victimTile[i]].transform.position);
            Fang2.AddComponent<BiteAnimation_SCR>();
            Fang2.GetComponent<BiteAnimation_SCR>().SetFrameIndex(1);
            Fang1.transform.position = tempGrid[victimTile[i]].transform.position + new Vector3(0f, 56f, 0f);
            Fang1.GetComponent<BiteAnimation_SCR>().SetTile(tempGrid[victimTile[i]]);
            Fang2.transform.position = tempGrid[victimTile[i]].transform.position + new Vector3(0f, -56f, 0f);
            Fang2.GetComponent<BiteAnimation_SCR>().SetTile(tempGrid[victimTile[i]]);
        }
        StartCoroutine(AddHPToEnemy(6));
        yield return new WaitForSeconds(1f);
        WriteMessage($"The {enemyName} regained some of its health from your tiles", true);
        yield return new WaitForSeconds(1.5f);
        turnsLeft = 2;
    }
    private IEnumerator SeedBarrage(int numOfHits)
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} ate several apples and attempted to shoot them!", true);
        yield return new WaitForSeconds(1f);
        for (int tries = 0; tries < numOfHits; tries++)
        {
            DealDamage(1, LUCK * tries);
            ClearMessage();
            yield return new WaitForSeconds(0.2f);
        }
        WriteMessage($"It landed {numOfHits} hits!", true);
        turnsLeft = 2;
    }
    private IEnumerator SoftPunch()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} threw a soft punch!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = UnityEngine.Random.Range(1, 3);
    }
    private IEnumerator PoisonBite(bool poisonPlayer)
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} bites you!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = 2;
        if (poisonPlayer)
        {
            BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Poisoned, 2);
            yield return new WaitForSeconds(0.5f);
            WriteMessage($"You got poisoned!", true);
        }
    }
    private IEnumerator SickLick(bool poisonPlayer)
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} gave you a sick lick!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = UnityEngine.Random.Range(1, 3);
        if (poisonPlayer)
        {
            BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Poisoned, 3);
            yield return new WaitForSeconds(0.5f);
            WriteMessage($"You got poisoned!", true);
        }
    }
    private IEnumerator Sneeze()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} delivered a contagious sneeze!", true);
        yield return new WaitForSeconds(1f);

        Vector3 BoardPos = GameObject.Find("Player1 Board").transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/Sneeze_PFX", BoardPos + new Vector3(396f, -540f, -405f));
        yield return new WaitForSeconds(1.9f);
        BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Confused, 2);
        WriteMessage($"Your controls are messed up!", true);
        turnsLeft = 2;
    }
    private IEnumerator TentacleSlap()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} swings a tentacle at you!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = 1;
    }
    private IEnumerator TentacleDoubleSlap()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} swings two tentacles at you!", true);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 2; i++)
        {
            DealDamage(ATK, LUCK);
            yield return new WaitForSeconds(0.5f);
        }
        turnsLeft = 2;
    }
    private IEnumerator SoftSpore()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} scattered its spores!", true);
        yield return new WaitForSeconds(1f);

        OtherFunctions.CreateObjectFromResource("Prefabs/Spores_PFX", Obj.transform.position + new Vector3(0f, 0f, -800f));
        yield return new WaitForSeconds(1f);

        GameObject[] tempGrid = BM.GetComponent<BattleManager_SCR>().GetPlayerGrid();
        for (int i = 0; i < tempGrid.Length; i++)
        {
            int tileTypeNum = (int)tempGrid[i].GetComponent<Playtile_SCR>().GetTileType();

            StartCoroutine(tempGrid[i].GetComponent<Playtile_SCR>().InfectTile(new Color(134f / 255f, 181f / 255f, 87f / 255f, 0.5f), tileTypeNum));
        }
        yield return new WaitForSeconds(1f);
        WriteMessage($"All of your tiles' value were reduced!", true);
        turnsLeft = 2;
    }
    private IEnumerator Chomp()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} takes a chomp on you!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK+1, LUCK);
        turnsLeft = 2;
    }
    private IEnumerator FireBreath(bool burnPlayer)
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} breathes fire!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK, true);

        if (burnPlayer)
        {
            BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Burned, 2);
            WriteMessage($"Your attack power is reduced by 1/2.", true);
        }
        turnsLeft = 2;
    }
    private IEnumerator Sunlight()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} casts a gleaming light in the sky!", true);
        yield return new WaitForSeconds(1f);
        GameObject Sunlight = OtherFunctions.CreateObjectFromResource("Prefabs/Sunlight_PFB", Obj.transform.position);
        Sunlight.GetComponent<Sunlight_SCR>().FlickerON();
        StartCoroutine(EasingFunctions.TranslateTo(Sunlight, Obj.transform.position + new Vector3(0f, 280f, 0f), 1.5f, 2, Easing.EaseOut));
        yield return new WaitForSeconds(3f);
        Sunlight.GetComponent<Sunlight_SCR>().ReleaseLight();
        GameManager_SCR.PlaySound(18);
        BM.GetComponent<BattleManager_SCR>().GetPlayer().EnableSunlight();
        WriteMessage($"Fire and Light type attacks are increased by half!", true);
        yield return new WaitForSeconds(1.5f);
        WriteMessage($"Water and Dark type attacks are decreased by half!", true);
        yield return new WaitForSeconds(1.5f);
        turnsLeft = 2;
    }
    private IEnumerator FlyingRam()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} flies straight at you!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = 1;
    }
    private IEnumerator ExplosiveSpore()
    {
        GameManager_SCR.PlaySound(17);
        int[] victimTile = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        int numOfVictimTiles = UnityEngine.Random.Range(4,9);
        for (int i = victimTile.Length - 1; i > 0; i--)
        {
            // might have a range error based random number
            int j = Mathf.FloorToInt((UnityEngine.Random.value * (i + 1)) % victimTile.Length);
            int temp = victimTile[i];
            victimTile[i] = victimTile[j];
            victimTile[j] = temp;
        }

        WriteMessage($"The {enemyName} scattered some explosive spores!", true);
        yield return new WaitForSeconds(1f);

        ClearMessage();
        GameObject[] tempGrid = BM.GetComponent<BattleManager_SCR>().GetPlayerGrid();
        OtherFunctions.CreateObjectFromResource("Prefabs/ExplosiveSpores_PFX", Obj.transform.position + new Vector3(0f, 0f, -800f));
        yield return new WaitForSeconds(1.5f);

        //DealDamage(1, 0);
        for (int i = 0; i < numOfVictimTiles; i++)
        {
            GameObject tile = tempGrid[victimTile[i]];
            tile.GetComponent<Playtile_SCR>().EnableGreyscale();
            OtherFunctions.CreateObjectFromResource("Prefabs/EnemyBurned_PFX", tile.transform.position);
        }
        yield return new WaitForSeconds(1f);
        WriteMessage($"Some of your tiles were scorched!", true);
        yield return new WaitForSeconds(1.5f);
        turnsLeft = 2;
    }
    private IEnumerator ConfusedHit()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} is confused and attacked itself!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, 0, false, true);
        turnsLeft = 1;
    }
    private IEnumerator DieAttack()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} rolled itself at you!", true);
        yield return new WaitForSeconds(1f);
        DealDamage(ATK, LUCK);
        turnsLeft = UnityEngine.Random.Range(1, 4);
    }
    private IEnumerator DrawCard()
    {
        GameManager_SCR.PlaySound(16);
        if (Cards.Count == 0) { CreateCards(); }
        int cardValue = (int)Cards[0];
        WriteMessage($"The {enemyName} draws a {cardValue} card!", true);
        yield return new WaitForSeconds(1f);
        GameObject Pokercard = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", Obj.transform.position + new Vector3(0f, 0f, -5f));
        OtherFunctions.ChangeSprite(Pokercard, "Sprites/GameplayUI/Pokercards", cardValue);
        StartCoroutine(EasingFunctions.TranslateTo(Pokercard, Obj.transform.position + new Vector3(0f, 64f, 0f), 0.5f, 3, Easing.EaseOut));
        yield return new WaitForSeconds(1f);
        StartCoroutine(EasingFunctions.ColorChangeFromHex(Pokercard, "#ffffff", 0.5f, 0f));
        DealDamage(cardValue + ATK, 0);
        turnsLeft = 1;
        yield return new WaitForSeconds(0.5f);
        RemoveCard();
        Destroy(Pokercard);
    }
    private IEnumerator ATK_Promotion()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} received a powerful promotion!", true);
        yield return new WaitForSeconds(1f);
        AddStatusAilment(StatusAilment.ATK_Up, 3, true);
        ClearMessage();
        WriteMessage($"The {enemyName}'s attack was increased by 2!", true);
        turnsLeft = 2;
    }
    private IEnumerator DEF_Promotion()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} received a defensive promotion!", true);
        yield return new WaitForSeconds(1f);
        AddStatusAilment(StatusAilment.DEF_Up, 3, true);
        ClearMessage();
        WriteMessage($"The {enemyName}'s defense was increased by 2!", true);
        turnsLeft = 2;
    }
    private IEnumerator Revenge_Promotion()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} received a vengeful promotion!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        AddStatusAilment(StatusAilment.Revenge, 3, true);
        WriteMessage($"You'll receive 1/2 the damage you deal to the {enemyName}.", true);
        turnsLeft = 2;
    }
    private IEnumerator Lovely_Promotion()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} received a lovely promotion!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        AddStatusAilment(StatusAilment.HP_Regen, 3, true);
        WriteMessage($"The {enemyName}'s HP will slowly recover for a brief period!", true);
        turnsLeft = 2;
    }
    private IEnumerator ThrowStone()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} throws a colorful stone!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        DealDamage(ATK, 0);
        turnsLeft = 1;
    }
    private IEnumerator SpillStones()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} spills all the stones!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        DealDamage(ATK + 4, 0);
        yield return new WaitForSeconds(1f);
        WriteMessage($"The {enemyName} needs time to reform itself!", true);
        turnsLeft = 3;
    }
    private IEnumerator Claw()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} swipes its sharp claws!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        DealDamage(ATK, 50);
        turnsLeft = 2;
    }
    private IEnumerator HP_Drain()
    {
        GameManager_SCR.PlaySound(17);
        int damage = ATK * 2;
        WriteMessage($"The {enemyName} attempts to drain your HP!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        DealDamage(damage, 0);
        int HP_Recovered = BM.GetComponent<BattleManager_SCR>().GetHPDrained(damage);
        StartCoroutine(AddHPToEnemy(HP_Recovered));
        yield return new WaitForSeconds(1f);
        WriteMessage($"The {enemyName} regained some of its HP!", true);
        turnsLeft = 2;
    }
    private IEnumerator Rollover()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} rolls over you!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        DealDamage(ATK, 0);
        if (phaseNum < 2) { turnsLeft = 3; }
        else { turnsLeft = 2; }
    }
    private IEnumerator Immovable()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} cannot move!", true);
        yield return new WaitForSeconds(2f);
        turnsLeft = 3;
    }
    private IEnumerator Squirt(bool paralyzePlayer)
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} squirts lemon juice on you!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        Vector3 BoardPos = BM.GetComponent<BattleManager_SCR>().GetPlayerBoard().transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/LemonJuice_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 16f, -101f));
        yield return new WaitForSeconds(1.9f);

        if (paralyzePlayer)
        {
            BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Paralyzed, 3);
            WriteMessage($"You are paralyzed! Some of your tiles are unselectable!", true);
        }
        else { WriteMessage($"The lemon juice had no effect on you!", true); }
        turnsLeft = 2;
    }
    private IEnumerator Throw()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} throws its orb at you!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        DealDamage(ATK, 0);
        turnsLeft = UnityEngine.Random.Range(1, 3);
    }
    private IEnumerator Freeze(bool freezePlayer)
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} blows a chilly breeze!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        Vector3 BoardPos = BM.GetComponent<BattleManager_SCR>().GetPlayerBoard().transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/Freeze_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 16f, -101f));
        yield return new WaitForSeconds(1.9f);

        if (freezePlayer)
        {
            BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Frozen, 3);
            WriteMessage($"You are frozen! You cannot attack or use Commands!", true);
        }
        else { WriteMessage($"The breeze had no effect on you!", true); }
        turnsLeft = 3;
    }
    private IEnumerator StrongRefresh()
    {
        GameManager_SCR.PlaySound(17);
        WriteMessage($"The {enemyName} gives you a refreshing drink!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        BM.GetComponent<BattleManager_SCR>().AddHPToPlayer(10);
        yield return new WaitForSeconds(1.5f);
        WriteMessage($"The refreshing drink made you woozy!", true);
        yield return new WaitForSeconds(1f);
        BM.GetComponent<BattleManager_SCR>().GetPlayer().AddStatusAilment(BattleManager_SCR.MainPlayer.StatusAilment.Confused, 4);
        turnsLeft = 3;
    }
    private IEnumerator Kiss()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} blows a kiss!", true);
        yield return new WaitForSeconds(1f);
        Vector3 BoardPos = BM.GetComponent<BattleManager_SCR>().GetPlayerBoard().transform.position;
        OtherFunctions.CreateObjectFromResource("Prefabs/Kiss_PFX", new Vector3(BoardPos.x + 436f, BoardPos.y - 515f, -101f));
        ClearMessage();
        yield return new WaitForSeconds(0.7f);
        DealDamage(ATK, 0);
        turnsLeft = 2;
    }
    private IEnumerator RapidSting()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} rapidly stings you!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        for (int tries = 0; tries < 3; tries++)
        {
            DealDamage(1, 0);
            yield return new WaitForSeconds(0.2f);
        }
        turnsLeft = 2;
    }
    private IEnumerator Sting()
    {
        GameManager_SCR.PlaySound(16);
        WriteMessage($"The {enemyName} flies by and stings you!", true);
        yield return new WaitForSeconds(1f);
        ClearMessage();
        DealDamage(ATK, 50);
        turnsLeft = UnityEngine.Random.Range(1, 3);
    }
    private void CreateCards()
    {
        Cards.AddRange(new Card[] { Card.Zero, Card.Ace, Card.Two, Card.Three, Card.Four, Card.Five, Card.Six,
                                    Card.Zero, Card.Ace, Card.Two, Card.Three, Card.Four, Card.Five, Card.Six});
        for (int i = Cards.Count - 1; i > 0; i--)
        {
            // might have a range error based random number
            int j = Mathf.FloorToInt((UnityEngine.Random.value * (i + 1)) % Cards.Count);
            Card temp = Cards[i];
            Cards[i] = Cards[j];
            Cards[j] = temp;
        }
    }
    private void RemoveCard() { Cards.RemoveAt(0); }
    private bool ThresholdReached(int damage)
    {
        return (hitCounter >= hitThreshold[phaseNum] || damage >= damageThreshold[phaseNum]);
    }
    private bool Broken() { return phaseNum == 4; }
    private void ResetHitCounter() { hitCounter = 0; }
    private void CheckDefenseCondition(int damage)
    {
        hitCounter++;
        if (ThresholdReached(damage))
        {
            ResetHitCounter();
            phaseNum++;
            if (phaseNum != 4)
            {
                switch (phaseNum)
                {
                    case 1:
                        {
                            WriteMessage($"You chipped a small piece of the {enemyName}!", true);
                            DEF = 4 + DEF_Levels[DEF_LevelNum];
                            break; }
                    case 2:
                        {
                            GameManager_SCR.PlaySound(34);
                            WriteMessage($"The {enemyName} is becoming more vulnerable!", true);
                            DEF = 2 + DEF_Levels[DEF_LevelNum];
                            OtherFunctions.CreateObjectFromResource("Prefabs/Cloud1_PFX", Obj.transform.position);
                            break;
                        }
                    case 3:
                        {
                            GameManager_SCR.PlaySound(35);
                            WriteMessage($"The {enemyName} is struggling to defend itself!", true);
                            DEF = 1 + DEF_Levels[DEF_LevelNum];
                            OtherFunctions.CreateObjectFromResource("Prefabs/Cloud2_PFX", Obj.transform.position);
                            break;
                        }
                }
                OtherFunctions.ChangeSprite(Obj, $"Sprites/Enemies/Eighter_Phase{phaseNum + 1}");
            }
            else
            {
                GameManager_SCR.PlaySound(36);
                GameManager_SCR.PlaySound(39);
                DEF = 0 + DEF_Levels[DEF_LevelNum];
                OtherFunctions.ChangeSprite(Obj, "Sprites/Enemies/Eighter_Broken");
                OtherFunctions.CreateObjectFromResource("Prefabs/Cloud3_PFX", Obj.transform.position);
                WriteMessage($"You've broken the {enemyName}! It's incapable of attacking!", true);
            }
        }
    }
}
