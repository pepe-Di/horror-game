using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string name_;
    public float hp, stamina;
    public State state;
   // public int sceneIndex;
   // public State state;
   // public List<Item> items = new List<Item>();
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("name"))
        {
            Fungus.Character character = GetComponent<Fungus.Character>();
            name_ = PlayerPrefs.GetString("name");
            character.nameText = PlayerPrefs.GetString("name");
            PlayerPrefs.DeleteKey("name");
            hp = 100f;
            stamina = 100f;
        }
    }
    public void LoadData(PlayerData data)
    {
        name_ = data.name;
        Fungus.Character character = GetComponent<Fungus.Character>();
        character.nameText = data.name;
        hp = data.hp;
        stamina = data.stamina;
        Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
        transform.position = position;
        Debug.Log("player loaded");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
