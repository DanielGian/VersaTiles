using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Popup_SCR : MonoBehaviour
{
    public enum PopupType
    {
        None = 0,
        Message,
        Lobby,
        Question
    }
    public enum Choice
    {
        Yes = 0,
        No
    }
    private const string Resource = "Sprites/PopupMessage";
    private GameObject Obj;
    private GameObject GM;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private PopupType PT;
    private Choice choice = Choice.No;
    private Player systemPlayer;
    private GameObject YesButton;
    private GameObject NoButton;
    private GameObject ReadyButton;
    private GameObject P1_Entry;
    private GameObject P2_Entry;
    private GameObject SelectArrow;
    private bool readyPlayer1 = false;
    private bool readyPlayer2 = false;


    private void Awake()
    {
        GM = GameObject.Find("GameManager");
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        frames = Resources.LoadAll<Sprite>(Resource);
        systemPlayer = ReInput.players.GetSystemPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        readyPlayer1 = ReInput.players.Players[0].controllers.joystickCount != 0;
        readyPlayer2 = ReInput.players.Players[1].controllers.joystickCount != 0;

        switch (PT)
        {
            case PopupType.Lobby:
                {
                    if (readyPlayer1) { OtherFunctions.ChangeSprite(P1_Entry, "Sprites/PlayerWaitTabs", 0); }
                    else { OtherFunctions.ChangeSprite(P1_Entry, "Sprites/PlayerWaitTabs", 1); }
                    if (readyPlayer2) { OtherFunctions.ChangeSprite(P2_Entry, "Sprites/PlayerWaitTabs", 2); }
                    else { OtherFunctions.ChangeSprite(P2_Entry, "Sprites/PlayerWaitTabs", 3); }

                    if (readyPlayer1 && readyPlayer2) { OtherFunctions.ChangeSprite(ReadyButton, "Sprites/Popup_Buttons", 4); }
                    else { OtherFunctions.ChangeSprite(ReadyButton, "Sprites/Popup_Buttons", 5); }
                    break;
                }
        }
    }

    public void SetPopupType(PopupType pt)
    {
        PT = pt;
        Vector3 objPosition = Obj.transform.position;

        switch (PT)
        {
            case PopupType.Lobby:
                {
                    OtherFunctions.ChangeSprite(Obj, "Sprites/LobbyMessage");
                    ReadyButton = OtherFunctions.CreateObjectFromResource("Prefabs/PopupButton_PFB", objPosition + new Vector3(0f, -224f, -1f));
                    ReadyButton.transform.SetParent(Obj.transform);
                    OtherFunctions.ChangeSprite(ReadyButton, "Sprites/Popup_Buttons", 5);
                    P1_Entry = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", objPosition + new Vector3(-128f, 16f, -1f));
                    P1_Entry.transform.SetParent(Obj.transform);
                    OtherFunctions.ChangeSprite(P1_Entry, "Sprites/PlayerWaitTabs", 1);
                    P2_Entry = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", objPosition + new Vector3(-128f, -82f, -1f));
                    P2_Entry.transform.SetParent(Obj.transform);
                    OtherFunctions.ChangeSprite(P2_Entry, "Sprites/PlayerWaitTabs", 3);
                    break;
                }
            case PopupType.Question:
                {
                    YesButton = OtherFunctions.CreateObjectFromResource("Prefabs/PopupButton_PFB", objPosition + new Vector3(-225f, -96f, -1f));
                    YesButton.transform.SetParent(Obj.transform);
                    OtherFunctions.ChangeSprite(YesButton, "Sprites/Popup_Buttons", 1);
                    NoButton = OtherFunctions.CreateObjectFromResource("Prefabs/PopupButton_PFB", objPosition + new Vector3(225f, -96f, -1f));
                    NoButton.transform.SetParent(Obj.transform);
                    OtherFunctions.ChangeSprite(NoButton, "Sprites/Popup_Buttons", 3);
                    break;
                }
        }
    }

    public void ChangeSprite(string resource, int frameIndex = 0)
    {
        frames = Resources.LoadAll<Sprite>(resource);
        SPR.sprite = frames[frameIndex];
    }

    public bool IsQuestion() { return PT == PopupType.Question; }

    public void ToggleChoice()
    {
        choice = (choice == Choice.No ? Choice.Yes : Choice.No);

        EnableSelect();
    }

    public PopupType GetPopupType() { return PT; } 

    public void ConfirmChoice()
    {
        if (choice == Choice.Yes)
        {
            if (GM.GetComponent<GameManager_SCR>().IsInTutorialRoom())
            {
                GM.GetComponent<GameManager_SCR>().SetTutorialChoice(true);
            }
            else if (GM.GetComponent<GameManager_SCR>().IsInPenaltyRoom())
            {
                GM.GetComponent<GameManager_SCR>().SetPenaltyChoice(true);
            }
            else
            {
#if UNITY_EDITOR
                //Quit the game in Unity Editor
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
        else if (choice == Choice.No)
        {
            if (GM.GetComponent<GameManager_SCR>().IsInTutorialRoom())
            {
                GM.GetComponent<GameManager_SCR>().SetTutorialChoice(false);
            }
            else if (GM.GetComponent<GameManager_SCR>().IsInPenaltyRoom())
            {
                GM.GetComponent<GameManager_SCR>().SetPenaltyChoice(false);
            }
            else { return; }
        }
    }

    public void EnableSelect()
    {
        Vector3 objPosition = Obj.transform.position;

        if (SelectArrow == null)
        {
            SelectArrow = OtherFunctions.CreateObjectFromResource("Prefabs/SelectArrow_PFB", new Vector3(objPosition.x + 225f - 180f, objPosition.y - 96f, -102f));
            SelectArrow.transform.SetParent(Obj.transform);
        }
        if (choice == Choice.Yes)
        {
            SelectArrow.transform.position = new Vector3(objPosition.x - 225f - 180f, objPosition.y - 96f, -102f);
            OtherFunctions.ChangeSprite(YesButton, "Sprites/Popup_Buttons", 0);
            OtherFunctions.ChangeSprite(NoButton, "Sprites/Popup_Buttons", 3);
        }
        else if (choice == Choice.No)
        {
            SelectArrow.transform.position = new Vector3(objPosition.x + 225f - 180f, objPosition.y - 96f, -102f);
            OtherFunctions.ChangeSprite(YesButton, "Sprites/Popup_Buttons", 1);
            OtherFunctions.ChangeSprite(NoButton, "Sprites/Popup_Buttons", 2);
        }
    }
}
