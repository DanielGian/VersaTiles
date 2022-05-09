using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class TitleTile_SCR : MonoBehaviour
{
    private const string Resource = "Sprites/Title_tiles";
    private GameObject Obj;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private bool canJump = false;
    private bool isJumping = false;
    private Vector3 objPosition;

    private int SS_Animation_index = 0;
    [SerializeField] private float timeVal = 1f;
    private readonly float timeValMax = 60f;
    private bool isCoroutineRunning = false;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        frames = Resources.LoadAll<Sprite>(Resource);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        objPosition = Obj.transform.position;

        if (canJump)
        {
            if (timeVal > 0) { timeVal -= 1f * Time.deltaTime; }
            else { StartCoroutine(ExecuteTimeline()); }
        }
        timeVal = Mathf.Clamp(timeVal, 0f, timeValMax);
    }

    IEnumerator ExecuteTimeline()
    {
        if (isCoroutineRunning) { yield break; }
        isCoroutineRunning = true;

        SS_Animation_index++;
        switch (SS_Animation_index)
        {
            case 1:
                {
                    isJumping = true;
                    StartCoroutine(EasingFunctions.TranslateTo(Obj, objPosition + new Vector3(0f, 64f, 0f), 0.3f, 3, Easing.EaseOut));
                    timeVal = 0.3f;
                    break;
                }
            case 2:
                {
                    StartCoroutine(EasingFunctions.TranslateTo(Obj, objPosition + new Vector3(0f, -64f, 0f), 0.2f, 3, Easing.EaseIn));
                    timeVal = 0.2f;
                    break;
                }
            case 3:
                {
                    StartCoroutine(EasingFunctions.TranslateTo(Obj, objPosition + new Vector3(0f, 32f, 0f), 0.2f, 3, Easing.EaseOut));
                    timeVal = 0.2f;
                    break;
                }
            case 4:
                {
                    StartCoroutine(EasingFunctions.TranslateTo(Obj, objPosition + new Vector3(0f, -32f, 0f), 0.15f, 3, Easing.EaseIn));
                    timeVal = 0.15f;
                    break;
                }
            case 5:
                {
                    SS_Animation_index = 0;
                    canJump = false;
                    isJumping = false;
                    break;
                }
        }

        isCoroutineRunning = false;
    }

    public void ChangeSprite(string resource, int frameIndex = 0)
    {
        frames = Resources.LoadAll<Sprite>(resource);
        SPR.sprite = frames[frameIndex];
    }

    public void AnimateJump(float time)
    {
        canJump = true;
        timeVal = time;
    }
    public void DisableJump()
    {
        canJump = false;
        timeVal = 4f;
    }

    public bool GetCanJump() { return canJump; }
    public bool GetIsJumping() { return isJumping; }
}
