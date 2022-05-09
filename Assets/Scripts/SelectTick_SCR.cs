using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class SelectTick_SCR : MonoBehaviour
{
    private const string Resource = "Sprites/selectionTicks";
    private GameObject Obj;
    private SpriteRenderer SPR;
    private Sprite[] frames;
    private float timeVal = 0f;
    private const float timeValMax = 60f;
    private int borderNum = 0;
    private const float rate = 2f;
    private bool isReturning = false;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        frames = Resources.LoadAll<Sprite>(Resource);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal > 0) { timeVal -= 1f * Time.deltaTime; }
        else
        {
            Vector3 objPosition = Obj.transform.position;
            isReturning = !isReturning;

            if (!isReturning)
            {
                if (borderNum == 0) { Obj.transform.position += new Vector3(-rate, rate, 0f); }
                if (borderNum == 1) { Obj.transform.position += new Vector3(rate, rate, 0f); }
                if (borderNum == 2) { Obj.transform.position += new Vector3(-rate, -rate, 0f); }
                if (borderNum == 3) { Obj.transform.position += new Vector3(rate, -rate, 0f); }
                timeVal = 0.5f;
            }
            else
            {
                if (borderNum == 0) { Obj.transform.position += new Vector3(rate, -rate, 0f); }
                if (borderNum == 1) { Obj.transform.position += new Vector3(-rate, -rate, 0f); }
                if (borderNum == 2) { Obj.transform.position += new Vector3(rate, rate, 0f); }
                if (borderNum == 3) { Obj.transform.position += new Vector3(-rate, rate, 0f); }
                timeVal = 0.5f;
            }
        }
        timeVal = Mathf.Clamp(timeVal, 0f, timeValMax);
    }

    public void SetTickBorder(int num) { OtherFunctions.ChangeSprite(Obj, Resource, num); borderNum = num; }
}
