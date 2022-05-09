using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash_SCR : MonoBehaviour
{
    private GameObject Obj;
    private SpriteRenderer SPR;
    private bool startFlash = false;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startFlash)
        {
            SPR.color -= new Color(0f, 0f, 0f, 1f * Time.deltaTime);
            if (SPR.color.a <= 0f) { Destroy(Obj); }
        }
    }

    public void Flash()
    {
        startFlash = true;
        SPR.color = new Color(1f, 1f, 1f, 1f);
    }
}
