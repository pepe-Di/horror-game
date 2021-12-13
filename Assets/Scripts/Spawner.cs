using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private System.Random _rnd = new System.Random();
    // inside a methode of the same class
    private void Awake()
    {
        int N_tiles = UnityEngine.Random.Range(3, 5);
        int N_floor = UnityEngine.Random.Range(2, 5);
        //int seed = _rnd.Next(0, 1000);
        int[,,] map = new int[N_tiles, N_tiles,N_floor];
        for (int k=0; k<N_floor;k++)
        {
            for (int i = 0; i< N_tiles; i++)
            {
                for (int j = 0; j < N_tiles; j++)
                {
                    map[i,j,k] = UnityEngine.Random.Range(0, 2);
                }
            }
        }
        for (int k = 0; k < N_floor; k++)
        {
            for (int i = 0; i < N_tiles; i++)
            {
                for (int j = 0; j < N_tiles; j++)
                {
                    switch(map[i, j, k])
                    {
                        case 0:
                            {
                                break;
                            }
                        default: 
                            { 
                                break; 
                            }
                    }
                }
            }
        }
        string s = "";
        for (int k = 0; k < N_floor; k++)
        {
            Debug.Log("[" + k + "]");
            for (int i = 0; i < N_tiles; i++)
            {
                for (int j = 0; j < N_tiles; j++)
                {
                    s += map[i, j, k] + " ";
                }
                Debug.Log(s);
                s = "";
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
//enum RoomType
//{
//    Class,
//    Corridor,
//    EntranceHall,
//    CorridorwthStairs,
//    Ñoncert_Hall,
//    Toilet,
//    Buffet,
//    Gym,
//    Storeroom,
//    Cabinet,
//    Teachers,
//    Library,
//    DressingRoom
//}
[System.Serializable]
class Building
{
    List<Floor> floors;
    Building()
    {

    }
}
class Floor
{
    int N;
    List<Room> rooms;
}
class Room
{
    public Room() 
    {

    }
}
