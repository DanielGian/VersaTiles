using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class BubbleSign_SCR : MonoBehaviour
{
    private GameObject Obj;
    private SpriteRenderer SPR;
    private float timeVal = 0.2f;
    private bool scaleDown = false;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        SPR.color = new Color(1f, 1f, 1f, 0f);
        Obj.transform.localScale = new Vector3(0f, 0f, 1f);
        OtherFunctions.ChangeSprite(Obj, "Sprites/GameplayUI/ExclamationBubble");
        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#ffffff", 0.2f, 1));
        StartCoroutine(EasingFunctions.TranslateTo(Obj, Obj.transform.position + new Vector3(0f, 64f, 0f), 0.2f, 3, Easing.EaseOut));
        StartCoroutine(EasingFunctions.ScaleTo(Obj, new Vector3(1.5f, 1.5f, 1f), 0.2f, 3, Easing.EaseOut));
    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal > 0f) { timeVal -= Time.deltaTime; }
        else
        {
            if (!scaleDown)
            {
                StartCoroutine(EasingFunctions.ScaleTo(Obj, new Vector3(1f, 1f, 1f), 0.2f, 3, Easing.EaseOut));
                scaleDown = true;
                timeVal = 0.4f;
            }
            else
            {
                SPR.color -= new Color(0f, 0f, 0f, 2f * Time.deltaTime);
                if (SPR.color.a <= 0f) { Destroy(Obj); }
            }
        }
    }
}
