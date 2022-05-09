using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using EaseFunctions;
using TMPro;
using System.IO;

public class GameManager_SCR : MonoBehaviour
{
    public enum ScreenType
    {
        Windowed = 0,
        Fullscreen
    };
    private string[] DescriptionArray = new string[5]
    {
        "Complete a series of enemy waves in turn-based fashion.",
        //"Take a look at the enemies you've fought so far.",
        "Not available in prototype version.",
        "Not available in prototype version.",
        //"See the talented staff involved in the making of VersaTiles.",
        "Quit the game.",
        "Press any button to join game. Two players are required to continue."
    };

    public enum TimelineScript
    {
        None = 0,
        SplashScreenAnimation,
        TitleScreenAnimation,
        TitleToMenu,
        MenuToTutorial,
        MenuToArcade,
        GoToBattleScreen,
        BattleBegin,
        MenuToVersus,
        BackToMainMenu,
        Popup,
        DemoBattleToArcade,
        DemoBattleToMainMenu,
        ShowTutorial
    }

    public enum MenuOption
    {
        None = 0,
        Arcade,
        Tutorial,
        Credits,
        Versus,
        Quit
    }

    public enum Theme
    {
        Jungle = 0,
        Casino,
        Internet
    }

    //Timeline Variables
    private int SS_Animation_index = 0;
    [SerializeField] private float timeVal = 0f;
    private readonly float timeValMax = 60f;
    [SerializeField] private bool timeline_running = false;
    [SerializeField] private TimelineScript TLS = TimelineScript.None;
    private bool isCoroutineRunning = false;

    //Splash Screen GameObjects
    private GameObject[] DD_Letter = new GameObject[2];
    private GameObject DD_Title;

    //Title Screen GameObjects
    private GameObject CM;
    private GameObject[] TitleTiles = new GameObject[10];
    private GameObject GridBkg;
    private GameObject LOCBkg;
    private GameObject GradientBkg;
    private GameObject AnyButton_Text;
    private GameObject Bits_PS;
    private GameObject Square_PS;
    private GameObject SlideBar;
    private GameObject CopyrightInfo;

    //Main Menu GameObjects
    private GameObject[] MenuButton = new GameObject[2];
    private GameObject Selection;
    private GameObject[] SelectTick = new GameObject[4];
    private GameObject SelectArrow;
    private GameObject[] Menubar = new GameObject[2];
    private TMP_Text DescText;
    private GameObject DarkOverlay;
    private GameObject TopOverlay;
    private GameObject Popup;
    private GameObject[] PlayerWaitTab = new GameObject[2];
    private GameObject ReadyBar;
    private GameObject[] techBorder = new GameObject[2];
    private GameObject ReadyToPlayText;
    private AudioSource AudioSrc_BGM;
    private static AudioSource[] SoundFX;
    private AudioClip BGM;

    //Menu variables
    private bool canMenuSelect = false;
    private bool canPopupSelect = false;
    private bool canInstrSelect = false;
    private GameObject[] InstrPanels = new GameObject[14];
    private GameObject[] panelNodes = new GameObject[14];
    private GameObject[] panelArrows = new GameObject[2];
    private int panelNum = 0;
    private Vector3 currentSelectPosition = new Vector3(0f, 0f, 0f);
    private MenuOption MB = MenuOption.Arcade;
    private MenuOption previousMB = MenuOption.Arcade;
    private IEnumerator[] easingMB = new IEnumerator[4];
    private const float MB_StartPosition = 420f;
    private float selectCooldownTime = 0.2f;
    private bool tutorialRequested = false;
    private bool penaltyRequested = false;
    private bool tutorialChoice = false;
    private bool penaltyChoice = false;
    private bool penaltyTimerON = true;
    private Player systemPlayer;
    private bool allPlayersReady = false;
    private ScreenType ST = ScreenType.Windowed;
    private bool allowLoadingStats = false;
    private const string newLine = "#SAVESTAT#";

    //Arcade Menu variables
    private Theme worldTheme = Theme.Jungle;
    private int levelNum = 1;
    private GameObject WorldTag;
    private GameObject LevelLine;
    private GameObject[] LevelNodes = new GameObject[2];
    private GameObject[] LevelTiles = new GameObject[3];

    private void Awake()
    {
        int SFX_Count = GameObject.FindGameObjectsWithTag("SFX").Length;
        SoundFX = new AudioSource[SFX_Count];
        for (int i = 0; i < SFX_Count; i++) { SoundFX[i] = this.gameObject.transform.GetChild(i).gameObject.GetComponent<AudioSource>(); }

        AudioSrc_BGM = this.gameObject.GetComponent<AudioSource>();
        BGM = Resources.Load<AudioClip>("Audio/BackgroundMusic/Menu_BGM");
        AudioSrc_BGM.clip = BGM;
        DD_Letter[0] = GameObject.Find("DCube1");
        DD_Letter[1] = GameObject.Find("DCube2");
        DD_Title = GameObject.Find("DigitalDiceTitle");
        CM = GameObject.Find("ReWired Input Manager");
        systemPlayer = ReInput.players.GetSystemPlayer();
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (systemPlayer.GetButtonDown("SetResolution"))
        {
            if (ST == ScreenType.Windowed)
            {
                ST = ScreenType.Fullscreen;
                Screen.SetResolution(1920, 1080, true);
            }
            else
            {
                ST = ScreenType.Windowed;
                Screen.SetResolution(1920, 1080, false);
            }
        }
        timeVal = Mathf.Clamp(timeVal, 0f, timeValMax);

        if (timeline_running)
        {
            if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
            else { StartCoroutine(ExecuteTimeline(TLS)); }
        }

        if (Bits_PS != null) { Bits_PS.transform.Rotate(new Vector3(0f, 0f, 0.05f)); }
        for (int i = 0; i < TitleTiles.Length; i++)
        {
            if (TitleTiles[i] != null)
            {
                if (TitleTiles[i].GetComponent<TitleTile_SCR>().GetCanJump()) { break; }
                if (i == TitleTiles.Length - 1)
                    for (int j = 0; j < TitleTiles.Length; j++) { TitleTiles[j].GetComponent<TitleTile_SCR>().AnimateJump((j * 0.08f) + 5f); }
            }
        }

        if (CM.GetComponent<ControllerManager_SCR>().CheckRequiredNumOfPlayers() && !allPlayersReady)
        {
            allPlayersReady = true;

            StartCoroutine(EasingFunctions.TranslateTo(techBorder[0], new Vector3(0f, 668f), 0.25f, 3, Easing.EaseOut));
            StartCoroutine(EasingFunctions.TranslateTo(techBorder[1], new Vector3(1920f, 412f), 0.25f, 3, Easing.EaseOut));
            StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.25f, 0.5f));
        }
        else if (!CM.GetComponent<ControllerManager_SCR>().CheckRequiredNumOfPlayers() && allPlayersReady)
        {
            allPlayersReady = false;

            StartCoroutine(EasingFunctions.TranslateTo(techBorder[0], new Vector3(0f, 1260f), 0.25f, 3, Easing.EaseOut));
            StartCoroutine(EasingFunctions.TranslateTo(techBorder[1], new Vector3(1920f, -180f), 0.25f, 3, Easing.EaseOut));
            StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.25f, 0f));
        }

        MenuNavigation();
        DisplayDescription();
        LevelManagement();

    }

    public void LoadWorldInfo()
    {
        string saveString = File.ReadAllText(Application.dataPath + "/Player_Stats.txt");
        string[] contents = saveString.Split(new[] { newLine }, System.StringSplitOptions.None);

        worldTheme = (Theme)(int.Parse(contents[0]) - 1);
        levelNum = int.Parse(contents[1]);
    }

    public void StartTimeline(TimelineScript tls, float time = 1f, int animationIndex = 0)
    {
        SS_Animation_index = animationIndex;
        TLS = tls;
        timeVal = time;
        timeline_running = true;
    }
    public void EndTimeline() { timeline_running = false; }

    IEnumerator ExecuteTimeline(TimelineScript timelinescript)
    {
        if (isCoroutineRunning) { yield break; }
        isCoroutineRunning = true;

        if (timelinescript == TimelineScript.SplashScreenAnimation)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        StartCoroutine(EasingFunctions.TranslateTo(DD_Letter[0].gameObject, new Vector3(-122f, 219f), 1.5f));
                        StartCoroutine(EasingFunctions.TranslateTo(DD_Letter[1], new Vector3(65f, 30f), 1.5f));
                        timeVal = 1.5f;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DD_Title, "#ffffff", 0.75f));
                        timeVal = 2f;
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DD_Letter[0], "#ffffff", 1f, 0f));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DD_Letter[1], "#ffffff", 1f, 0f));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DD_Title, "#ffffff", 1f, 0f));
                        timeVal = 2f;
                        break;
                    }
                case 4:
                    {
                        SceneManager.LoadScene("Resources/Scenes/TitleMenu_SCN");
                        StartTimeline(TimelineScript.TitleScreenAnimation, 2f);
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.TitleScreenAnimation)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        GameObject TitleTile_V = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(-240f, 804f, -5));
                        TitleTile_V.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 0);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_V, new Vector3(423f, 804f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 2:
                    {
                        GameObject TitleTile_e1 = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(667f, -120f, -5));
                        TitleTile_e1.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 1);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_e1, new Vector3(667f, 857f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 3:
                    {
                        GameObject TitleTile_r = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(2240f, 857f, -5));
                        TitleTile_r.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 2);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_r, new Vector3(865f, 857f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 4:
                    {
                        GameObject TitleTile_s1 = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(1063f, -120f, -5));
                        TitleTile_s1.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 3);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_s1, new Vector3(1063f, 857f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 5:
                    {
                        GameObject TitleTile_a = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(2240f, 857f, -5));
                        TitleTile_a.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 4);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_a, new Vector3(1261f, 857f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 6:
                    {
                        GameObject TitleTile_T = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(2240f, 596f, -5));
                        TitleTile_T.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 5);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_T, new Vector3(716f, 596f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 7:
                    {
                        GameObject TitleTile_i = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(960f, -120f, -5));
                        TitleTile_i.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 6);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_i, new Vector3(960f, 649f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 8:
                    {
                        GameObject TitleTile_l = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(2240f, 649f, -5));
                        TitleTile_l.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 7);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_l, new Vector3(1158f, 649f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 9:
                    {
                        GameObject TitleTile_e2 = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(1356f, -120f, -5));
                        TitleTile_e2.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 8);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_e2, new Vector3(1356f, 649f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 1f;
                        break;
                    }
                case 10:
                    {
                        GameObject TitleTile_s2 = OtherFunctions.CreateObjectFromResource("Prefabs/TitleTile_PFB", new Vector3(1554f, -120f, -5));
                        TitleTile_s2.GetComponent<TitleTile_SCR>().ChangeSprite("Sprites/Title_tiles", 9);
                        StartCoroutine(EasingFunctions.TranslateTo(TitleTile_s2, new Vector3(1554f, 649f, -5), 0.3f, 3, Easing.EaseOut));
                        timeVal = 0.25f;
                        break;
                    }
                case 11:
                    {
                        GameObject TrailBit_Emitter = GameObject.Find("TrailBit Emitter");
                        Square_PS.GetComponent<ParticleSystem>().Play();
                        TrailBit_Emitter.GetComponent<LightTrail_Emitter_SCR>().EnableEmitter();

                        TitleTiles = GameObject.FindGameObjectsWithTag("Title Tile");
                        for (int i = 0; i < TitleTiles.Length; i++) { TitleTiles[i].GetComponent<TitleTile_SCR>().AnimateJump((i * 0.08f) + 5f); }

                        GameObject.Find("Flash Effect").GetComponent<Flash_SCR>().Flash();
                        GridBkg.GetComponent<SpriteRenderer>().color = new Color(0f, 208f / 255f, 1f, 0.25f);
                        LOCBkg.GetComponent<SpriteRenderer>().color = new Color(0f, 135f / 255f, 1f, 0.5f);
                        GradientBkg.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                        CopyrightInfo.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                        Bits_PS.GetComponent<ParticleSystem>().Play();

                        ChangeBGM("Audio/BackgroundMusic/Menu_BGM");
                        StartCoroutine(AudioFade_SCR.Fade(AudioSrc_BGM, 0.85f, 1f));

                        timeVal = 2f;
                        break;
                    }
                case 12:
                    {
                        AnyButton_Text.GetComponent<AnyButtonText_SCR>().StartBlinking();
                        StartCoroutine(EasingFunctions.TranslateTo(SlideBar, new Vector2(960f, SlideBar.transform.position.y), 1f, 3, Easing.EaseOut));
                        EndTimeline();
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.TitleToMenu)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        tutorialRequested = false;
                        penaltyRequested = false;
                        tutorialChoice = false;
                        penaltyChoice = false;
                        bool Discontinue = false;
                        for (int i = 0; i < TitleTiles.Length; i++)
                        {
                            if (TitleTiles[i].GetComponent<TitleTile_SCR>().GetIsJumping()) { Discontinue = true; break; }
                        }
                        if (Discontinue) { SS_Animation_index--; break; }
                        for (int i = 0; i < TitleTiles.Length; i++)
                        {
                            Vector3 tilePosition = TitleTiles[i].transform.position;
                            TitleTiles[i].GetComponent<TitleTile_SCR>().DisableJump();
                            StartCoroutine(EasingFunctions.TranslateTo(TitleTiles[i], new Vector3(2400f + (270f * i), tilePosition.y, tilePosition.z), 0.1f));
                        }
                        Destroy(AnyButton_Text);
                        StartCoroutine(EasingFunctions.TranslateTo(CopyrightInfo, new Vector2(2240f, CopyrightInfo.transform.position.y), 0.1f));
                        StartCoroutine(EasingFunctions.TranslateTo(SlideBar, new Vector2(-960f, SlideBar.transform.position.y), 0.1f));
                        timeVal = 0.5f;
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < TitleTiles.Length; i++) { Destroy(TitleTiles[i]); }
                        Destroy(CopyrightInfo);
                        Destroy(SlideBar);
                        timeVal = 1.5f;
                        break;
                    }
                case 3:
                    {
                        for (int i = 0; i < Menubar.Length; i++)
                        {
                            Menubar[i] = GameObject.Find($"Menu Bar{i + 1}");
                            StartCoroutine(EasingFunctions.TranslateTo(Menubar[i], new Vector2(960f, Menubar[i].transform.position.y), 1f));
                        }
                        for (int i = 0; i < MenuButton.Length; i++)
                        {
                            int index = (i == 0 ? 0 : 3);
                            MenuButton[i] = OtherFunctions.CreateObjectFromResource("Prefabs/MenuButton_PFB", new Vector3(-320f, 832f - (i * 144f), 0f));
                            MenuButton[i].GetComponent<MenuButton_SCR>().ChangeSprite("Sprites/MenuButtons", index);
                            StartCoroutine(EasingFunctions.TranslateTo(MenuButton[i], new Vector2(MB_StartPosition, MenuButton[i].transform.position.y), 0.5f, 3, Easing.EaseOut));
                            yield return new WaitForSeconds(0.1f);
                        }
                        timeVal = 1f;
                        break;
                    }
                case 4:
                    {
                        EnableMenuSelection(MenuOption.Arcade);
                        EndTimeline();
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.MenuToTutorial)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        SceneManager.LoadScene("Resources/Scenes/Tutorial_SCN");
                        timeVal = 1f;
                        break;
                    }
                case 2:
                    {
                        tutorialRequested = true;
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 0f));
                        Popup = OtherFunctions.CreateObjectFromResource("Prefabs/PopupMsg_PFB", new Vector3(960f, -540f, -100f));
                        OtherFunctions.ChangeSprite(Popup, "Sprites/PopupMessage", 1);
                        Popup.GetComponent<Popup_SCR>().SetPopupType(Popup_SCR.PopupType.Question);

                        StartCoroutine(EasingFunctions.TranslateTo(Popup, new Vector2(960f, 540f), 0.5f, 3, Easing.EaseOut));

                        DisableMenuSelection();
                        StartTimeline(TimelineScript.Popup, 1f);
                        break;
                    }
                case 3:
                    {
                        for (int i = 0; i < panelArrows.Length; i++)
                        {
                            if (panelArrows[i] != null) { Destroy(panelArrows[i]); }
                        }
                        tutorialRequested = false;
                        penaltyRequested = true;
                        Popup = OtherFunctions.CreateObjectFromResource("Prefabs/PopupMsg_PFB", new Vector3(960f, -540f, -100f));
                        OtherFunctions.ChangeSprite(Popup, "Sprites/PopupMessage", 2);
                        Popup.GetComponent<Popup_SCR>().SetPopupType(Popup_SCR.PopupType.Question);

                        StartCoroutine(EasingFunctions.TranslateTo(Popup, new Vector2(960f, 540f), 0.5f, 3, Easing.EaseOut));

                        DisableMenuSelection();
                        StartTimeline(TimelineScript.Popup, 1f);
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.MenuToArcade)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        try
                        {
                            LoadWorldInfo();
                        }
                        catch (Exception e) { Debug.LogError(e.Message); }
                        SceneManager.LoadScene("Resources/Scenes/ArcadeMenu_SCN");
                        timeVal = 1f;
                        break;
                    }
                case 2:
                    {
                        if (worldTheme == Theme.Casino)
                        {
                            LevelTiles[0].transform.position = new Vector3(720f, 540f, -49f);
                            LevelTiles[1].transform.position = new Vector3(1200f, 540f, -49f);
                            LevelLine.transform.localScale = new Vector3(480f, 16f, 1f);
                            Destroy(LevelTiles[2]);
                        }
                        if (GameObject.Find("TrailBit Emitter")) { Destroy(GameObject.Find("TrailBit Emitter")); }
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 0f));
                        CreateSelection(LevelTiles[levelNum - 1], 208f, 226f, new Color(1f, 1f, 1f, 1f), 1.5f);
                        StartTimeline(TimelineScript.GoToBattleScreen, 3f);
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.GoToBattleScreen)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        StartCoroutine(AudioFade_SCR.Fade(AudioSrc_BGM, 0f, 2f));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 1f));
                        timeVal = 2f;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 1f));
                        CreateSelection(LevelTiles[0], 208f, 226f, new Color(1f, 1f, 1f, 1f), 1.5f);
                        timeVal = 1f;
                        break;
                    }
                case 3:
                    {
                        if (worldTheme == Theme.Jungle)
                        {
                            switch (levelNum)
                            {
                                case 1: { ChangeBGM("Audio/BackgroundMusic/Jungle1_BGM"); break; }
                                case 2: { ChangeBGM("Audio/BackgroundMusic/Jungle2_BGM"); break; }
                                case 3: { ChangeBGM("Audio/BackgroundMusic/Jungle3_BGM"); break; }
                            }
                        }
                        else if (worldTheme == Theme.Casino)
                        {
                            switch (levelNum)
                            {
                                case 1: { ChangeBGM("Audio/BackgroundMusic/Casino1_BGM"); break; }
                                case 2: { ChangeBGM("Audio/BackgroundMusic/Casino2_BGM"); break; }
                            }
                        }
                        else if (worldTheme == Theme.Internet) { ChangeBGM("Audio/BackgroundMusic/Internet1_BGM"); }

                        StartCoroutine(AudioFade_SCR.Fade(AudioSrc_BGM, 0.5f, 1f));

                        Destroy(Bits_PS);
                        Destroy(Square_PS);
                        Destroy(GridBkg);
                        Destroy(LOCBkg);
                        Destroy(GradientBkg);
                        SceneManager.LoadScene("Resources/Scenes/BattleScreen_SCN");
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 0f));
                        StartTimeline(TimelineScript.BattleBegin, 2f);
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.BattleBegin)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(ReadyBar, "#000000", 0.25f, 1f));
                        GameManager_SCR.PlaySound(42);
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[0], new Vector3(0f, 668f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[1], new Vector3(1920f, 412f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(ReadyToPlayText, new Vector3(960f, 540f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.25f, 0.5f));
                        timeVal = 1f;
                        break;
                    }
                case 2:
                    {
                        GameManager_SCR.PlaySound(42);
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(ReadyBar, "#000000", 0.25f, 0f));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[0], new Vector3(0f, 1260f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(techBorder[1], new Vector3(1920f, -180f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.TranslateTo(ReadyToPlayText, new Vector3(2700f, 540f), 0.25f, 3, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.25f, 0f));
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.DemoBattleToArcade)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        StartCoroutine(AudioFade_SCR.Fade(AudioSrc_BGM, 0f, 2f));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 2f, 1f));
                        timeVal = 3f;
                        break;
                    }
                case 2:
                    {
                        StartTimeline(TimelineScript.MenuToArcade, 1f);
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.DemoBattleToMainMenu)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        StartCoroutine(AudioFade_SCR.Fade(AudioSrc_BGM, 0f, 2f));
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 2f, 1f));
                        timeVal = 3f;
                        break;
                    }
                case 2:
                    {
                        DarkOverlay.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                        SceneManager.LoadScene("Resources/Scenes/TitleMenu_SCN");
                        worldTheme = Theme.Jungle;
                        levelNum = 1;
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 0f));
                        StartTimeline(TimelineScript.TitleScreenAnimation, 1f);
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.MenuToVersus)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        for (int i = 0; i < MenuButton.Length; i++) { Destroy(MenuButton[i]); }
                        for (int i = 0; i < PlayerWaitTab.Length; i++)
                        {
                            PlayerWaitTab[i] = OtherFunctions.CreateObjectFromResource("Prefabs/PlayerWaitTab_PFB", new Vector3(-480f - (421f * i) - 36f, 540f, -2f));
                            if (i == 0) { StartCoroutine(EasingFunctions.TranslateTo(PlayerWaitTab[i], new Vector3(960f - 360f, 540f), 0.5f, 3, Easing.EaseOut)); }
                            else if (i == 1) { StartCoroutine(EasingFunctions.TranslateTo(PlayerWaitTab[i], new Vector3(960f + 360f, 540f), 0.5f, 3, Easing.EaseOut)); }
                            PlayerWaitTab[i].GetComponent<PlayerWaitTab_SCR>().AssignPlayerID(i);
                        }
                        timeVal = 1f;
                        break;
                    }
                case 2:
                    {
                        CM.GetComponent<ControllerManager_SCR>().EnableLobbyControls();
                        EndTimeline();
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.BackToMainMenu)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        for (int i = 0; i < PlayerWaitTab.Length; i++) { Destroy(PlayerWaitTab[i]); }
                        for (int i = 0; i < MenuButton.Length; i++)
                        {
                            MenuButton[i] = OtherFunctions.CreateObjectFromResource("Prefabs/MenuButton_PFB", new Vector3(-320f, 832f - (i * 144f), 0f));
                            MenuButton[i].GetComponent<MenuButton_SCR>().ChangeSprite("Sprites/MenuButtons", i);
                            StartCoroutine(EasingFunctions.TranslateTo(MenuButton[i], new Vector2(MB_StartPosition, MenuButton[i].transform.position.y), 0.25f, 3, Easing.EaseOut));
                        }
                        timeVal = 0.5f;
                        break;
                    }
                case 2:
                    {
                        EnableMenuSelection(MenuOption.Arcade);
                        EndTimeline();
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.Popup)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        Popup.GetComponent<Popup_SCR>().EnableSelect();
                        EnablePopupSelection();
                        EndTimeline();
                        break;
                    }
                case 2:
                    {
                        Destroy(Popup);
                        if (tutorialRequested)
                        {
                            if (tutorialChoice)
                            {
                                StartTimeline(TimelineScript.ShowTutorial, 0.1f);
                            }
                            else
                            {
                                StartTimeline(TimelineScript.MenuToTutorial, 0.1f, 2);
                            }
                        }
                        else if (penaltyRequested)
                        {
                            if (penaltyChoice)
                            {
                                penaltyTimerON = true;
                            }
                            else
                            {
                                penaltyTimerON = false;
                            }
                            StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 1f));
                            StartTimeline(TimelineScript.MenuToArcade, 2f);
                        }
                        else
                        {
                            EnableMenuSelection(MenuOption.Quit);
                            EndTimeline();
                        }
                        break;
                    }
            }
        }
        else if (timelinescript == TimelineScript.ShowTutorial)
        {
            SS_Animation_index++;
            switch (SS_Animation_index)
            {
                case 1:
                    {
                        GameObject InstrPanelGroup = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(3120f, 540f, -105f));
                        for (int i = 0; i < InstrPanels.Length; i++)
                        {
                            InstrPanels[i] = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(3120f, 540f, -105f));
                            OtherFunctions.ChangeSprite(InstrPanels[i], "Sprites/InstructionPanels", i);
                            InstrPanels[i].transform.SetParent(InstrPanelGroup.transform);
                        }
                        StartCoroutine(EasingFunctions.TranslateTo(InstrPanels[panelNum], new Vector3(960f, 540f, -105f), 0.5f, 3, Easing.EaseOut));
                        timeVal = 1f;
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < panelArrows.Length; i++)
                        {
                            panelArrows[i] = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", new Vector3(320f + ((1280f) * i), 540f, -105f));
                            panelArrows[i].transform.localScale = new Vector3(2f, 2f, 1f);
                            OtherFunctions.ChangeSprite(panelArrows[i], "Sprites/SelectArrow");
                            panelArrows[i].GetComponent<SpriteRenderer>().color = new Color(0f, 138f / 255f, 1f, 1f);
                            if (i == 0) { panelArrows[i].transform.rotation = Quaternion.Euler(0f, 180f, 0f); }
                        }
                        canInstrSelect = true;
                        EndTimeline();
                        break;
                    }
            }
        }

        isCoroutineRunning = false;
    }

    public void EnableMenuSelection(MenuOption menu_option)
    {
        MB = menu_option;
        canMenuSelect = true;
        if (SelectArrow == null) { SelectArrow = OtherFunctions.CreateObjectFromResource("Prefabs/SelectArrow_PFB", new Vector3(96f, -320f, -1f)); }

        switch (MB)
        {
            case MenuOption.Arcade:
                {
                    float MB_YPosition = MenuButton[0].transform.position.y;
                    CreateSelection(MenuButton[0], 522f, 104f, new Color(0f, 228f / 255f, 1f, 1f));
            
                    ArrangeMenuAnimation(0);
                    SelectArrow.transform.position = new Vector2(SelectArrow.transform.position.x, MB_YPosition);
                    break;
                }
            case MenuOption.Tutorial:
                {
                    float MB_YPosition = MenuButton[1].transform.position.y;

                    ArrangeMenuAnimation(1);
                    SelectArrow.transform.position = new Vector2(SelectArrow.transform.position.x, MB_YPosition);
                    break;
                }
            case MenuOption.Credits:
                {
                    float MB_YPosition = MenuButton[2].transform.position.y;
                    ArrangeMenuAnimation(2);
                    SelectArrow.transform.position = new Vector2(SelectArrow.transform.position.x, MB_YPosition);
                    break;
                }
            case MenuOption.Quit:
                {
                    float MB_YPosition = MenuButton[1].transform.position.y;
                    ArrangeMenuAnimation(1);
                    SelectArrow.transform.position = new Vector2(SelectArrow.transform.position.x, MB_YPosition);
                    break;
                }
        }
    }

    private void MenuNavigation()
    {
        if (!CanMoveSelection()) { selectCooldownTime -= 1f * Time.deltaTime; }
        selectCooldownTime = Mathf.Clamp(selectCooldownTime, 0f, 10f);

        if (canPopupSelect)
        {
            if (Popup.GetComponent<Popup_SCR>().GetPopupType() == Popup_SCR.PopupType.Question)
            {
                if (systemPlayer.GetButtonDown("Confirm"))
                {
                    PlaySound(1);
                    Popup.GetComponent<Popup_SCR>().ConfirmChoice();
                    selectCooldownTime = 0.2f;
                    DisablePopupSelection();
                    StartCoroutine(EasingFunctions.TranslateTo(Popup, new Vector2(960f, -540f), 0.5f, 3, Easing.EaseOut));
                    StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.5f, 0f));
                    StartTimeline(TimelineScript.Popup, 1f, 1);
                }
                else if ((systemPlayer.GetButton("SelectRight") || systemPlayer.GetButton("SelectLeft")) && CanMoveSelection())
                {
                    PlaySound(2);
                    selectCooldownTime = 0.2f;
                    Popup.GetComponent<Popup_SCR>().ToggleChoice();
                }
            }
        }
        if (canInstrSelect)
        {
            if (systemPlayer.GetButton("SelectRight") && CanMoveSelection())
            {
                PlaySound(2);
                if (panelNum < InstrPanels.Length - 1)
                {
                    selectCooldownTime = 0.55f;
                    StartCoroutine(EasingFunctions.TranslateTo(InstrPanels[panelNum++], new Vector3(-1200f, 540f, -105f), 0.5f, 3, Easing.EaseOut));
                    StartCoroutine(EasingFunctions.TranslateTo(InstrPanels[panelNum], new Vector3(960f, 540f, -105f), 0.5f, 3, Easing.EaseOut));
                }
                else
                {
                    canInstrSelect = false;
                    Destroy(panelArrows[1]);
                    StartCoroutine(EasingFunctions.TranslateTo(InstrPanels[panelNum], new Vector3(-1200f, 540f, -105f), 0.5f, 3, Easing.EaseOut));
                    StartTimeline(TimelineScript.MenuToTutorial, 0.5f, 2);
                }
            }
            else if (systemPlayer.GetButton("SelectLeft") && CanMoveSelection() && panelNum > 0)
            {
                PlaySound(2);
                selectCooldownTime = 0.55f;
                StartCoroutine(EasingFunctions.TranslateTo(InstrPanels[panelNum--], new Vector3(3120f, 540f, -105f), 0.5f, 3, Easing.EaseOut));
                StartCoroutine(EasingFunctions.TranslateTo(InstrPanels[panelNum], new Vector3(960f, 540f, -105f), 0.5f, 3, Easing.EaseOut));
            }
        }
        else if (canMenuSelect)
        {
            if (systemPlayer.GetButtonDown("Confirm"))
            {
                PlaySound(1);
                selectCooldownTime = 0.2f;

                switch (MB)
                {
                    case MenuOption.Arcade:
                        {
                            for (int i = 0; i < MenuButton.Length; i++)
                            {
                                if (easingMB[i] != null) { StopCoroutine(easingMB[i]); }
                                easingMB[i] = EasingFunctions.TranslateTo(MenuButton[i], new Vector2(-560f, MenuButton[i].transform.position.y), 0.5f, 3, Easing.EaseOut);
                                StartCoroutine(easingMB[i]);
                            }

                            Destroy(SelectArrow);
                            DisableMenuSelection();
                            StartTimeline(TimelineScript.MenuToTutorial, 2f);
                            StartCoroutine(EasingFunctions.ColorChangeFromHex(TopOverlay, "#000000", 0.5f, 1f));
                            break;
                        }
                    case MenuOption.Quit:
                        {
                            Popup = OtherFunctions.CreateObjectFromResource("Prefabs/PopupMsg_PFB", new Vector3(960f, -540f, -100f));
                            OtherFunctions.ChangeSprite(Popup, "Sprites/PopupMessage", 0);
                            Popup.GetComponent<Popup_SCR>().SetPopupType(Popup_SCR.PopupType.Question);

                            StartCoroutine(EasingFunctions.TranslateTo(Popup, new Vector2(960f, 540f), 0.5f, 3, Easing.EaseOut));
                            StartCoroutine(EasingFunctions.ColorChangeFromHex(DarkOverlay, "#000000", 0.5f, 0.5f));

                            DisableMenuSelection();
                            StartTimeline(TimelineScript.Popup, 1f);
                            break;
                        }
                    case MenuOption.Versus:
                        {
                            for (int i = 0; i < MenuButton.Length; i++)
                            {
                                if (easingMB[i] != null) { StopCoroutine(easingMB[i]); }
                                easingMB[i] = EasingFunctions.TranslateTo(MenuButton[i], new Vector2(-560f, MenuButton[i].transform.position.y), 0.5f, 3, Easing.EaseOut);
                                StartCoroutine(easingMB[i]);
                            }

                            Destroy(SelectArrow);
                            DisableMenuSelection();
                            StartTimeline(TimelineScript.MenuToVersus, 1f);
                            break;
                        }
                }
            }
            else if (systemPlayer.GetButton("SelectUp") && CanMoveSelection())
            {
                PlaySound(2);
                selectCooldownTime = 0.2f;
                Selection.transform.parent = null;

                switch (MB)
                {
                    case MenuOption.Arcade:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[1].transform.position.y;
                            Selection.transform.position = MenuButton[1].transform.position;
                            Selection.transform.SetParent(MenuButton[1].transform);

                            MB = MenuOption.Quit;
                            ArrangeMenuAnimation(1);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                    case MenuOption.Tutorial:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[0].transform.position.y;
                            Selection.transform.position = MenuButton[0].transform.position;
                            Selection.transform.SetParent(MenuButton[0].transform);

                            MB = MenuOption.Arcade;
                            ArrangeMenuAnimation(0);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                    case MenuOption.Credits:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[1].transform.position.y;
                            Selection.transform.position = MenuButton[1].transform.position;
                            Selection.transform.SetParent(MenuButton[1].transform);

                            MB = MenuOption.Tutorial;
                            ArrangeMenuAnimation(1);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                    case MenuOption.Quit:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[0].transform.position.y;
                            Selection.transform.position = MenuButton[0].transform.position;
                            Selection.transform.SetParent(MenuButton[0].transform);

                            MB = MenuOption.Arcade;
                            ArrangeMenuAnimation(0);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                }
            }
            else if (systemPlayer.GetButton("SelectDown") && CanMoveSelection())
            {
                PlaySound(2);
                selectCooldownTime = 0.2f;
                Selection.transform.parent = null;

                switch (MB)
                {
                    case MenuOption.Arcade:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[1].transform.position.y;
                            Selection.transform.position = MenuButton[1].transform.position;
                            Selection.transform.SetParent(MenuButton[1].transform);

                            MB = MenuOption.Quit;
                            ArrangeMenuAnimation(1);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                    case MenuOption.Tutorial:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[2].transform.position.y;
                            Selection.transform.position = MenuButton[2].transform.position;
                            Selection.transform.SetParent(MenuButton[2].transform);

                            MB = MenuOption.Credits;
                            ArrangeMenuAnimation(2);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                    case MenuOption.Credits:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[3].transform.position.y;
                            Selection.transform.position = MenuButton[3].transform.position;
                            Selection.transform.SetParent(MenuButton[3].transform);

                            MB = MenuOption.Quit;
                            ArrangeMenuAnimation(3);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                    case MenuOption.Quit:
                        {
                            Vector2 SA_Position = SelectArrow.transform.position;
                            float MB_YPosition = MenuButton[0].transform.position.y;
                            Selection.transform.position = MenuButton[0].transform.position;
                            Selection.transform.SetParent(MenuButton[0].transform);

                            MB = MenuOption.Arcade;
                            ArrangeMenuAnimation(0);
                            SelectArrow.transform.position = new Vector2(SA_Position.x, MB_YPosition);
                            //StartCoroutine(EasingFunctions.TranslateTo(SelectArrow, new Vector2(SA_Position.x, MB_YPosition), 0.5f, 3, Easing.EaseOut));
                            break;
                        }
                }
            }
        }
    }
    public bool isLoadActivated() { return allowLoadingStats; }
    public void ActivateLoad() { allowLoadingStats = true; }
    public void DeactivateLoad() { allowLoadingStats = false; }

    public void ExitVersusLobby()
    {
        for (int i = 0; i < PlayerWaitTab.Length; i++)
        {
            StartCoroutine(EasingFunctions.TranslateTo(PlayerWaitTab[i], new Vector3(-480f - (421f * i) - 36f, 540f, -2f), 0.5f, 3, Easing.EaseOut));
            PlayerWaitTab[i].GetComponent<PlayerWaitTab_SCR>().Deactivate();
        }
        CM.GetComponent<ControllerManager_SCR>().DisableLobbyControls();
        DisableMenuSelection();
        StartTimeline(TimelineScript.BackToMainMenu, 0.75f);
    }

    public void ArrangeMenuAnimation(int ignoreIndex)
    {
        for (int i = 0; i < MenuButton.Length; i++)
        {
            float MB_YPosition = MenuButton[i].transform.position.y;

            if (easingMB[i] != null) { StopCoroutine(easingMB[i]); }
            if (i == ignoreIndex) { easingMB[i] = EasingFunctions.TranslateTo(MenuButton[i], new Vector2(MB_StartPosition + 96f, MB_YPosition), 0.5f, 3, Easing.EaseOut); }
            else { easingMB[i] = EasingFunctions.TranslateTo(MenuButton[i], new Vector2(MB_StartPosition, MB_YPosition), 0.5f, 3, Easing.EaseOut); }
            StartCoroutine(easingMB[i]);
        }
    }

    public void CreateSelection(GameObject obj, float width, float height, Color col, float scale = 1f, float padding = 0f)
    {
        Vector3 pos = obj.transform.position;
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

        Selection.transform.SetParent(obj.transform);
    }

    private void DisplayDescription()
    {
        if (DescText == null) { return; }
        if (CM.GetComponent<ControllerManager_SCR>().LobbyControlsON()) { DescText.text = DescriptionArray[4]; return; }
        if (!canMenuSelect) { DescText.text = "";  return; }

        switch (MB)
        {
            case MenuOption.Arcade: { DescText.text = DescriptionArray[0]; break; }
            case MenuOption.Tutorial: { DescText.text = DescriptionArray[1]; break; }
            case MenuOption.Credits: { DescText.text = DescriptionArray[2]; break; }
            case MenuOption.Quit: { DescText.text = DescriptionArray[3]; break; }
        }
    }

    private void LevelManagement()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int worldNum = (int)worldTheme;
        Color themeColor = new Color();
        Color nullColor = new Color(1f, 1f, 1f);

        if (sceneName == "ArcadeMenu_SCN")
        {
            if (worldTheme == Theme.Jungle)
            {
                themeColor = new Color(0f, 239f / 255f, 62f / 255f);
                LevelNodes[0].GetComponent<SpriteRenderer>().color = themeColor;
                LevelNodes[1].GetComponent<SpriteRenderer>().color = nullColor;
                LevelNodes[0].transform.localScale = new Vector3(32f, 32f, 1f);
                LevelNodes[1].transform.localScale = new Vector3(16f, 16f, 1f);
            }
            else if (worldTheme == Theme.Casino)
            {
                themeColor = new Color(187f / 255f, 0f, 40f / 255f);
                LevelNodes[0].GetComponent<SpriteRenderer>().color = nullColor;
                LevelNodes[1].GetComponent<SpriteRenderer>().color = themeColor;
                LevelNodes[0].transform.localScale = new Vector3(16f, 16f, 1f);
                LevelNodes[1].transform.localScale = new Vector3(32f, 32f, 1f);
            }
            for (int i = 0; i < LevelTiles.Length; i++)
            {
                if (LevelTiles[i] != null) { OtherFunctions.ChangeSprite(LevelTiles[i], "Sprites/LevelNumberTiles", (worldNum * 3) + i); }
            }

            OtherFunctions.ChangeSprite(WorldTag, "Sprites/WorldTags", worldNum);
            LevelLine.GetComponent<SpriteRenderer>().color = themeColor;
        }
    }

    private bool CanMoveSelection() { return (selectCooldownTime == 0f); }

    public void DisableMenuSelection() { canMenuSelect = false; }
    public void EnablePopupSelection() { canPopupSelect = true; }
    public void DisablePopupSelection() { canPopupSelect = false; }
    public bool IsInTutorialRoom() { return tutorialRequested; }
    public bool IsInPenaltyRoom() { return penaltyRequested; }
    public void SetTutorialChoice(bool choice) { tutorialChoice = choice; }
    public void SetPenaltyChoice(bool choice) { penaltyChoice = choice; }
    public bool PenaltyTimerEnabled() { return penaltyTimerON; }
    public int GetWorldNumber() { return (int)worldTheme + 1; }
    public int GetLevelNumber() { return levelNum; }
    public static void PlaySound(int listenerNum) { SoundFX[listenerNum-1].Play(); }
    public static void StopSound(int listenerNum) { SoundFX[listenerNum - 1].Stop(); }
    public AudioSource GetAudioSrc() { return AudioSrc_BGM; }
    public void ChangeBGM(string Src, bool loopMusic = true)
    {
        BGM = Resources.Load<AudioClip>(Src);
        AudioSrc_BGM.clip = BGM;
        AudioSrc_BGM.Play();
        AudioSrc_BGM.loop = loopMusic;
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
            case "TitleMenu_SCN":
                {
                    for (int i = 0; i < techBorder.Length; i++) { techBorder[i] = GameObject.Find($"TechBorder{i + 1}"); }
                    if (GameObject.Find("Grid Background")) { GridBkg = GameObject.Find("Grid Background"); }
                    if (GameObject.Find("LOC Background")) { LOCBkg = GameObject.Find("LOC Background"); }
                    if (GameObject.Find("DarkBlueGradient")) { GradientBkg = GameObject.Find("DarkBlueGradient"); }
                    if (GameObject.Find("Square PS")) { Square_PS = GameObject.Find("Square PS"); }
                    if (GameObject.Find("AnyButtonText_PFB")) { AnyButton_Text = GameObject.Find("AnyButtonText_PFB"); }
                    if (GameObject.Find("Bits PS")) { Bits_PS = GameObject.Find("Bits PS"); }
                    if (GameObject.Find("Slide Bar")) { SlideBar = GameObject.Find("Slide Bar"); }
                    if (GameObject.Find("CopyrightInfo")) { CopyrightInfo = GameObject.Find("CopyrightInfo"); }
                    if (GameObject.Find("Dark Overlay")) { DarkOverlay = GameObject.Find("Dark Overlay"); DontDestroyOnLoad(DarkOverlay); }
                    if (GameObject.Find("Top Overlay")) { TopOverlay = GameObject.Find("Top Overlay"); DontDestroyOnLoad(TopOverlay); }
                    if (GameObject.Find("Desc Canvas")) { DescText = GameObject.Find("Desc Canvas").transform.GetChild(0).gameObject.GetComponent<TMP_Text>(); }
                    break;
                }
            case "ArcadeMenu_SCN":
                {
                    for (int i = 0; i < LevelTiles.Length; i++) { LevelTiles[i] = GameObject.Find($"LvlTile {i + 1}"); }
                    for (int i = 0; i < LevelNodes.Length; i++) { LevelNodes[i] = GameObject.Find($"LvlNode {i + 1}"); }
                    if (GameObject.Find("World Tag")) { WorldTag = GameObject.Find("World Tag"); }
                    if (GameObject.Find("LvlLine")) { LevelLine = GameObject.Find("LvlLine"); }
                    break;
                }
            case "BattleScreen_SCN":
                {
                    if (GameObject.Find("ReadyBar")) { ReadyBar = GameObject.Find("ReadyBar"); }
                    if (GameObject.Find("TechBorder1")) { techBorder[0] = GameObject.Find("TechBorder1"); }
                    if (GameObject.Find("TechBorder2")) { techBorder[1] = GameObject.Find("TechBorder2"); }
                    if (GameObject.Find("ReadyToPlay Text")) { ReadyToPlayText = GameObject.Find("ReadyToPlay Text"); }
                    break;
                }
        }
    }
}
