using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class PanBackground_SCR : MonoBehaviour
{
    public enum Scroll
    {
        Right = 1,
        Left = -1
    }
    private GameObject Obj;
    [SerializeField] private float bkgWidth;
    private bool isScrolling = false;
    [SerializeField] private Scroll ScrollDir = Scroll.Right;
    [SerializeField] private bool startScroll = false;
    private float timeVal = 0f;
    private float panTime = 30f;

    // Start is called before the first frame update
    private void Awake()
    {
        Obj = this.gameObject;
        Obj.transform.position = new Vector3(0f, 540f, 100f);
        bkgWidth = Obj.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (startScroll)
        {
            if (timeVal > 0f) { timeVal -= 1 * Time.deltaTime; }
            else
            {
                bkgWidth = Obj.GetComponent<SpriteRenderer>().bounds.size.x;
                isScrolling = false;
                timeVal = panTime;

                if (!isScrolling)
                {
                    Vector3 currentPos = Obj.transform.position;
                    ScrollDir = (ScrollDir == Scroll.Right ? Scroll.Left : Scroll.Right);
                    isScrolling = true;

                    if (ScrollDir == Scroll.Right)
                    {
                        StartCoroutine(EasingFunctions.TranslateTo(Obj, currentPos - new Vector3(bkgWidth / 4f, 0f, 0f), panTime, 2, Easing.EaseInOut));
                    }
                    else if (ScrollDir == Scroll.Left)
                    {
                        StartCoroutine(EasingFunctions.TranslateTo(Obj, currentPos + new Vector3(bkgWidth / 4f, 0f, 0f), panTime, 2, Easing.EaseInOut));
                    }
                }
            }
        }
    }

    public void ActivateScroll() { startScroll = true; }
}
