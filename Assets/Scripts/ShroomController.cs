using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomController : MonoBehaviour
{
    public GameController gameController;
    public GameObject shroom;
    [SerializeField] List<O_Shroom> shrooms;

    void Start()
    {
        shrooms = new List<O_Shroom>();
        for(int i = 0; i < 5; i++)
        {
            SpawnShrooom();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnShrooom()
    {
        float var = Random.Range(0.1f, 0.3f);
        GameObject s = Instantiate(shroom, new Vector3(var, var, 0), Quaternion.identity);
        O_Shroom new_shroom = s.GetComponent<O_Shroom>();

        new_shroom.controller = gameController;
        new_shroom.player = gameController.player.GetComponent<O_Player>();

        shrooms.Add(new_shroom);
    }
}
