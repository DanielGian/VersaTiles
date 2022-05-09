using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class StatsConditionText_SCR : MonoBehaviour
{
    private GameObject Obj;
    private float timeVal = 0.75f;
    private bool time_running = false;

    private void Awake()
    {
        Obj = this.gameObject;
        Obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine(EasingFunctions.TranslateTo(Obj, Obj.transform.position + new Vector3(0f, 128f, 0f), 2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal > 0f)
        {
            timeVal -= 1f * Time.deltaTime;
            if (Obj.GetComponent<SpriteRenderer>().color.a < 1f) { Obj.GetComponent<SpriteRenderer>().color += new Color(0f, 0f, 0f, 0.1f); }
        }
        else
        {
            Obj.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.05f);
        }

        if (Obj.GetComponent<SpriteRenderer>().color.a <= 0f) { Destroy(Obj); }
    }
}
