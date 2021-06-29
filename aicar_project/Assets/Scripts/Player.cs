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

    // �Ÿ� ����
    public RaycastHit2D hit1;
    public RaycastHit2D hit2;
    public RaycastHit2D hit3;
    public RaycastHit2D hit4;
    public RaycastHit2D hit5;

    // �ΰ����� ���� ����
    public float[,] w1 = new float[4,5];    // �Է��� -> ������ ����ġ
    public float[,] w2 = new float[2,4];    // ������ -> ����� ����ġ
    public float[] b1 = new float[4];       // �������� ����
    public float[] b2 = new float[2];       // ������� ����

    // ���� ������ ����
    public int checkCount = 0;
    
    System.Random rand = new System.Random();   // ���� ������


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();

        // ù��° �� ����ġ, ���� ����
        for (int i = 0; i < w1.GetLength(0); i++)
        {
            for (int j = 0; j < w1.GetLength(1); j++)
            {
                w1[i, j] = ((float)rand.NextDouble() * 2) - 1.0f;   // -1.0 ~ 1.0 ������ ���� ����ġ�� ���
                //UnityEngine.Debug.Log("w1[" + i + "," + j + "] = " + w1[i,j]);
            }
            b1[i] = 0f;
        }
        // �ι�° �� ����ġ, ���� ����
        for (int i = 0; i < w2.GetLength(0); i++)
        {
            for (int j = 0; j < w2.GetLength(1); j++)
            {   
                w2[i, j] = ((float)rand.NextDouble() * 2) - 1.0f;   // -1.0 ~ 1.0 ������ ���� ����ġ�� ���
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
            //GetComponent<Rigidbody2D>().velocity = speed_vec; // ���� ��ġ�� ����

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, Time.deltaTime * 200, Space.Self);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -Time.deltaTime * 200, Space.Self);
            }
        }
        else if (move_method == -1) // ���� ����
        {
            sprite.color = Color.gray;
        }

        // ȸ��
        //Vector3 dir = transform.position - originPos;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //originPos = transform.position;
    }

    void CheckWall()
    {
        // ���� ������ �׷���
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(-1f, 1)) * 60.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(-0.3f, 1)) * 90.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(0, 1)) * 100.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(0.3f, 1)) * 90.0f, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector2(1f,1)) * 60.0f, Color.red);

        // ������ ����
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

            // ������ �Ÿ��� �������� �ʾ��� ��� �ִ�Ÿ� 10f�� ����
            // ����ȭ �ǽ�
            x = Rak_Ai.normalization(x);

            float[] r = new float[2];
            r = Rak_Ai.predict(x, w1, w2, b1, b2);

            speed_vec.y = speed;
            transform.Translate(speed_vec);
            Left(r[0]);
            Right(r[1]);
        }
        else if (move_method == -1) // ���� ����
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

    private void OnCollisionEnter2D(Collision2D collision) // �� ��ü�� ������ �浹�� üũ�ϰ� �ʹ�.
    {
        if (collision.gameObject.tag == "Wall")
        {
            move_method = -1;
        }
    }
}