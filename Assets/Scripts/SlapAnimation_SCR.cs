using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class SlapAnimation_SCR : MonoBehaviour
{
    private GameObject Obj;
    private GameObject BM;
    private GameObject Enemy;
    private float timeVal = 0f;
    private int animationIndex = 0;

    private void Awake()
    {
        Obj = this.gameObject;
        BM = GameObject.Find("BattleManager");
        Enemy = BM.GetComponent<BattleManager_SCR>().GetEnemyTarget();
        OtherFunctions.ChangeSprite(Obj, "Sprites/GameplayUI/SlapHand");
        Obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
        else
        {
            Vector3 pos = Obj.transform.position;
            switch (animationIndex)
            {
                case 0:
                    {
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#ffffff", 0.15f, 1));
                        StartCoroutine(EasingFunctions.TranslateTo(Obj, pos + new Vector3(128f, -64f, 0f), 0.18f, 5, Easing.EaseIn));
                        timeVal = 0.18f;
                        break;
                    }
                case 1:
                    {
                        GameManager_SCR.PlaySound(31);
                        Enemy.GetComponent<Enemy_SCR>().ReceiveDamage(1, true, true, 10f);
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#ffffff", 0.18f, 1));
                        StartCoroutine(EasingFunctions.TranslateTo(Obj, pos + new Vector3(128f, 64f, 0f), 0.2f, 5, Easing.EaseOut));
                        StartCoroutine(EasingFunctions.RelRotateTo(Obj, 180f, Axis.Y, 0.1f));
                        timeVal = 0.4f;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#ffffff", 0.1f, 0));
                        timeVal = 0.1f;
                        break;
                    }
                case 3:
                    {
                        Destroy(Obj);
                        break;
                    }
            }
            animationIndex++;
        }
    }
}
