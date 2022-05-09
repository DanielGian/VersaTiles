using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCmdChoice_SCR : MonoBehaviour
{
    private GameObject Obj;
    private GameObject[] selectTick = new GameObject[4];
    private bool isSelected = false;
    private Color color = new Color(1f, 1f, 1f, 1f);

    private void Awake()
    {
        Obj = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected) { Obj.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f); }
        else { Obj.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.5f); }
    }

    public void Selected(Color selectedColor)
    {
        color = selectedColor;
        CreateSelection(160f, 62f, new Color(0f, 138f / 255f, 1f, 1f), 1f, 4f);
        isSelected = true;
        Debug.Log("Colored!");
    }
    public void Deselected(Color selectedColor)
    {
        color = selectedColor;
        for (int i = 0; i < selectTick.Length; i++) { Destroy(selectTick[i]); }
        isSelected = false;
        Debug.Log("Uncolored!");
    }

    public void CreateSelection(float width, float height, Color col, float scale = 1f, float padding = 0f)
    {
        Vector3 pos = Obj.transform.position;

        for (int i = 0; i < selectTick.Length; i++)
        {
            if (selectTick[i] == null)
            {
                Vector3 relPos = new Vector3(0f, 0f, 0f);
                if (i == 0) { relPos = new Vector3((-width / 2f) - padding, (height / 2f) + padding, -505f); }        //Top-left corner
                else if (i == 1) { relPos = new Vector3((width / 2f) + padding, (height / 2f) + padding, -505f); }    //Top-right corner
                else if (i == 2) { relPos = new Vector3((-width / 2f) - padding, (-height / 2f) - padding, -505f); }  //Bottom-left corner
                else if (i == 3) { relPos = new Vector3((width / 2f) + padding, (-height / 2f) - padding, -505f); }   //Bottom-right corner

                selectTick[i] = OtherFunctions.CreateObjectFromResource("Prefabs/SelectTick_PFB", pos + relPos);
                selectTick[i].GetComponent<SelectTick_SCR>().SetTickBorder(i);
                selectTick[i].transform.localScale = new Vector3(scale, scale, 1f);
                selectTick[i].transform.SetParent(Obj.transform);
                selectTick[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
                selectTick[i].GetComponent<SpriteRenderer>().color = col;
            }
        }
    }
}
