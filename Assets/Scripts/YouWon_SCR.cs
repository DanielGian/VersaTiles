using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class YouWon_SCR : MonoBehaviour
{
    private GameObject Obj;
    private const string Resource = "Sprites/GameplayUI/YouWonTiles";
    private GameObject[] YOUWON_Tiles = new GameObject[6];
    private int tileIndex = 0;
    private float timeVal = 0f;
    private const float timeInterval = 0.1f;
    private bool time_running = false;

    private void Awake()
    {
        Obj = this.gameObject;
        for (int i = 0; i < YOUWON_Tiles.Length; i++)
        {
            int XIndex = i % 3;
            int YIndex = (i < 3 ? 0 : 1);
            float XVal = -132f + (132f * XIndex);
            float YVal = -140f - (140f * YIndex);

            YOUWON_Tiles[i] = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", Obj.transform.position + new Vector3(XVal, YVal, -5f));
            OtherFunctions.ChangeSprite(YOUWON_Tiles[i], Resource, i);
            YOUWON_Tiles[i].transform.localScale = new Vector3(0f, 0f, 0f);
            YOUWON_Tiles[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            YOUWON_Tiles[i].transform.SetParent(Obj.transform);
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
                StartCoroutine(EasingFunctions.ColorChangeFromHex(YOUWON_Tiles[tileIndex], "#ffffff", 0.5f, 1f));
                StartCoroutine(EasingFunctions.ScaleTo(YOUWON_Tiles[tileIndex], new Vector2(1f, 1f), 0.5f, 3, Easing.EaseOut));
                StartCoroutine(EasingFunctions.RelRotateCycles(YOUWON_Tiles[tileIndex], 4, Axis.Z, 0.5f, false, 3, Easing.EaseOut));
                if (tileIndex == 2) { timeVal = 0.2f; }
                else { timeVal = timeInterval; }
                tileIndex++;

                if (tileIndex == 6) { time_running = false; }
            }
        }
    }
}
