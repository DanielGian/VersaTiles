using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiler_SCR : MonoBehaviour
{
    public enum Emote
    {
        Idle = 0,
        Walk,
        Happy,
        Hurt
    }
    private const string Resource = "Sprites/Tiler/Tiler_idle";
    private GameObject Obj;
    private Vector3 staticPos;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private int frameIndex = 0;
    private float deltatime = 0f;
    private float playbackSpeed = 5f;
    private bool hasTakenDamage = false;
    private float vibration = 0f;
    private Emote Emotion = Emote.Idle;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        frames = Resources.LoadAll<Sprite>(Resource);
        staticPos = Obj.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayBack();

        if (hasTakenDamage)
        {
            float randX = UnityEngine.Random.Range(-vibration, vibration);
            float randY = UnityEngine.Random.Range(-vibration, vibration);
            Obj.transform.position = staticPos + new Vector3(randX, randY, 0f);

            if (vibration > 0f) { vibration -= 0.25f; }
            else { vibration = 0; hasTakenDamage = false; }
        }
        else { Obj.transform.position = staticPos; }
    }

    public void ChangeEmote(Emote emote)
    {
        Emotion = emote;
        deltatime = 0f;
        frameIndex = 0;

        switch (Emotion)
        {
            case Emote.Idle:
                {
                    staticPos = Obj.transform.position;
                    frames = Resources.LoadAll<Sprite>("Sprites/Tiler/Tiler_idle");
                    break;
                }
            case Emote.Walk:
                {
                    frames = Resources.LoadAll<Sprite>("Sprites/Tiler/Tiler_walk");
                    break;
                }
            case Emote.Happy:
                {
                    frames = Resources.LoadAll<Sprite>("Sprites/Tiler/Tiler_happy");
                    break;
                }
            case Emote.Hurt:
                {
                    frames = Resources.LoadAll<Sprite>("Sprites/Tiler/Tiler_hurt");
                    break;
                }
        }
    }
    public void Vibrate(int intensity)
    {
        hasTakenDamage = true;
        vibration = intensity;
    }

    private void PlayBack()
    {
        deltatime += (playbackSpeed * Time.deltaTime);
        if (deltatime >= 1f)
        {
            deltatime -= 1f;
            frameIndex++;
        }
        SPR.sprite = frames[frameIndex % frames.Length];
    }
}
