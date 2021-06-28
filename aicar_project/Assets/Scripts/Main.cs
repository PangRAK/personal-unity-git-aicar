using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using RAK_AI;
using System.Runtime.CompilerServices;

public class Main : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject player5;
    public GameObject player6;
    public GameObject player7;
    public GameObject player8;
    public GameObject player9;
    public GameObject player10;
    public GameObject checkPoint1;
    public GameObject checkPoint2;
    public GameObject checkPoint3;
    public GameObject checkPoint4;
    public GameObject checkPoint5;
    public GameObject checkPoint6;
    public GameObject checkPoint7;
    public GameObject checkPoint8;
    public GameObject checkPoint9;
    private Player[] playerScript = new Player[10]; // player ������ ������ ����ϱ� ���� ����
    public float[] playerScore = new float[10];     // �� player�� �������� ������ ����
    public float[] playerRank = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    System.Random rand = new System.Random();       // ���� ������
    private float[,,] child_w1 = new float[10, 4, 5];
    private float[,,] child_w2 = new float[10, 2, 4];


    // Start is called before the first frame update
    void Start()
    {
        // ������ �������� �ڵ��� 10���� ������
        player1 = Instantiate(player1, transform.position, transform.rotation) as GameObject; 
        Thread.Sleep(20);
        player2 = Instantiate(player2, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player3 = Instantiate(player3, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player4 = Instantiate(player4, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player5 = Instantiate(player5, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player6 = Instantiate(player6, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player7 = Instantiate(player7, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player8 = Instantiate(player8, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player9 = Instantiate(player9, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);
        player10 = Instantiate(player10, transform.position, transform.rotation) as GameObject;
        Thread.Sleep(20);

        // 10���� �ڵ��� ������ Script ������ ������
        playerScript[0] = (Player)player1.GetComponent(typeof(Player));
        playerScript[1] = (Player)player2.GetComponent(typeof(Player));
        playerScript[2] = (Player)player3.GetComponent(typeof(Player));
        playerScript[3] = (Player)player4.GetComponent(typeof(Player));
        playerScript[4] = (Player)player5.GetComponent(typeof(Player));
        playerScript[5] = (Player)player6.GetComponent(typeof(Player));
        playerScript[6] = (Player)player7.GetComponent(typeof(Player));
        playerScript[7] = (Player)player8.GetComponent(typeof(Player));
        playerScript[8] = (Player)player9.GetComponent(typeof(Player));
        playerScript[9] = (Player)player10.GetComponent(typeof(Player));

    }

    // Update is called once per frame
    void Update()
    {
        if (checkAlive()) { }   // ����ִ� �÷��̾ ���� ��
        else                    // ��� �÷��̾ �׾��� ��, �����˰��� ������ �ϰ� ������ �����
        {
            UnityEngine.Debug.Log("[ Start Fitness Check ]");
            CheckRanking();     // �÷��̾���� ����(���յ�)�� üũ��
            UnityEngine.Debug.Log("[ End Fitness Check ]");

            UnityEngine.Debug.Log("[ Start Genetic Process ]");
            geneticProcess();   // üũ�� ����(���յ�)�� �̿��Ͽ� ����,����,���̸� ����
            UnityEngine.Debug.Log("[ End Genetic Process ]");

            UnityEngine.Debug.Log("[ Start Reset Player ]");
            resetPlayer();      // geneticProcess���� ����� ����� �̿��Ͽ� player���� �ʱ�ȭ�Ͽ� �����
            UnityEngine.Debug.Log("[ End Reset Player ]");
            
        }
    }

    void CheckRanking()
    {
        playerScore[0] = (playerScript[0].checkCount * 100f) - CalcDist(player1, playerScript[0].checkCount);
        playerScore[1] = (playerScript[1].checkCount * 100f) - CalcDist(player2, playerScript[1].checkCount);
        playerScore[2] = (playerScript[2].checkCount * 100f) - CalcDist(player3, playerScript[2].checkCount);
        playerScore[3] = (playerScript[3].checkCount * 100f) - CalcDist(player4, playerScript[3].checkCount);
        playerScore[4] = (playerScript[4].checkCount * 100f) - CalcDist(player5, playerScript[4].checkCount);
        playerScore[5] = (playerScript[5].checkCount * 100f) - CalcDist(player6, playerScript[5].checkCount);
        playerScore[6] = (playerScript[6].checkCount * 100f) - CalcDist(player7, playerScript[6].checkCount);
        playerScore[7] = (playerScript[7].checkCount * 100f) - CalcDist(player8, playerScript[7].checkCount);
        playerScore[8] = (playerScript[8].checkCount * 100f) - CalcDist(player9, playerScript[8].checkCount);
        playerScore[9] = (playerScript[9].checkCount * 100f) - CalcDist(player10, playerScript[9].checkCount);

        //for (int i = 0; i < 10; i++)
        //    UnityEngine.Debug.Log("score_" + (i + 1) + " = " + playerScore[i]);


        // �÷��̾� ���� ���ϱ�
        for (int i = 0; i < playerScore.Length; i++)
            playerRank[i] = 1;
        for (int i = 0; i < playerScore.Length; i++)
        {
            for (int j = 0; j < playerScore.Length; j++)
            {
                //�� �� ���� ������ 1�� ����
                if (playerScore[i] < playerScore[j])
                {
                    playerRank[i]++;
                }
            }
        }

        for (int i = 0; i < playerScore.Length; i++)
        {
            UnityEngine.Debug.Log("rank" + (i + 1) + " = " + playerRank[i]);
        }
    }

    float CalcDist(GameObject x_player, int count)
    {
        float result = -1000;
        if(count == 0)
        {
            result =  Vector2.Distance(x_player.transform.position, checkPoint1.transform.position);
        }
        else if(count == 1)
        {
            result =  Vector2.Distance(x_player.transform.position, checkPoint2.transform.position);
        }
        else if (count == 2)
        {
            result = Vector2.Distance(x_player.transform.position, checkPoint3.transform.position);
        }
        else if (count == 3)
        {
            result = Vector2.Distance(x_player.transform.position, checkPoint4.transform.position);
        }
        else if (count == 4)
        {
            result = Vector2.Distance(x_player.transform.position, checkPoint5.transform.position);
        }
        else if (count == 5)
        {
            result = Vector2.Distance(x_player.transform.position, checkPoint6.transform.position);
        }
        else if (count == 6)
        {
            result = Vector2.Distance(x_player.transform.position, checkPoint7.transform.position);
        }
        else if (count == 7)
        {
            result = Vector2.Distance(x_player.transform.position, checkPoint8.transform.position);
        }
        else if (count == 8)
        {
            result = Vector2.Distance(x_player.transform.position, checkPoint9.transform.position);
        }
        return result;
    }

    void geneticProcess()
    {
        // ���� �˰���==============================================================
        int[] selection = new int[4];           // ����� 4��
        selection = Rak_Ai.rankingSelection();  // ��ŷ������ ����
        //for(int i = 0; i < selection.Length; i++)
        //{
        //    UnityEngine.Debug.Log("selection" + (i + 1) + " = " + selection[i]);
        //}

        // ���� �˰���==============================================================
        float[,,] selct_w1 = new float[4, 4, 5];    // �Է��� -> ������ ����ġ
        float[,,] selct_w2 = new float[4, 2, 4];    // ������ -> ����� ����ġ

        for (int i = 0; i < selection.GetLength(0); i++)    // ������ 4������ w1������ ����
        {
            for (int j = 0; j < playerRank.GetLength(0); j++)
            {
                if (selection[i] == playerRank[j])
                {
                    for (int x = 0; x < selct_w1.GetLength(1); x++)
                    {
                        for (int y = 0; y < selct_w1.GetLength(2); y++)
                        {
                            selct_w1[i, x, y] = playerScript[j].w1[x, y];
                        }
                    }
                }
            }
        }

        for (int i = 0; i < selection.GetLength(0); i++)    // ������ 4������ w2������ ����
        {
            for (int j = 0; j < playerRank.GetLength(0); j++)
            {
                if (selection[i] == playerRank[j])
                {
                    for (int x = 0; x < selct_w2.GetLength(1); x++)
                    {
                        for (int y = 0; y < selct_w2.GetLength(2); y++)
                        {
                            selct_w2[i, x, y] = playerScript[j].w2[x, y];
                        }
                    }
                }
            }
        }

        // ���� ���� ���� �յ� ������ ���
        // 1��~5�� �ڽ� 1��,2�� �θ� �̿��ϸ� ����
        float randFloat = 0f;
        for (int i = 0; i < 5; i++) // w1 ����ġ
        {
            for (int x = 0; x < child_w1.GetLength(1); x++)
            {
                for (int y = 0; y < child_w1.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w1[i, x, y] = selct_w1[0, x, y];
                    }
                    else
                    {
                        child_w1[i, x, y] = selct_w1[1, x, y];
                    }

                    // ���� �˰���===========================================
                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]);
                    //randFloat = (float)rand.NextDouble();
                    //if (randFloat < 0.01f)
                    //{
                    //    if (randFloat < 0.005f)
                    //    {
                    //        child_w1[i, x, y] += (1 - child_w1[i, x, y]) / 2;
                    //    }
                    //    else
                    //    {
                    //        child_w1[i, x, y] -= (-1 - child_w1[i, x, y]) / 2;
                    //    }
                    //}
                    // ========================================================
                }
            }
        }
        for (int i = 0; i < 5; i++) // w2 ����ġ
        {
            for (int x = 0; x < child_w2.GetLength(1); x++)
            {
                for (int y = 0; y < child_w2.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w2[i, x, y] = selct_w2[0, x, y];
                    }
                    else
                    {
                        child_w2[i, x, y] = selct_w2[1, x, y];
                    }

                    // ���� �˰���===========================================
                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]);
                    //randFloat = (float)rand.NextDouble();
                    //if (randFloat < 0.01f)
                    //{
                    //    if (randFloat < 0.005f)
                    //    {
                    //        child_w2[i, x, y] += (1 - child_w2[i, x, y]) / 2;
                    //    }
                    //    else
                    //    {
                    //        child_w2[i, x, y] -= (-1 - child_w2[i, x, y]) / 2;
                    //    }
                    //}
                    // ========================================================
                }
            }
        }

        // 6��~10�� �ڽ� 3��,4�� �θ� �̿��ϸ� ����
        for (int i = 5; i < 10; i++) // w1 ����ġ
        {
            for (int x = 0; x < child_w1.GetLength(1); x++)
            {
                for (int y = 0; y < child_w1.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w1[i, x, y] = selct_w1[2, x, y];
                    }
                    else
                    {
                        child_w1[i, x, y] = selct_w1[3, x, y];
                    }

                    // ���� �˰���===========================================
                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]);
                    //randFloat = (float)rand.NextDouble();
                    //if (randFloat < 0.01f)
                    //{
                    //    if(randFloat < 0.005f)
                    //    {
                    //        child_w1[i, x, y] += (1 - child_w1[i, x, y]) / 2;
                    //    }
                    //    else
                    //    {
                    //        child_w1[i, x, y] -= (-1 - child_w1[i, x, y]) / 2;
                    //    }
                    //}
                    // ========================================================
                }
            }
        }
        for (int i = 5; i < 10; i++) // w2 ����ġ
        {
            for (int x = 0; x < child_w2.GetLength(1); x++)
            {
                for (int y = 0; y < child_w2.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w2[i, x, y] = selct_w2[2, x, y];
                    }
                    else
                    {
                        child_w2[i, x, y] = selct_w2[3, x, y];
                    }

                    // ���� �˰���===========================================
                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]);
                    //randFloat = (float)rand.NextDouble();
                    //if (randFloat < 0.01f)
                    //{
                    //    if (randFloat < 0.005f)
                    //    {
                    //        child_w2[i, x, y] += (1 - child_w2[i, x, y]) / 2;
                    //    }
                    //    else
                    //    {
                    //        child_w2[i, x, y] -= (-1 - child_w2[i, x, y]) / 2;
                    //    }
                    //}
                    // ========================================================
                }
            }
        }

    }

    bool checkAlive()
    {
        for (int i = 0; i < playerScript.Length; i++)
        {
            if (playerScript[i].move_method != -1)
            {
                return true;
            }
        }
        return false;
    }

    void resetPlayer()
    {
        for (int i = 0; i < playerScript.Length; i++)
        {
            // �÷��̾� ����ġ ����
            for (int x = 0; x < child_w1.GetLength(1); x++)
            {
                for (int y = 0; y < child_w1.GetLength(2); y++)
                {
                    playerScript[i].w1[x, y] = child_w1[i, x, y];
                }
            }
            for (int x = 0; x < child_w2.GetLength(1); x++)
            {
                for (int y = 0; y < child_w2.GetLength(2); y++)
                {
                    playerScript[i].w2[x, y] = child_w2[i, x, y];
                }
            }

            // �÷��̾� ��ġ ���������� �̵�
            //playerScript[i].transform.position = Vector2.MoveTowards(playerScript[i].transform.position, transform.position);
            playerScript[i].transform.position = transform.position;    // ��ġ �̵�
            playerScript[i].transform.rotation = transform.rotation;    // ���� �ٶ󺸰� ȸ��
            playerScript[i].sprite.color = Color.yellow;                // �÷��̾� �� ����
            playerScript[i].move_method = 0;                            // move_method ����
        }
    }
}
