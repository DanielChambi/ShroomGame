using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Player : MonoBehaviour
{
    public float spd = 0;
    public float max_spd = 0.15f;
    public float min_spd = 0.001f;
    public float acc = 0.001f;
    public float deacc_factor = 0.01f;

    public Vector3 speed;
    public Vector3 direction;
    public Vector3 looking_direction;
    public Vector3 pointing_direction;

    public Transform pointing_arrow;
    public float arrow_distance;
    public float arrow_curr_distance;
    void Start()
    {
        looking_direction = Vector3.right;
        pointing_direction = Vector3.right;
    }

    void Update()
    {
        //move character

        Movement();


        //update pointing direction

        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = 0;
        pointing_direction = mouse_pos - transform.position;
        float pointing_dis = pointing_direction.magnitude;
        pointing_direction = pointing_direction / pointing_dis;

        pointing_arrow.position = transform.position + pointing_direction * arrow_distance;

        float angle = Mathf.Atan2(pointing_direction.y, pointing_direction.x) * Mathf.Rad2Deg;
        pointing_arrow.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

    }

    void Movement()
    {

        float h_dir = Input.GetAxis("Horizontal");
        float v_dir = Input.GetAxis("Vertical");

        direction = new Vector3(h_dir, v_dir, 0);

        if (h_dir != 0 || v_dir != 0)
        {
            Vector3 acceleration = new Vector3(h_dir, v_dir, 0);
            acceleration = acceleration.normalized;
            acceleration = acceleration * acc;

            speed += acceleration;
            spd = speed.magnitude;
            if (spd >= max_spd)
            {
                speed = speed.normalized;
                speed = speed * max_spd;
                spd = max_spd;
            }

            looking_direction = speed.normalized;

            float angle = Mathf.Atan2(looking_direction.y, looking_direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 30, Vector3.forward);
        }
        else
        {

            if (speed.magnitude > min_spd)
            {
                speed = speed * deacc_factor;
            }
            else
            {
                speed = Vector3.zero;
            }
        }

        spd = speed.magnitude;
        transform.position += speed;
    }
}
