using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using RAK_AI;

public class Player : MonoBehaviour
{
    public int move_method;

    public float speed;
    public Vector2 speed_vec;
    public Vector3 originPos;
    public SpriteRenderer sprite;

    // 거리 센서
    public RaycastHit2D hit1;
    public RaycastHit2D hit2;
    public RaycastHit2D hit3;
    public RaycastHit2D hit4;
    public RaycastHit2D hit5;

    
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        AutoMove();
        CheckWall();
    }

    void Move()
    {
        if (move_method == 0)
        {
            //speed_vec.x = Input.GetAxis("Horizontal") * speed;
            speed_vec.y = Input.GetAxis("Vertical") * speed;

            transform.Translate(speed_vec);
            //GetComponent<Rigidbody2D>().velocity = speed_vec; // 벽과 곂치지 않음

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, Time.deltaTime * 200, Space.Self);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -Time.deltaTime * 200, Space.Self);
            }
        }
        else if (move_method == 1)
        {
            speed_vec = Vector2.zero;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                speed_vec.x += speed;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                speed_vec.x -= speed;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                speed_vec.y += speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                speed_vec.y -= speed;
            }

            GetComponent<Rigidbody2D>().velocity = speed_vec;
            //transform.Translate(speed_vec);
        }
        else if (move_method == 2)
        {
            speed_vec.x = Input.GetAxis("Horizontal") * UnityEngine.Random.Range(-0.1f, 0.1f);
            speed_vec.y = Input.GetAxis("Vertical") * UnityEngine.Random.Range(-0.1f, 0.1f);


            transform.Translate(speed_vec);
        }
        else if (move_method == 3)
        {
            speed_vec = Vector2.zero;

            speed_vec.x += UnityEngine.Random.Range(-0.5f, 0.5f);
            speed_vec.y += UnityEngine.Random.Range(-0.5f, 0.5f);

            transform.Translate(speed_vec);


        }
        else if (move_method == -1) // 게임 오버
        {
            sprite.color = Color.gray;
        }

        // 회전
        //Vector3 dir = transform.position - originPos;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //originPos = transform.position;
    }

    void CheckWall()
    {
        // 센서 범위를 그려줌
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(0, 1)) * 20.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(-0.2f, 1)) * 10.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(0.2f, 1)) * 10.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(-1, 1)) * 2.5f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(1,1)) * 2.5f, Color.red);

        // 센서를 생성
        hit1 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(0, 1)), 20.0f, LayerMask.GetMask("Wall"));
        hit2 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(-0.2f, 1)), 10.0f, LayerMask.GetMask("Wall"));
        hit3 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(0.2f, 1)), 10.0f, LayerMask.GetMask("Wall"));
        hit4 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(-1, 1)), 3.5f, LayerMask.GetMask("Wall"));
        hit5 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(1, 1)), 3.5f, LayerMask.GetMask("Wall"));

        if (hit1.collider != null)
        {
            if (hit1.collider.gameObject.layer == 6)
                UnityEngine.Debug.Log("hit1 = " + hit1.distance);
        }
        if (hit2.collider != null)
        {
            if (hit2.collider.gameObject.layer == 6)
                UnityEngine.Debug.Log("hit2 = " + hit2.distance);
        }
        if (hit3.collider != null)
        {
            if (hit3.collider.gameObject.layer == 6)
                UnityEngine.Debug.Log("hit3 = " + hit3.distance);
        }
        if (hit4.collider != null)
        {
            if (hit4.collider.gameObject.layer == 6)
                UnityEngine.Debug.Log("hit4 = " + hit4.distance);
        }
        if (hit5.collider != null)
        {
            if (hit5.collider.gameObject.layer == 6)
                UnityEngine.Debug.Log("hit5 = " + hit5.distance);
        }

    }

    void AutoMove()
    {
        
        if (hit1.distance == 0)
            hit1.distance = 9999f;
        if (hit2.distance == 0)
            hit2.distance = 9999f;
        if (hit3.distance == 0)
            hit3.distance = 9999f;
        if (hit4.distance == 0)
            hit4.distance = 9999f;
        if (hit5.distance == 0)
            hit5.distance = 9999f;

        int random = UnityEngine.Random.Range(0, 10);

        speed_vec.y = speed;
        transform.Translate(speed_vec);


        if (hit4.distance > hit5.distance)
        {
            Left();
        }
        else if(hit4.distance < hit5.distance)
        {
            Right();
        }
    }

    void Left()
    {
        transform.Rotate(0, 0, Time.deltaTime * 200, Space.Self);
    }

    void Right()
    {
        transform.Rotate(0, 0, -Time.deltaTime * 200, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision) // 두 물체간 물리적 충돌을 체크하고 싶다.
    {
        if (collision.gameObject.tag == "Wall")
        {
            move_method = -1;
        }
    }
}