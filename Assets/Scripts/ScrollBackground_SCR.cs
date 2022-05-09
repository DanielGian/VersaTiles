using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground_SCR : MonoBehaviour
{
    private GameObject Obj;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float xLimit;
    [SerializeField] private float yLimit;
    private const float speedMultiplier = 5f;
    private Vector3 startPosition;

    private void Awake()
    {
        Obj = this.gameObject;
        startPosition = Obj.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Obj.transform.position += new Vector3(horizontalSpeed * speedMultiplier * Time.deltaTime, verticalSpeed * speedMultiplier * Time.deltaTime, 0f);
        float displacementX = Mathf.Abs(startPosition.x - Obj.transform.position.x);
        float displacementY = Mathf.Abs(startPosition.y - Obj.transform.position.y);

        //Wrap around the scene if background is outside the boundaries
        if (displacementX >= xLimit && horizontalSpeed != 0f) { Obj.transform.position = new Vector3(startPosition.x, Obj.transform.position.y, Obj.transform.position.z); }
        if (displacementY >= yLimit && verticalSpeed != 0f) { Obj.transform.position = new Vector3(Obj.transform.position.x, startPosition.y, Obj.transform.position.z); }
    }

    /// <summary>
    /// Positions an object in the scene (Converts from screen to world space)
    /// </summary>
    public static void PositionObject(GameObject Obj, Vector3 Pos)
    {
        Vector3 pos = new Vector3(Pos.x, Pos.y, Pos.z);
        Obj.transform.position = new Vector3(pos.x, pos.y, Pos.z);
    }
}
