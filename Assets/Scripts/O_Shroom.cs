using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Shroom : MonoBehaviour
{
    public float speed = 0;
    public float max_speed = 0.04f;
    public float acc = 0.002f;

    public GameController controller;
    public O_Player player;

    public Vector3 target_pos;
    public Vector3 direction;

    public State state;

    public bool test;

    private void OnEnable()
    {
        EventManager.onTargetSet += SetTargetPos;
    }


    void Start()
    {
        target_pos = transform.position;
        state = State.Following;
    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.OnTheWay:
                OnTheWayUpdate();
                break;
            case State.Following:
                FollowingUpdate();
                break;
            default:
                break;
        }
    }

    void IdleUpdate()
    {

    }

    void OnTheWayUpdate()
    {
        Vector3 route = target_pos - transform.position;
        direction = route.normalized;
        float distance = route.magnitude;

        if (distance >= 1)
        {
            speed += acc;
            if (speed > max_speed) speed = max_speed;

            transform.position = transform.position + direction * speed;
        }
        else
        {
            speed = 0;
            state = State.Idle;
        }
    }

    void FollowingUpdate()
    {
        Vector3 player_pos = player.transform.position;
        //trail behind
        target_pos = player_pos - player.looking_direction * 4f;


        Vector3 route = target_pos - transform.position;
        direction = route.normalized;
        float distance = route.magnitude;

        if (distance >= 1)
        {
            speed += acc;
            if (speed > max_speed) speed = max_speed;

            transform.position += direction * speed;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform collided = collision.transform;
        if (collided.tag == "Shroom")
        {
            Vector3 dir = transform.position - collided.position;
            dir.z = 0;
            dir = dir.normalized;

            transform.position += 0.01f * dir;

        }
    }
    void SetTargetPos()
    {
        if (state == State.Idle)
        {
            target_pos = controller.target;
            state = State.OnTheWay;
        }

    }

    private void OnDisable()
    {
        EventManager.onTargetSet -= SetTargetPos;
    }

    public enum State
    {
        Idle,
        OnTheWay,
        Following
    }
}