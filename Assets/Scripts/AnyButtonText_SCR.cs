using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class AnyButtonText_SCR : MonoBehaviour
{
    public enum BlinkType
    {
        None = 0,
        In,
        Out
    }

    private const string Resource = "Sprites/AnyButton_Text";
    private GameObject Obj;
    private GameObject GM;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private Player systemPlayer;

    private bool isBlinking = false;
    private bool isConfirmed = false;
    [SerializeField] private BlinkType Blinking = BlinkType.In;
    [SerializeField] private float blinkTime;
    [SerializeField] private float blinkInterval = 1f;
    [SerializeField] private float blinkRate = 5f;

    private void Awake()
    {
        Obj = this.gameObject;
        GM = GameObject.Find("GameManager");
        SPR = Obj.GetComponent<SpriteRenderer>();
        frames = Resources.LoadAll<Sprite>(Resource);
        blinkTime = blinkInterval;
        systemPlayer = ReInput.players.GetSystemPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isBlinking)
        {
            if (ReInput.controllers.GetAnyButton() && !isConfirmed)
            {
                GameManager_SCR.PlaySound(1);
                blinkInterval = 0.02f; blinkRate = 25f;
                Blinking = BlinkType.None;
                blinkTime = blinkInterval;
                SPR.color = new Color(1f, 1f, 1f, 1f);
                SPR.sprite = frames[Random.Range(1, 6)];
                GM.GetComponent<GameManager_SCR>().StartTimeline(GameManager_SCR.TimelineScript.TitleToMenu, 2f);
                isConfirmed = true;
            }
            if (blinkTime > 0f) { blinkTime -= 1f * Time.deltaTime; }
            else
            {
                switch (Blinking)
                {
                    case BlinkType.None:
                        {
                            Blinking = BlinkType.Out;
                            break;
                        }
                    case BlinkType.In:
                        {
                            if (SPR.color.a < 1f) { SPR.color += new Color(0f, 0f, 0f, blinkRate * Time.deltaTime); }
                            else { Blinking = BlinkType.None; blinkTime = blinkInterval; }
                            break;
                        }
                    case BlinkType.Out:
                        {
                            if (SPR.color.a >= 0f) { SPR.color -= new Color(0f, 0f, 0f, blinkRate * Time.deltaTime); }
                            else { Blinking = BlinkType.In; }
                            break;
                        }
                }
            }
        }
    }

    public void StartBlinking() { isBlinking = true; }
}
