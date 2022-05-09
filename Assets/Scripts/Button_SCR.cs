using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SCR : MonoBehaviour
{
    public enum ButtonType
    {
        ShoulderR = 0,
        ShoulderL
    }
    private string Resource;
    private GameObject Obj;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private int frameIndex = 0;
    private float deltatime = 0f;
    private const float playbackSpeed = 5f;
    [SerializeField] private ButtonType button;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        if (button == ButtonType.ShoulderR) { Resource = "Sprites/GameplayUI/ShoulderR_Button"; }
        else if (button == ButtonType.ShoulderL) { Resource = "Sprites/GameplayUI/ShoulderL_Button"; }
        frames = Resources.LoadAll<Sprite>(Resource);
    }

    // Update is called once per frame
    void Update()
    {
        PlayBack();
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
