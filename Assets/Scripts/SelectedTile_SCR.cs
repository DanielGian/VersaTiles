using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTile_SCR : MonoBehaviour
{
    private GameObject Obj;
    private GameObject Highlight;
    private GameObject[] selectTick = new GameObject[4];

    private void Awake()
    {
        Obj = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateSelection(88f, 96f, new Color(0f, 138f/255f, 1f, 1f), 1f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Highlight != null)
        {
            float opacity = ((Mathf.Sin(1f * DateTime.Now.Millisecond / 150f) + 1));
            opacity = Mathf.Clamp(opacity, 0.1f, 0.75f);
            Highlight.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, opacity);
        }
    }

    public void CreateSelection(float width, float height, Color col, float scale = 1f, float padding = 0f)
    {
        Vector3 pos = Obj.transform.position;

        for (int i = 0; i < selectTick.Length; i++)
        {
            if (selectTick[i] == null)
            {
                Vector3 relPos = new Vector3(0f, 0f, 0f);
                if (i == 0) { relPos = new Vector3((-width / 2f) - padding, (height / 2f) + padding, -2f); }        //Top-left corner
                else if (i == 1) { relPos = new Vector3((width / 2f) + padding, (height / 2f) + padding, -2f); }    //Top-right corner
                else if (i == 2) { relPos = new Vector3((-width / 2f) - padding, (-height / 2f) - padding, -2f); }  //Bottom-left corner
                else if (i == 3) { relPos = new Vector3((width / 2f) + padding, (-height / 2f) - padding, -2f); }   //Bottom-right corner

                selectTick[i] = OtherFunctions.CreateObjectFromResource("Prefabs/SelectTick_PFB", pos + relPos);
                selectTick[i].GetComponent<SelectTick_SCR>().SetTickBorder(i);
                selectTick[i].transform.localScale = new Vector3(scale, scale, 1f);
                selectTick[i].transform.SetParent(Obj.transform);
                selectTick[i].GetComponent<SpriteRenderer>().color = col;
            }
        }

        Highlight = OtherFunctions.CreateObjectFromResource("Prefabs/EmptyObject_PFB", pos + new Vector3(0f,0f,-1f));
        Highlight.transform.SetParent(Obj.transform);
        OtherFunctions.ChangeSprite(Highlight, "Sprites/TinyPixel");
        Highlight.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(132, 140f, 1f);
        Highlight.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, 1f);
    }
}
