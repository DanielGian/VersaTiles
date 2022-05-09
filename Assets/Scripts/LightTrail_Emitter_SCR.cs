using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrail_Emitter_SCR : MonoBehaviour
{
    private GameObject Obj;

    private float timeVal = 1f;
    private const float timeValMax = 60f;
    [SerializeField] private bool isEmitting = false;

    private void Awake()
    {
        Obj = this.gameObject;
        timeVal = Random.Range(1f, 3f);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(Obj);
    }

    // Update is called once per frame
    void Update()
    {
        timeVal = Mathf.Clamp(timeVal, 0f, timeValMax);

        if (isEmitting)
        {
            if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
            else
            {
                for (int i = 0; i < Random.Range(1, 3); i++)
                    OtherFunctions.CreateObjectFromResource("Prefabs/TrailBit_PFB", new Vector3(-16f, Random.Range(0f, 880f), 98f));
                timeVal = Random.Range(1f, 3f);
            }
        }
    }

    public void EnableEmitter() { isEmitting = true; }
}
