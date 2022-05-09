using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class Playtile_SCR : MonoBehaviour
{
    public enum Tile
    {
        ATKPlus1,       // Attack +1
        ATKPlus2,       // Attack +2
        ATKPlus3,       // Attack +3
        ATKPlus4,       // Attack +4
        MulATKPlus1,    // Multi-Attack +1
        MulATKPlus2,    // Multi-Attack +2
        Guard,          // Guard
        HPPlus5,        // Heart Points +5
        CPPlus1,        // Command Points +1
        CPPlus2,        // Command Points +2
        CmdItem,        // Command Item
        Mul2,           // x2
        Mul3,           // x3
        ElementSwap,    // Element Swap
        ATKPlus0 = 20,       // Attack +0
        MulATKPlus0,    // Multi-Attack +0
        CPPlus0,        // Command Points +0
        HPPlus2,        // Heart Points +2
        Mul1,           // x1
    };

    private GameObject Obj;
    private Shader GreyscaleShader;
    private const string Resource = "Sprites/GameplayUI/PlayTiles";
    private Tile tileType = Tile.ATKPlus1;
    private bool isNullified = false;
    [SerializeField] private int ID = 0;
    private int frameIndex = 0;
    private float grayVal = 0f;
    private float vibration = 0f;
    private Vector3 staticPos;
    private bool isInfected = false;

    private void Awake()
    {
        Obj = this.gameObject;
        GreyscaleShader = Resources.Load<Shader>("Greyscale_SHD");
        staticPos = Obj.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isNullified)
        {
            if (grayVal < 1f)
            {
                grayVal += 0.2f;
                Obj.GetComponent<SpriteRenderer>().material.SetFloat("_EffectAmount", grayVal);
            }
        }
        if (isInfected)
        {
            float randX = UnityEngine.Random.Range(-vibration, vibration);
            float randY = UnityEngine.Random.Range(-vibration, vibration);
            Obj.transform.position = staticPos + new Vector3(randX, randY, 0f);

            if (vibration > 0f) { vibration -= 0.25f; }
            else
            {
                vibration = 0;
                isInfected = false;
                Obj.transform.position = staticPos;
            }
        }
    }

    public void EnableGreyscale()
    {
        Obj.GetComponent<SpriteRenderer>().material.shader = GreyscaleShader;
        isNullified = true;
    }

    public bool isTileNullified() { return isNullified; }

    public int GetID() { return ID; }
    public void SetID(int id)
    {
        ID = id;
        Obj.name = $"Playtile {id}";
    }
    public int GetFrameIndex() { return frameIndex; }
    public Tile GetTileType() { return tileType; }
    public void SetTileType(Tile t) { tileType = t; }
    public int GetElementIndex() { return frameIndex; }
    public IEnumerator InfectTile(Color infectedColor, int typeNum)
    {
        isInfected = true;
        StartCoroutine(EasingFunctions.ColorChangeFromRGBA(Obj, infectedColor, 0.3f, Format.Scalar));
        switch (typeNum)
        {
            //Attack +1 Tile
            case 0: { AssignTileType(20); break; }
            //Attack +2 Tile
            case 1: { AssignTileType(0); break; }
            //Attack +3 Tile
            case 2: { AssignTileType(1); break; }
            //Attack +4 Tile
            case 3: { AssignTileType(2); break; }
            //Multi-Attack +1 Tile
            case 4: { AssignTileType(21); break; }
            //Multi-Attack +2 Tile
            case 5: { AssignTileType(4); break; }
            //Heart Points +5 Tile
            case 7: { AssignTileType(23); break; }
            //Command Points +1 Tile
            case 8: { AssignTileType(22); break; }
            //Command Points +2 Tile
            case 9: { AssignTileType(8); break; }
            //x2 Tile
            case 11: { AssignTileType(24); break; }
        }
        for (int i = 0; i < 10; i++)
        {
            vibration += 2;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.2f);

        StartCoroutine(EasingFunctions.ColorChangeFromRGBA(Obj, new Color(1f, 1f, 1f, 1f), 0.5f, Format.Scalar));
    }

    public void AssignTileType(int typeNum, int frameNum = -1)
    {
        OtherFunctions.ChangeSprite(Obj, Resource, typeNum);
        switch (typeNum)
        {
            //Attack +1 Tile
            case 0:
                {
                    tileType = Tile.ATKPlus1;
                    OtherFunctions.ChangeSprite(Obj, Resource, 0);
                    break;
                }
            //Attack +2 Tile
            case 1:
                {
                    tileType = Tile.ATKPlus2;
                    OtherFunctions.ChangeSprite(Obj, Resource, 1);
                    break;
                }
            //Attack +3 Tile
            case 2:
                {
                    tileType = Tile.ATKPlus3;
                    OtherFunctions.ChangeSprite(Obj, Resource, 2);
                    break;
                }
            //Attack +4 Tile
            case 3:
                {
                    tileType = Tile.ATKPlus4;
                    OtherFunctions.ChangeSprite(Obj, Resource, 3);
                    break;
                }
            //Multi-Attack +1 Tile
            case 4:
                {
                    tileType = Tile.MulATKPlus1;
                    OtherFunctions.ChangeSprite(Obj, Resource, 4);
                    break;
                }
            //Multi-Attack +2 Tile
            case 5:
                {
                    tileType = Tile.MulATKPlus2;
                    OtherFunctions.ChangeSprite(Obj, Resource, 5);
                    break;
                }
            //Guard Tile
            case 6:
                {
                    tileType = Tile.Guard;
                    OtherFunctions.ChangeSprite(Obj, Resource, 6);
                    break;
                }
            //Heart Points +5 Tile
            case 7:
                {
                    tileType = Tile.HPPlus5;
                    OtherFunctions.ChangeSprite(Obj, Resource, 7);
                    break;
                }
            //Command Points +1 Tile
            case 8:
                {
                    tileType = Tile.CPPlus1;
                    OtherFunctions.ChangeSprite(Obj, Resource, 8);
                    break;
                }
            //Command Points +2 Tile
            case 9:
                {
                    tileType = Tile.CPPlus2;
                    OtherFunctions.ChangeSprite(Obj, Resource, 9);
                    break;
                }
            //Command Tile
            case 10:
                {
                    tileType = Tile.CmdItem;
                    OtherFunctions.ChangeSprite(Obj, Resource, 10);
                    break;
                }
            //x2 Tile
            case 11:
                {
                    tileType = Tile.Mul2;
                    OtherFunctions.ChangeSprite(Obj, Resource, 11);
                    break;
                }
            //x3 Tile
            case 12:
                {
                    tileType = Tile.Mul3;
                    OtherFunctions.ChangeSprite(Obj, Resource, 12);
                    break;
                }
            //Element Swap Tile
            case 13:
                {
                    tileType = Tile.ElementSwap;
                    if (frameNum == -1) { frameIndex = Random.Range(13, 20); }
                    else { frameIndex = frameNum; }
                    OtherFunctions.ChangeSprite(Obj, Resource, frameIndex);
                    break;
                }
            //Attack +0 Tile
            case 20:
                {
                    tileType = Tile.ATKPlus0;
                    OtherFunctions.ChangeSprite(Obj, Resource, 20);
                    break;
                }
            //Multi-Attack +0 Tile
            case 21:
                {
                    tileType = Tile.MulATKPlus0;
                    OtherFunctions.ChangeSprite(Obj, Resource, 21);
                    break;
                }
            //Command Points +0 Tile
            case 22:
                {
                    tileType = Tile.CPPlus0;
                    OtherFunctions.ChangeSprite(Obj, Resource, 22);
                    break;
                }
            //Heart Points +2 Tile
            case 23:
                {
                    tileType = Tile.HPPlus2;
                    OtherFunctions.ChangeSprite(Obj, Resource, 23);
                    break;
                }
            //x1 Tile
            case 24:
                {
                    tileType = Tile.Mul1;
                    OtherFunctions.ChangeSprite(Obj, Resource, 24);
                    break;
                }
        }
    }
}
