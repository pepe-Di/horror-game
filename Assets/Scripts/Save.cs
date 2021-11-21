using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    //[SerializeField] private PlayerData playerData = new PlayerData();
    public static Save instance;
    private void Start()
    {
        instance = this;
    }
    public void SaveData()
    {
       // SaveSystem.SavePlayer(this);
    }
    public void LoadData()
    {
        
    }
}
[System.Serializable]
public class Item
{
    public string name;
    public string descr;
    public int index;
    Item()
    {
    }
}

[System.Serializable]
public class PlayerData
{
    public string name;
    public float hp;
    public float stamina;
    public int sceneIndex; 
    public float[] position;
    //public List<Item> items = new List<Item>();
    public PlayerData(Player player)
    {
        sceneIndex = GameManager.instance.GetSceneIndex();
        hp = player.hp;
        stamina = player.stamina;
        name = player.name_;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}

[System.Serializable]
public class EnemyData 
{
    EnemyData()
    {

    }
}
[System.Serializable]
public class PersonData
{
    public float hp;
    public float stamina;
    public string name;
    public float[] position;
    public State state;
    public List<string> quests = new List<string>();
    public PersonData(GameObject player)
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
[System.Serializable]
public class LevelData
{
    public int sceenIndex;
    public string name;

}
[System.Serializable]
public class Quest
{
    public string name;
    public string descr;
    Quest()
    {

    }
}