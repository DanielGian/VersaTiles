using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Sunlight_SCR : MonoBehaviour
{
    private GameObject Obj;
    private Light2D sunlight;
    private bool flicker = false;
    private bool disperse = false;

    private void Awake()
    {
        Obj = this.gameObject;
        sunlight = Obj.transform.Find("Sunlight").gameObject.GetComponent<Light2D>();
        sunlight.intensity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (flicker) { LightFlicker(); }
        if (disperse)
        {

            if (sunlight.intensity <= 0f)
            {
                if (Obj.GetComponent<SpriteRenderer>().color.a > 0f) { Obj.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.04f); }
                else { Destroy(Obj); }
            }
            else
            {
                sunlight.pointLightOuterRadius += 32f;
                sunlight.intensity -= 0.02f;
            }
        }
    }

    private void LightFlicker()
    {
        float flickerValue = Random.Range(1.5f, 2.75f);
        sunlight.intensity = flickerValue;
    }
    public void ReleaseLight()
    {
        FlickerOFF();
        sunlight.intensity = 2.75f;
        disperse = true;
    }
    public void FlickerON() { flicker = true; }
    public void FlickerOFF() { flicker = false; }
}
