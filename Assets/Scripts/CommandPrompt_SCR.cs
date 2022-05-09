using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandPrompt_SCR : MonoBehaviour
{
    private GameObject Obj;
    private GameObject BM;
    private SpriteRenderer SPR;
    private GameObject Name_Text;
    private GameObject Desc_Text;
    private GameObject Cost_Text;
    private GameObject Affects_Text;
    private GameObject Rarity_Text;
    private GameObject Highlight;
    private GameObject[] selectTicks = new GameObject[4];
    private GameObject[] ChoiceBoxes = new GameObject[3];
    private TMP_Text[] Choice_Text = new TMP_Text[3];
    private int choiceNum = 0;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        BM = GameObject.Find("BattleManager");
        Name_Text = Obj.transform.Find("Canvas/Name Text").gameObject;
        Desc_Text = Obj.transform.Find("Canvas/Description Text").gameObject;
        Cost_Text = Obj.transform.Find("Canvas/CP_Cost Text").gameObject;
        Affects_Text = Obj.transform.Find("Canvas/Affects Text").gameObject;
        Rarity_Text = Obj.transform.Find("Canvas/Rarity Text").gameObject;

        Name_Text.transform.position = Obj.transform.position + new Vector3(-108f, 153.6f, -1f);
        Desc_Text.transform.position = Obj.transform.position + new Vector3(-108f, 38f, -1f);
        Cost_Text.transform.position = Obj.transform.position + new Vector3(-108f, -90f, -1f);
        Affects_Text.transform.position = Obj.transform.position + new Vector3(-108f, -130f, -1f);
        Rarity_Text.transform.position = Obj.transform.position + new Vector3(-108f, -170f, -1f);

        GameManager_SCR.PlaySound(10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectUp()
    {
        Vector3 pos = ChoiceBoxes[choiceNum].transform.position;

        choiceNum = (choiceNum == 0 ? 2 : --choiceNum);
        FillChoice();
    }
    public void SelectDown()
    {
        Vector3 pos = ChoiceBoxes[choiceNum].transform.position;

        choiceNum = ++choiceNum % ChoiceBoxes.Length;
        FillChoice();
    }
    public void TransferCommandInfo(string name, string desc, int cp_cost, string affectsWho, string rarity, Color col, bool hasEnoughCP)
    {
        string cantUseText = "";
        if (!hasEnoughCP)
        {
            choiceNum = 1;
            cantUseText = "(Not Enough)";
        }
        Name_Text.GetComponent<TMP_Text>().text = name;
        Desc_Text.GetComponent<TMP_Text>().text = desc;
        Cost_Text.GetComponent<TMP_Text>().text = $"Cost: {cp_cost} CP <color=#ff0000>{cantUseText}</color>";
        Affects_Text.GetComponent<TMP_Text>().text = $"Affects: {affectsWho}";
        Rarity_Text.GetComponent<TMP_Text>().text = $"Rarity: {rarity}";
        SPR.color = col;

        for (int i = 0; i < ChoiceBoxes.Length; i++)
        {
            float yOffset = -96f * i;
            ChoiceBoxes[i] = OtherFunctions.CreateObjectFromResource("Prefabs/SelectBox_PFB", Obj.transform.position + new Vector3(248f, 96f + yOffset, -1f));
            Choice_Text[i] = ChoiceBoxes[i].transform.Find("Canvas/Choice Text").gameObject.GetComponent<TMP_Text>();
            Choice_Text[i].transform.position = ChoiceBoxes[i].transform.position + new Vector3(0f, 0f, -1f);

            if (i == 0) { Choice_Text[i].text = $"Use"; }
            else if (i == 1) { Choice_Text[i].text = $"Discard"; }
            else { Choice_Text[i].text = $"Exit"; }

            ChoiceBoxes[i].transform.SetParent(Obj.transform);
            if (i == choiceNum)
            {
                ChoiceBoxes[i].GetComponent<SelectedCmdChoice_SCR>().Selected(SPR.color);
                Choice_Text[i].color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                ChoiceBoxes[i].GetComponent<SelectedCmdChoice_SCR>().Deselected(SPR.color);
                Choice_Text[i].color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    public int GetChoice() { return choiceNum; }
    private void FillChoice()
    {
        for (int i = 0; i < ChoiceBoxes.Length; i++)
        {
            if (i == choiceNum)
            {
                ChoiceBoxes[i].GetComponent<SelectedCmdChoice_SCR>().Selected(SPR.color);
                Choice_Text[i].color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                ChoiceBoxes[i].GetComponent<SelectedCmdChoice_SCR>().Deselected(SPR.color);
                Choice_Text[i].color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }
}
