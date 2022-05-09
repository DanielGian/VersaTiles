using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Rewired;

public class PlayerWaitTab_SCR : MonoBehaviour
{
    private const string Resource = "Sprites/PlayerWaitTab";
    private GameObject Obj;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private bool isPlayerReady = false;
    private int playerID = -1;
    private Color textColor;
    private GameObject conditionText;
    private GameObject gamepadIcon;
    private string gamepadTypeString = "";
    private bool isGamepadAssigned = false;
    private bool isActive = true;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        frames = Resources.LoadAll<Sprite>(Resource);
        conditionText = OtherFunctions.CreateObjectFromResource("Prefabs/WaitConditionText_PFB", Obj.transform.position + new Vector3(0f, 0f, -1f));
        conditionText.transform.SetParent(Obj.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (playerID == 0) { isPlayerReady = ReInput.players.Players[0].controllers.joystickCount != 0; }
            else if (playerID == 1) { isPlayerReady = ReInput.players.Players[1].controllers.joystickCount != 0; }
            else { isPlayerReady = false; }

            if (isPlayerReady && !isGamepadAssigned) { AssignGamepad(); }

            CheckPlayerStatus();
        }
    }

    public void AssignPlayerID(int id) { playerID = id; }

    public void Deactivate() { isActive = false; }

    private void AssignGamepad()
    {
        const string XBOX_syntax = "^XInput Gamepad [0-9]{1}$";
        const string Sony_syntax = "^Sony DualShock [0-9]{1}$";

        string gamepadName = "";
        if (playerID == 0) { gamepadName = ReInput.players.Players[0].controllers.Joysticks[0].name; }
        else if (playerID == 1) { gamepadName = ReInput.players.Players[1].controllers.Joysticks[0].name; }

        //Create game controller icon
        gamepadIcon = OtherFunctions.CreateObjectFromResource("Prefabs/ControllerIcon_PFB", Obj.transform.position + new Vector3(0f, 256f, -2f));
        gamepadIcon.transform.SetParent(Obj.transform);

        if (Regex.IsMatch(gamepadName, XBOX_syntax)) { OtherFunctions.ChangeSprite(gamepadIcon, "Sprites/controllerIcons", 0); }
        else if (Regex.IsMatch(gamepadName, Sony_syntax)) { OtherFunctions.ChangeSprite(gamepadIcon, "Sprites/controllerIcons", 1); }
        else { OtherFunctions.ChangeSprite(gamepadIcon, "Sprites/controllerIcons", 2); }

        isGamepadAssigned = true;
    }

    private void CheckPlayerStatus()
    {
        if (playerID == 0)
        {
            OtherFunctions.ChangeSprite(Obj, "Sprites/PlayerWaitTab", 0);
            textColor = new Color(0f / 255f, 138f / 255f, 255f / 255f);
        }
        else if (playerID == 1)
        {
            OtherFunctions.ChangeSprite(Obj, "Sprites/PlayerWaitTab", 1);
            textColor = new Color(255f / 255f, 126f / 255f, 0f / 255f);
        }

        if (conditionText != null)
        {
            conditionText.GetComponent<SpriteRenderer>().color = textColor;
            if (isPlayerReady) { OtherFunctions.ChangeSprite(conditionText, "Sprites/WaitConditionText", 1); }
            else
            {
                OtherFunctions.ChangeSprite(conditionText, "Sprites/WaitConditionText", 0);
                isGamepadAssigned = false;
                if (gamepadIcon != null) { Destroy(gamepadIcon); }
            }
        }
    }
}
