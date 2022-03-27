using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData", fileName = "new GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] public float master_vol, music_vol, effects_vol, vol,vol1,vol2, sens,txt_speed;
    [SerializeField] public int lg, q, res, style, last_slot, cur_slot;
    [SerializeField] public bool fc;

    public GameData()
    {
        master_vol = 0; music_vol = 0; effects_vol = 0;
        vol = 1; vol1 = 1; vol2 = 1; lg = 0; sens = 1; style = 0; 
        cur_slot = -1; last_slot = -1; txt_speed = 1; res = -1;
    }
    public void ForceSerialization()

    {

#if UNITY_EDITOR

        UnityEditor.EditorUtility.SetDirty(this);

#endif

    }
}
//[System.Serializable]
//public class Item
//{
//    public string name;
//    public string descr;
//    public int index;
//    Item()
//    {
//    }
//}
//[System.Serializable]
//public class EnemyData 
//{
//    EnemyData()
//    {

//    }
//}
//[System.Serializable]
//public class PersonData
//{
//    public float hp;
//    public float stamina;
//    public string name;
//    public float[] position;
//    public State state;
//    public List<string> quests = new List<string>();
//    public PersonData(GameObject player)
//    {
//        position = new float[3];
//        position[0] = player.transform.position.x;
//        position[1] = player.transform.position.y;
//        position[2] = player.transform.position.z;
//    }
//}
//[System.Serializable]
//public class LevelData
//{
//    public int sceenIndex;
//    public string name;

//}
//[System.Serializable]
//public class Quest
//{
//    public string name;
//    public string descr;
//    Quest()
//    {

//    }
//}