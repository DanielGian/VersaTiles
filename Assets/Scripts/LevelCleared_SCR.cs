using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class LevelCleared_SCR : MonoBehaviour
{
    private GameObject Obj;
    private const string Resource = "Sprites/GameplayUI/LevelClearedTiles";
    private GameObject[] LEVELCLEARED_Tiles = new GameObject[12];
    private int tileIndex = 0;
    private float timeVal = 0f;
    private const float timeInterval = 0.1f;
    private bool time_running = false;

    private void Awake()
    {
        Obj = this.gameObject;
        for (int i = 0; i < LEVELCLEARED_Tiles.Length; i++)
        {
            int XIndex = (i < 5 ? i % 5 : i - 5);
            int YIndex = (i < 5 ? 0 : 1);
            float XVal = -96f + (96f * XIndex);
            float YVal = -104f - (104f * YIndex);

            LEVELCLEARED_Tiles[i] = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", Obj.transform.position + new Vector3(XVal, YVal, -5f));
            OtherFunctions.ChangeSprite(LEVELCLEARED_Tiles[i], Resource, i);
            LEVELCLEARED_Tiles[i].transform.localScale = new Vector3(0f, 0f, 0f);
            LEVELCLEARED_Tiles[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            LEVELCLEARED_Tiles[i].transform.SetParent(Obj.transform);
        }
        time_running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (time_running)
        {
            if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
            else
            {
                StartCoroutine(EasingFunctions.ColorChangeFromHex(LEVELCLEARED_Tiles[tileIndex], "#ffffff", 0.5f, 1f));
                StartCoroutine(EasingFunctions.ScaleTo(LEVELCLEARED_Tiles[tileIndex], new Vector2(1f, 1f), 0.5f, 3, Easing.EaseOut));
                StartCoroutine(EasingFunctions.RelRotateCycles(LEVELCLEARED_Tiles[tileIndex], 4, Axis.Z, 0.5f, false, 3, Easing.EaseOut));
                if (tileIndex == 4) { timeVal = 0.1f; }
                else { timeVal = timeInterval; }
                tileIndex++;

                if (tileIndex == 12) { time_running = false; }
            }
        }
    }
}
