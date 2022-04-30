using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void OnAwake(SaveSlot[] saveSlots, string[] text)
    {
        string path;
        for (int i = 0; i < saveSlots.Length; i++)
        {
            path = Application.persistentDataPath +"/SaveSlot_" + i + ".ini";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                SaveSlot data = formatter.Deserialize(stream) as SaveSlot;
                saveSlots[i] = data;
                stream.Close();
            }
            else
            {
                SaveSlot saveSlot = new SaveSlot(text[i]);
                saveSlot.name = "Empty slot";
                saveSlots[i] = saveSlot;
            }
        }
    }
    public static Options LoadOptions()
    {
        string path = Application.persistentDataPath +"/Nekopara_vol_2.exe";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Options data = formatter.Deserialize(stream) as Options;
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
    public static void SaveOptions(Options data)
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath +"/Nekopara_vol_2.exe";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream,data);
            stream.Close();Debug.Log("saved"); Debug.Log(path);
        }
        catch
        {
            Debug.LogError("!");
        }
    }
    public static void ClearSlot(int selectedSlot)
    {
        string path = Application.persistentDataPath + "/SaveSlot_" + selectedSlot + ".ini";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("deleted");
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }
    public static void SavePlayer(Player player, EnemyController enemy)
    {
        try
        {
            int selectedSlot = DataManager.instance.gameData.cur_slot;
            BinaryFormatter formatter = new BinaryFormatter();
            PlayerData data = new PlayerData(player);
            AIdata aiData = new AIdata(enemy);
            DataManager.saveSlots[selectedSlot].Rewrite(data,aiData);
            string path = Application.persistentDataPath + "/SaveSlot_" + selectedSlot + ".ini";
            FileStream stream = new FileStream(path, FileMode.Create); Debug.Log(data.name);
            formatter.Serialize(stream, DataManager.saveSlots[selectedSlot]);
            stream.Close();
            Debug.Log("saved"); Debug.Log(path);
        }
        catch
        {

        }
    }
    public static SaveSlot LoadPlayer()
    {
        int selectedSlot = DataManager.instance.gameData.cur_slot;
        string path = Application.persistentDataPath + "/SaveSlot_"+ selectedSlot + ".ini";
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
public class Options
{
    public float master_vol, music_vol, effects_vol, vol,vol1,vol2, sens,txt_speed;
    public int lg, q, res, style, last_slot, cur_slot,frame_limit;
    public bool frame_mode,particles;
    private bool fc;

    public Options(GameData data)
    {
        this.master_vol = data.master_vol;
        this.music_vol = data.music_vol;
        this.effects_vol = data.effects_vol;
        this.vol = data.vol;
        this.vol1 = data.vol1;
        this.vol2 = data.vol2;
        this.sens = data.sens;
        this.txt_speed = data.txt_speed;
        this.lg = data.lg;
        this.q = data.q;
        this.res = data.res;
        this.style = data.style;
        this.last_slot = data.last_slot;
        this.cur_slot = data.cur_slot;
        this.frame_limit = data.frame_limit;
        this.frame_mode = data.frame_mode;
        this.particles = data.particles;
        this.fc = data.fc;
    }

    public bool Fc { get => fc; set => fc = value; }
}
[System.Serializable]
public class AIdata{
    public float[] position;
    public float[] rotation;
    public int state;
    public AIdata(EnemyController enemy)
    {
        position = new float[3];
        rotation = new float[3];
        position[0] = enemy.transform.position.x;
        position[1] = enemy.transform.position.y;
        position[2] = enemy.transform.position.z;
        rotation[0] = enemy.transform.rotation.x;
        rotation[1] = enemy.transform.rotation.y;
        rotation[2] = enemy.transform.rotation.z;
        state = ((int)enemy.state);
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
    public string[] items;
    public string[] used_items;
    public int[] quests;
    public int[] finished_quests;
    public int state;
    public PlayerData(Player player)
    {
        time = System.DateTime.Now.ToString();
        sceneIndex = 2;
        hp = player.hp;
        stamina = player.stamina;
        name = player.name_;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        state = ((int)player.state);
        items = new string[player.items.Count];
        for(int i = 0; i < items.Length; i++)
        {
            items[i] = player.items[i].GetGmName();
        }
        used_items = new string[player.used_items.Count];
        for(int i = 0; i < used_items.Length; i++)
        {
            used_items[i] = player.used_items[i].GetGmName();
        }
        quests = new int[player.quests.Count];
        for(int i = 0; i < quests.Length; i++)
        {
            quests[i] = player.quests[i].id;
        }
        finished_quests = new int[player.finished_quests.Count];
        for(int i = 0; i < finished_quests.Length; i++)
        {
            finished_quests[i] = player.finished_quests[i].id;
        }
    }
}
[System.Serializable]
public class SaveSlot
{
    public AIdata aiData;
    public PlayerData playerData;
    public string name;
    public string text;
    public bool isEmpty = true;
    public bool selected = false;
    public SaveSlot(string text) 
    {
        this.text = text;
        isEmpty = true;
    }
    public void Rewrite(PlayerData playerData, AIdata aiData)
    {
        name = playerData.name+"'s slot";
        text = playerData.name + " " + playerData.time;
        this.playerData = playerData; 
        this.aiData = aiData; 
        isEmpty = false;
    }
    public void Clear()
    {
        name = "Empty slot";
        playerData = null; 
        aiData = null;
        isEmpty = true;
    }
    public SaveSlot(PlayerData playerData,  AIdata aiData)
    {
        name = "";
        text = playerData.name + " "+ playerData.time;
        this.playerData = playerData;
        this.aiData = aiData; 
        isEmpty = false;
    }
}

