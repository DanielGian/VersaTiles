using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class BiteAnimation_SCR : MonoBehaviour
{
    private GameObject Obj;
    private GameObject BM;
    private GameObject chewyTile;
    private float timeVal = 0f;
    private int frameIndex = 0;
    private int animationIndex = 0;
    private Vector3 staticPos;

    private void Awake()
    {
        Obj = this.gameObject;
        staticPos = Obj.transform.position;
        BM = GameObject.Find("BattleManager");
        Obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
        else
        {
            switch (animationIndex)
            {
                case 0:
                    {
                        OtherFunctions.ChangeSprite(Obj, "Sprites/GameplayUI/Fangs", frameIndex);
                        if (frameIndex == 0) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, 16f, 0f), 0.18f)); }
                        else if (frameIndex == 1) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, -16f, 0f), 0.18f)); }
                        timeVal = 0.18f;
                        break;
                    }
                case 1:
                    {
                        if (chewyTile != null)
                        {
                            chewyTile.GetComponent<Playtile_SCR>().EnableGreyscale();
                            StartCoroutine(EasingFunctions.ScaleYTo(chewyTile, 0.25f, 0.18f));
                        }
                        if (frameIndex == 0) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, 8f, 0f), 0.18f)); }
                        else if (frameIndex == 1) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, -8f, 0f), 0.18f)); }
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#ffffff", 0.18f, 1));
                        timeVal = 0.2f;
                        break;
                    }
                case 2:
                    {
                        if (frameIndex == 0) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, 16f, 0f), 0.1f)); }
                        else if (frameIndex == 1) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, -16f, 0f), 0.1f)); }
                        timeVal = 0.1f;
                        break;
                    }
                case 3:
                    {
                        if (frameIndex == 0) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, 8f, 0f), 0.1f)); }
                        else if (frameIndex == 1) { StartCoroutine(EasingFunctions.TranslateTo(Obj, staticPos + new Vector3(0f, -8f, 0f), 0.1f)); }
                        timeVal = 0.1f;
                        break;
                    }
                case 4:
                    {
                        if (chewyTile != null)
                        {
                            StartCoroutine(EasingFunctions.ScaleYTo(chewyTile, 1f, 0.1f));
                        }
                        StartCoroutine(EasingFunctions.ColorChangeFromHex(Obj, "#ffffff", 0.1f, 0));
                        timeVal = 0.1f;
                        break;
                    }
                case 5:
                    {
                        Destroy(Obj);
                        break;
                    }
            }
            animationIndex++;
        }
    }
    public void SetTile(GameObject tile) { chewyTile = tile; }

    public void SetFrameIndex(int index) { frameIndex = index; }
}
