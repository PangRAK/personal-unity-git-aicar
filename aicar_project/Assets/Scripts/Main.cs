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
    private Player[] playerScript = new Player[10]; // player 내부의 변수를 사용하기 위해 선언
    public float[] playerScore = new float[10];     // 각 player의 점수들을 저장할 변수
    public float[] playerRank = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    System.Random rand = new System.Random();       // 난수 생성용
    private float[,,] child_w1 = new float[10, 4, 5];
    private float[,,] child_w2 = new float[10, 2, 4];


    // Start is called before the first frame update
    void Start()
    {
        // 적절한 간격으로 자동차 10개를 생성함
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

        // 10개의 자동차 각각의 Script 변수를 세팅함
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
        if (checkAlive()) { }   // 살아있는 플레이어가 있을 시
        else                    // 모든 플레이어가 죽었을 시, 유전알고리즘 연산을 하고 세팅후 재시작
        {
            UnityEngine.Debug.Log("[ Start Fitness Check ]");
            CheckRanking();     // 플레이어들의 순위(적합도)를 체크함
            UnityEngine.Debug.Log("[ End Fitness Check ]");

            UnityEngine.Debug.Log("[ Start Genetic Process ]");
            geneticProcess();   // 체크된 순위(적합도)를 이용하여 선택,교차,변이를 수행
            UnityEngine.Debug.Log("[ End Genetic Process ]");

            UnityEngine.Debug.Log("[ Start Reset Player ]");
            resetPlayer();      // geneticProcess에서 산출된 결과를 이용하여 player들을 초기화하여 재시작
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


        // 플레이어 순위 구하기
        for (int i = 0; i < playerScore.Length; i++)
            playerRank[i] = 1;
        for (int i = 0; i < playerScore.Length; i++)
        {
            for (int j = 0; j < playerScore.Length; j++)
            {
                //비교 후 값이 작으면 1씩 증가
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
        // 선택 알고리즘==============================================================
        int[] selection = new int[4];           // 골라진 4개
        selection = Rak_Ai.rankingSelection();  // 랭킹셀렉션 실행
        //for(int i = 0; i < selection.Length; i++)
        //{
        //    UnityEngine.Debug.Log("selection" + (i + 1) + " = " + selection[i]);
        //}

        // 교차 알고리즘==============================================================
        float[,,] selct_w1 = new float[4, 4, 5];    // 입력층 -> 은닉층 가중치
        float[,,] selct_w2 = new float[4, 2, 4];    // 은닉층 -> 출력층 가중치

        for (int i = 0; i < selection.GetLength(0); i++)    // 교차할 4명의의 w1유전자 저장
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

        for (int i = 0; i < selection.GetLength(0); i++)    // 교차할 4명의의 w2유전자 저장
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

        // 교차 연산 시작 균등 교차를 사용
        // 1번~5번 자식 1번,2번 부모를 이용하며 만듬
        float randFloat = 0f;
        for (int i = 0; i < 5; i++) // w1 가중치
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

                    // 변이 알고리즘===========================================
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
        for (int i = 0; i < 5; i++) // w2 가중치
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

                    // 변이 알고리즘===========================================
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

        // 6번~10번 자식 3번,4번 부모를 이용하며 만듬
        for (int i = 5; i < 10; i++) // w1 가중치
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

                    // 변이 알고리즘===========================================
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
        for (int i = 5; i < 10; i++) // w2 가중치
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

                    // 변이 알고리즘===========================================
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
            // 플레이어 가중치 세팅
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

            // 플레이어 위치 시작점으로 이동
            //playerScript[i].transform.position = Vector2.MoveTowards(playerScript[i].transform.position, transform.position);
            playerScript[i].transform.position = transform.position;    // 위치 이동
            playerScript[i].transform.rotation = transform.rotation;    // 정면 바라보게 회전
            playerScript[i].sprite.color = Color.yellow;                // 플레이어 색 복구
            playerScript[i].move_method = 0;                            // move_method 복구
        }
    }
}
