using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class StarFX_SCR : MonoBehaviour
{
    private List<Color> StarColor = new List<Color>
    {
        new Color(1f, 221f/255f, 87f/255f, 1f),
        new Color(126f/255f, 1f, 87f/255f, 1f),
        new Color(87f/255f, 207f/255f, 1f, 1f),
        new Color(151f/255f, 87f/255f, 1f, 1f),
    };
    private GameObject Obj;
    private SpriteRenderer SPR;
    private float index = 0;
    private float speed = 0f;

    private void Awake()
    {
        Obj = this.gameObject;
        SPR = Obj.GetComponent<SpriteRenderer>();
        SPR.color = StarColor[Random.Range(0, 4)];
        StartCoroutine(EasingFunctions.ColorChangeFromRGBA(Obj, new Color(SPR.color.r, SPR.color.g, SPR.color.b, 0f), Random.Range(0.5f, 1f), Format.Scalar));
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.AngleAxis(index * 45f, Vector3.forward);
        Vector3 direction = Vector3.right;
        Vector3 rotatedDirection = rotation * direction;
        transform.Translate(rotatedDirection * speed);

        if (SPR.color.a <= 0f) { Destroy(Obj); }
    }

    public void AssignIndex(float i, float spd) { index = i; speed = spd; }
}
