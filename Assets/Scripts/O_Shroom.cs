using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Shroom : MonoBehaviour
{
    public float speed = 0;
    public float max_speed_walk = 0.04f;
    public float max_speed_throw = 0.09f;
    public float acc = 0.002f;

    public float trail_player_dis = 3;

    public GameController controller;
    public O_Player player;

    public Vector3 target_pos;
    public Vector3 direction;

    public State state;

    public bool target_reached = false;
    public Vector3 target_reached_pos;

    public bool thrown_positioned = false;

    public O_Obstacle curr_obstacle;
    public float work_cooldown = 1;
    public float work_timer = 0;

    private void OnEnable()
    {
        EventManager.onTargetSet += SetTargetPos;
    }


    void Start()
    {
        target_pos = transform.position;

        target_reached_pos = Vector3.zero;

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
            case State.Thrown:
                ThrownUpdate();
                break;
            case State.Working:
                WorkingUpdate();
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
        float distance = route.sqrMagnitude;

        if (distance >= 1)
        {
            speed += acc;
            if (speed > max_speed_walk) speed = max_speed_walk;

            transform.position = transform.position + direction * speed;
        }
        else
        {
            speed = 0;
            target_reached = true;
            target_reached_pos = target_pos;
            state = State.Idle;
        }
    }

    void FollowingUpdate()
    {
        Vector3 player_pos = player.transform.position;
        //trail behind
        target_pos = player_pos - player.looking_direction * trail_player_dis;


        Vector3 route = target_pos - transform.position;
        direction = route.normalized;
        float distance = route.sqrMagnitude;

        if (distance >= 1)
        {
            if (!target_reached)
            {
                speed += acc;
                if (speed > max_speed_walk) speed = max_speed_walk;

                transform.position += direction * speed;

            } else
            {
                if(target_pos != target_reached_pos)
                {
                    target_reached = false;
                }
            }
        } else
        {
            speed = 0;
            target_reached = true;
            target_reached_pos = target_pos;
        }
    }

    void ThrownUpdate()
    {
        Vector3 route = target_pos - transform.position;
        direction = route.normalized;
        float distance = route.sqrMagnitude;

        if(distance >= 1)
        {
            speed += acc;
            if (speed > max_speed_throw) speed = max_speed_throw;

            transform.position += direction * speed;
        } else
        {
            if (!thrown_positioned)
            {
                thrown_positioned = true;
                target_pos = player.transform.position + player.pointing_direction * player.throw_distance;
            } else
            {
                //search interactive obj
                //if none:
                SetIdle();
            }
        }
    }

    void WorkingUpdate()
    {
        if(curr_obstacle != null)
        {
            work_timer += Time.deltaTime;
            if (work_timer >= work_cooldown)
            {
                curr_obstacle.DealDamage(1);
                work_timer = 0;
            }
        } else
        {
            FinishWork();
        }
    }

    public void SetThrown()
    {
        state = State.Thrown;
        target_pos = player.transform.position + player.pointing_direction * player.arrow_distance;
        thrown_positioned = false;
    }

    public void SetFollowing()
    {
        state = State.Following;
        target_reached = false;
    }

    public void SetIdle()
    {
        state = State.Idle;
    }

    public void SetWorking(O_Obstacle obstacle)
    {
        curr_obstacle = obstacle;
        curr_obstacle.AddWorkingShroom(this);
        state = State.Working;
    }

    public void FinishWork()
    {
        if(state == State.Working)
        {
            curr_obstacle = null;
            work_timer = 0;
            SetIdle();
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


    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform collided = collision.transform;
        if (collided.tag == "Shroom")
        {
            Vector3 dir = transform.position - collided.position;
            dir.z = 0;
            dir = dir.normalized;

            transform.position += 0.01f * dir;

            O_Shroom other_shroom = collided.GetComponent<O_Shroom>();
            if (other_shroom.target_reached)
            {
                if(target_pos == other_shroom.target_pos)
                {
                    target_reached = true;
                    target_reached_pos = target_pos;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform collided = collision.transform;
        if(collided.tag == "Player")
        {
            if (state == State.Idle)
            {
                SetFollowing();
            }
        }
        if(collided.tag == "Obstacle")
        {
            if(state == State.Thrown)
            {
                SetWorking(collided.GetComponent<O_Obstacle>());
            }
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
        Following,
        Thrown,
        Working
    }
}
