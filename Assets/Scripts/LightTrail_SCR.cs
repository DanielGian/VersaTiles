using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrail_SCR : MonoBehaviour
{
    public enum Go
    {
        Horizontal = 0,
        Vertical
    }

    private GameObject Obj;
    [SerializeField] private float speed;
    private Go TrailDirection = Go.Horizontal;
    private const float speedMultiplier = 5f;

    private float timeVal;
    private const float timeValMax = 60f;

    private void Awake()
    {
        Obj = this.gameObject;
        speed = Random.Range(200f, 400f);
        timeVal = Random.Range(0.3f, 1.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(Obj);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalSpeed = (TrailDirection == Go.Horizontal ? speed : 0f);
        float verticalSpeed = (TrailDirection == Go.Vertical ? speed : 0f);

        Obj.transform.position += new Vector3(horizontalSpeed * speedMultiplier * Time.deltaTime, verticalSpeed * speedMultiplier * Time.deltaTime, 0f);

        timeVal = Mathf.Clamp(timeVal, 0f, timeValMax);
        if (timeVal > 0f) { timeVal -= 1f * Time.deltaTime; }
        else { GoToggle(); }

        if (Obj.transform.position.x > 1920f * 1.75f) { Destroy(Obj); }
    }

    private void GoToggle()
    {
        if (TrailDirection == Go.Horizontal) { TrailDirection = Go.Vertical; timeVal = Random.Range(0.1f, 0.2f); }
        else { TrailDirection = Go.Horizontal; timeVal = Random.Range(1f, 2f); }
    }
}
