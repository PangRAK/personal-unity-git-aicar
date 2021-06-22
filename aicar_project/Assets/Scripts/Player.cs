using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int move_method;

    public float speed;
    public Vector2 speed_vec;
    public Vector3 originPos;
    public SpriteRenderer sprite;

    // 거리 센서


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.up) * 5.0f, Color.red, LayerMask.GetMask("Wall"));
        //UnityEngine.Debug.DrawRay(transform.position, transform.up * 5.0f, Color.red, LayerMask.GetMask("Wall"));
        //UnityEngine.Debug.DrawRay(transform.position, Vector2.up * 2.0f, Color.red, LayerMask.NameToLayer("Wall"));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 5.0f, LayerMask.GetMask("Wall"));
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 5.0f, LayerMask.GetMask("Wall"));

        //if(hit.collider != null)
        //{
        //    UnityEngine.Debug.Log(hit.collider.name);
        //}

        //if (Physics2D.Raycast(transform.position, Vector2.zero))
        if (hit.collider != null)
        {
            UnityEngine.Debug.Log(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer == 6)
            {
                UnityEngine.Debug.Log("Wall");
                return;
            }
            else
                UnityEngine.Debug.Log("!Wall");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // 두 물체간 물리적 충돌을 체크하고 싶다.
    {
        if (collision.gameObject.tag == "Wall")
        {
            move_method = -1;
            //Debug.Log("플레이어가 벽에 부딪혔습니다.");
            //Debug.Log(collision.contacts[0].point);
        }
    }
}