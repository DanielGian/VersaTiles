using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade_SCR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static IEnumerator Fade(AudioSource Src, float volume, float duration)
    {
        float time = 0f;

        float origVol = Src.volume;
        float finalVol = volume;

        float OV = origVol;
        float FV = finalVol;

        Src.volume = OV;

        while (time <= duration)
        {
            float currentVol = OV;

            currentVol = Mathf.Lerp(OV, FV, time / duration);

            Src.volume = currentVol;

            yield return null;
            time += Time.deltaTime;
        }
        Src.volume = FV;
        time = 0f;
    }
}