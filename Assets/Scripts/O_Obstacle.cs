using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Obstacle : MonoBehaviour
{
    [SerializeField] List<O_Shroom> working_shrooms;

    public float max_hp = 50;
    public float curr_hp;
    void Start()
    {
        curr_hp = max_hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWorkingShroom(O_Shroom s)
    {
        working_shrooms.Add(s);
    }

    public void DealDamage(float dmg)
    {
        curr_hp -= dmg;
        if(curr_hp <= 0)
        {
            Remove();
        }
    }

    void Remove()
    {
        foreach(O_Shroom s in working_shrooms)
        {
            s.FinishWork();
        }

        Destroy(this.gameObject);
    }
}
