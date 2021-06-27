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
    //private Player playerScript1;
    //private Player playerScript2;
    //private Player playerScript3;
    //private Player playerScript4;
    //private Player playerScript5;
    //private Player playerScript6;
    //private Player playerScript7;
    //private Player playerScript8;
    //private Player playerScript9;
    //private Player playerScript10;
    private Player[] playerScript = new Player[10];
    public float[] playerScore = new float[10];
    public float[] playerRank = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    System.Random rand = new System.Random();   // 난수 생성용

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
        CheckRanking();
    }

    void CheckRanking()
    {
        //UnityEngine.Debug.Log("v1 = " + Vector2.Distance(player1.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v2 = " + Vector2.Distance(player2.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v3 = " + Vector2.Distance(player3.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v4 = " + Vector2.Distance(player4.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v5 = " + Vector2.Distance(player5.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v6 = " + Vector2.Distance(player6.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v7 = " + Vector2.Distance(player7.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v8 = " + Vector2.Distance(player8.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v9 = " + Vector2.Distance(player9.transform.position, checkPoint1.transform.position));
        //UnityEngine.Debug.Log("v10 = " + Vector2.Distance(player10.transform.position, checkPoint1.transform.position));

        //UnityEngine.Debug.Log("" + playerScript1.checkCount);
        //UnityEngine.Debug.Log("" + playerScript2.checkCount);
        //UnityEngine.Debug.Log("" + playerScript3.checkCount);
        //UnityEngine.Debug.Log("" + playerScript4.checkCount);
        //UnityEngine.Debug.Log("" + playerScript5.checkCount);
        //UnityEngine.Debug.Log("" + playerScript6.checkCount);
        //UnityEngine.Debug.Log("" + playerScript7.checkCount);
        //UnityEngine.Debug.Log("" + playerScript8.checkCount);
        //UnityEngine.Debug.Log("" + playerScript9.checkCount);
        //UnityEngine.Debug.Log("" + playerScript10.checkCount);

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
        float[,,] selct_w2 = new float[4, 3, 4];    // 은닉층 -> 출력층 가중치

        for (int i = 0; i < selection.Length; i++)  // 교차할 4명의의 유전자 저장
        {
            for(int j = 0; j < 10; j++)
            {
                if(selection[i] == playerRank[j])
                {
                    for (int x = 0; x < 4; x++)
                    {
                        for (int y = 0; y < 5; y++)
                        {
                            selct_w1[i, x, y] = playerScript[j].w1[x, y];
                        }
                    }
                }
            }
        }

        // 교차 연산 시작 균등 교차를 사용
        float[,,] child_w1 = new float[10, 4, 5];
        float[,,] child_w2 = new float[10, 3, 4];

        // 1번~5번 자식 1번,2번 부모를 이용하며 만듬
        float randFloat = 0f;
        for (int i = 0; i < 5; i++) // w1 가중치
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 5; y++)
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
                }
            }
        }
        for (int i = 0; i < 5; i++) // w2 가중치
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 4; y++)
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
                }
            }
        }

        // 6번~10번 자식 3번,4번 부모를 이용하며 만듬
        for (int i = 5; i < 10; i++) // w1 가중치
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 5; y++)
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
                }
            }
        }
        for (int i = 5; i < 10; i++) // w2 가중치
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 4; y++)
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
                }
            }
        }
    }

}
