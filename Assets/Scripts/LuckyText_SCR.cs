using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class LuckyText_SCR : MonoBehaviour
{
    private GameObject Obj;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private const string Resource = "Sprites/GameplayUI/LuckyText";
    private float timeVal = 0.1f;
    private const float timeInterval = 0.1f;
    private float lifeTime = 1f;
    private bool fadeOut = false;
    private int imageIndex = 0;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        frames = Resources.LoadAll<Sprite>(Resource);

        Obj.transform.localScale = new Vector3(0f,0f,0f);
        StartCoroutine(EasingFunctions.ScaleTo(Obj, new Vector2(1f, 1f), 0.5f, 3, Easing.EaseOut));
        StartCoroutine(EasingFunctions.RelRotateTo(Obj, 2190f, Axis.Z, 0.5f, 4, Easing.EaseOut));
    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
        else
        {
            imageIndex = ++imageIndex % frames.Length;
            OtherFunctions.ChangeSprite(Obj, Resource, imageIndex);
            timeVal = timeInterval;
        }

        if (lifeTime > 0f) { lifeTime -= 1f * Time.deltaTime; }
        else
        {
            if (!fadeOut)
            {
                fadeOut = true;
                StartCoroutine(EasingFunctions.ScaleTo(Obj, new Vector2(3f, 3f), 0.5f, 3, Easing.EaseOut));
                StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#ffffff", 0.5f, 0f));
                lifeTime = 0.5f;
            }
            else { Destroy(Obj); }
        }
    }
}
