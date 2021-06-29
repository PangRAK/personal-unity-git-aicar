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

    // 인공지능 변수 세팅
    public float[,] w1 = new float[4,5];    // 입력층 -> 은닉층 가중치
    public float[,] w2 = new float[2,4];    // 은닉층 -> 출력층 가중치
    public float[] b1 = new float[4];       // 은닉층의 편향
    public float[] b2 = new float[2];       // 출력층의 편향

    // 순위 측정용 변수
    public int checkCount = 0;
    
    System.Random rand = new System.Random();   // 난수 생성용


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();

        // 첫번째 층 가중치, 편향 세팅
        for (int i = 0; i < w1.GetLength(0); i++)
        {
            for (int j = 0; j < w1.GetLength(1); j++)
            {
                w1[i, j] = ((float)rand.NextDouble() * 2) - 1.0f;   // -1.0 ~ 1.0 사이의 값을 가중치로 사용
                //UnityEngine.Debug.Log("w1[" + i + "," + j + "] = " + w1[i,j]);
            }
            b1[i] = 0f;
        }
        // 두번째 층 가중치, 편향 세팅
        for (int i = 0; i < w2.GetLength(0); i++)
        {
            for (int j = 0; j < w2.GetLength(1); j++)
            {   
                w2[i, j] = ((float)rand.NextDouble() * 2) - 1.0f;   // -1.0 ~ 1.0 사이의 값을 가중치로 사용
                //UnityEngine.Debug.Log("w2[" + i + "," + j + "] = " + w1[i, j]);
            }
            b2[i] = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckWall();
        AutoMove();
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
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(-1f, 1)) * 60.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(-0.3f, 1)) * 90.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(0, 1)) * 100.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(0.3f, 1)) * 90.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(1f,1)) * 60.0f, Color.red);

        // 센서를 생성
        hit1 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(-1, 1)), 100.0f, LayerMask.GetMask("Wall"));
        hit2 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(-0.3f, 1)), 100.0f, LayerMask.GetMask("Wall"));
        hit3 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(0, 1)), 100.0f, LayerMask.GetMask("Wall"));
        hit4 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(0.3f, 1)), 100.0f, LayerMask.GetMask("Wall"));
        hit5 = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(1, 1)), 100.0f, LayerMask.GetMask("Wall"));

        //UnityEngine.Debug.Log("h1 = " + hit1.distance);
        //UnityEngine.Debug.Log("h2 = " + hit2.distance);
        //UnityEngine.Debug.Log("h3 = " + hit3.distance);
        //UnityEngine.Debug.Log("h4 = " + hit4.distance);
        //UnityEngine.Debug.Log("h5 = " + hit5.distance);
    }

    void AutoMove()
    {
        if (move_method == 0)
        {
            float[] x = new float[5];
            x = new float[] { hit1.distance / 10, hit2.distance / 10, hit3.distance / 10, hit4.distance / 10, hit5.distance / 10 };

            // 센서에 거리가 감지되지 않았을 경우 최대거리 10f로 세팅
            // 정규화 실시
            x = Rak_Ai.normalization(x);

            float[] r = new float[2];
            r = Rak_Ai.predict(x, w1, w2, b1, b2);

            speed_vec.y = speed;
            transform.Translate(speed_vec);
            Left(r[0]);
            Right(r[1]);
        }
        else if (move_method == -1) // 게임 오버
        {
            sprite.color = Color.gray;
        }
    }

    void Left(float degree)
    {
        transform.Rotate(0, 0, Time.deltaTime * degree * 250, Space.Self);
    }

    void Right(float degree)
    {
        transform.Rotate(0, 0, -Time.deltaTime * degree * 250, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 10)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 11)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 12)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 13)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 14)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 15)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 16)
        {
            checkCount++;
        }
        if (collider.gameObject.layer == 17)
        {
            checkCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // 두 물체간 물리적 충돌을 체크하고 싶다.
    {
        if (collision.gameObject.tag == "Wall")
        {
            move_method = -1;
        }
    }
}