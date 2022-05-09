using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EaseFunctions;

public class OtherFunctions : MonoBehaviour
{
    /// <summary>
    /// Instantiates an object from the Resources folder
    /// </summary>
    public static GameObject CreateObjectFromResource(string Resource, Vector3 Pos)
    {
        Vector3 pos = new Vector3(Pos.x, Pos.y, Pos.z);
        GameObject Obj = Instantiate(Resources.Load<GameObject>(Resource), new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        return Obj;
    }
    /// <summary>
    /// Instantiates an object from the Resources folder
    /// </summary>
    public static GameObject CreateObjectFromResource(string Resource, Vector2 Pos)
    {
        Vector2 pos = new Vector2(Pos.x, Pos.y);
        GameObject Obj = Instantiate(Resources.Load<GameObject>(Resource), new Vector2(pos.x, pos.y), Quaternion.identity);
        return Obj;
    }

    /// <summary>
    /// Change the sprite of GameObject
    /// </summary>
    public static void ChangeSprite(GameObject Obj, string Resource, int frameIndex = 0)
    {
        SpriteRenderer SPR = Obj.GetComponent<SpriteRenderer>();
        Sprite[] frames = Resources.LoadAll<Sprite>(Resource);
        SPR.sprite = frames[frameIndex];
    }
}
