using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EaseFunctions;

public class DamageIcon_SCR : MonoBehaviour
{
    private const string Resource = "Sprites/GameplayUI/DamageIcon";
    private GameObject Obj;
    private GameObject Damage_Text;
    private int colorNum = 0;
    private float timeVal = 0.75f;
    private bool time_running = false;

    private void Awake()
    {
        Obj = this.gameObject;
        Damage_Text = Obj.transform.Find("Canvas/Damage Text").gameObject;
        Obj.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Obj.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 66f / 255f);
        StartCoroutine(EasingFunctions.TranslateTo(Obj, Obj.transform.position + new Vector3(64f, 32f, 0f), 0.2f, 4, Easing.EaseOut));
    }

    // Update is called once per frame
    void Update()
    {
        if (Damage_Text != null) { Damage_Text.transform.position = Obj.transform.position; }

        if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
        else
        {
            Obj.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.05f);
            Damage_Text.GetComponent<TMP_Text>().color -= new Color(0f, 0f, 0f, 0.05f);
        }
        
        if (Obj.GetComponent<SpriteRenderer>().color.a <= 0f) { Destroy(Obj); }
    }

    private IEnumerator SwitchColor()
    {
        while (true)
        {
            float alpha = Obj.GetComponent<SpriteRenderer>().color.a;
            switch (colorNum)
            {
                case 0:
                    {
                        Obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                        break;
                    }
                case 1:
                    {
                        Obj.GetComponent<SpriteRenderer>().color = new Color(115f / 255f, 237f / 255f, 1f, alpha);
                        break;
                    }
                case 2:
                    {
                        Obj.GetComponent<SpriteRenderer>().color = new Color(1f, 184f / 255f, 83f / 255f, alpha);
                        break;
                    }
            }
            yield return new WaitForSeconds(0.25f);
            colorNum = ++colorNum % 3;
        }
    }

    public void AssignDamage(int damage)
    {
        Damage_Text.GetComponent<TMP_Text>().text = $"{damage}";
    }

    public void LuckyHit()
    {
        StartCoroutine(SwitchColor());
    }
}

