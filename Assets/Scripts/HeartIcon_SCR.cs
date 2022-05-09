using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EaseFunctions;

public class HeartIcon_SCR : MonoBehaviour
{
    private const string Resource = "Sprites/GameplayUI/HeartIcon";
    private GameObject Obj;
    private GameObject Heart_Text;
    private float timeVal = 1f;
    private bool time_running = false;

    private void Awake()
    {
        Obj = this.gameObject;
        Heart_Text = Obj.transform.Find("Canvas/Heart Text").gameObject;
        Obj.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        StartCoroutine(EasingFunctions.TranslateTo(Obj, Obj.transform.position + new Vector3(0f, 64f, 0f), 0.5f, 3, Easing.EaseOut));
    }

    // Update is called once per frame
    void Update()
    {
        if (Heart_Text != null) { Heart_Text.transform.position = Obj.transform.position; }

        if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
        else
        {
            Obj.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.05f);
            Heart_Text.GetComponent<TMP_Text>().color -= new Color(0f, 0f, 0f, 0.05f);
        }

        if (Obj.GetComponent<SpriteRenderer>().color.a <= 0f) { Destroy(Obj); }
    }

    public void AssignHP(int hp)
    {
        Heart_Text.GetComponent<TMP_Text>().text = $"{hp}";
    }
}
