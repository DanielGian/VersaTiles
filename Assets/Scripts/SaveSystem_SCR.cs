using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem_SCR : MonoBehaviour
{
    private void Awake()
    {
        PlayerStats PS = new PlayerStats
        {
            MaxHP = 0,
            MaxCP = 0
        };
        string json = JsonUtility.ToJson(PS);
        Debug.Log(PS);
    }
    public static void SaveStats()
    {
    }

    public static void LoadStats()
    {
        string saveString = File.ReadAllText(Application.persistentDataPath + "/Player_Stats.txt");
    }

    private class PlayerStats
    {
        public int MaxHP;
        public int MaxCP;
    }
}
