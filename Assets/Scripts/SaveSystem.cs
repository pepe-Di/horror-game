using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void OnAwake(SaveSlot[] saveSlots, string[] text)
    {
        string path = Application.persistentDataPath + "save.ini";
        for (int i = 0; i < saveSlots.Length; i++)
        {
            path = Application.persistentDataPath + i + "save.ini";
            if (File.Exists(path))
            {
                //���������
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                SaveSlot data = formatter.Deserialize(stream) as SaveSlot;
                saveSlots[i] = data;
                stream.Close();
            }
            else
            {
                //��������� ����
                SaveSlot saveSlot = new SaveSlot(text[i]);
                saveSlot.name = "Empty slot";
                saveSlots[i] = saveSlot;
            }
        }
    }
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        PlayerData data = new PlayerData(player);
        DataManager.saveSlots[DataManager.instance.selectedSlot].Rewrite(data);
        string path = Application.persistentDataPath + DataManager.instance.selectedSlot +"save.ini";
        FileStream stream = new FileStream(path, FileMode.Create); Debug.Log(data.name);
        formatter.Serialize(stream, DataManager.saveSlots[DataManager.instance.selectedSlot]);
        stream.Close();
        Debug.Log("saved"); Debug.Log(path);
    }
    public static SaveSlot LoadPlayer()
    {
        string path = Application.persistentDataPath + DataManager.instance.selectedSlot + "save.ini";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveSlot data = formatter.Deserialize(stream) as SaveSlot; Debug.Log(data.name);
            stream.Close();
            Debug.Log("loaded");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
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
    public string time;
    //public List<Item> items = new List<Item>();
    public PlayerData(Player player)
    {
        time = System.DateTime.Now.ToString();
        sceneIndex = 2;
        //GameManager.instance.GetSceneIndex();
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
public class SaveSlot
{
    public PlayerData playerData;
    public string name;
    public string text;
    public SaveSlot(string text) 
    {
        this.text = text;
    }
    public void Rewrite(PlayerData playerData)
    {
        name = playerData.name+"'s slot";
        text = playerData.name + " " + playerData.time;
        this.playerData = playerData;
    }
    public SaveSlot(PlayerData playerData)
    {
        name = "";
        text = playerData.name + " "+ playerData.time;
        this.playerData = playerData;
    }
}

