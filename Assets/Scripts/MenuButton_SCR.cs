using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton_SCR : MonoBehaviour
{
    private const string Resource = "Sprites/MenuButtons";
    private GameObject Obj;
    private SpriteRenderer SPR;
    private Sprite[] frames;

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
        
    }
    
    public void ChangeSprite(string resource, int frameIndex = 0)
    {
        frames = Resources.LoadAll<Sprite>(resource);
        SPR.sprite = frames[frameIndex];
    }
}
