using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using RAK_AI;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

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
    private GameObject[] player = new GameObject[28];
    private GameObject cameraObject;
    public GameObject checkPoint1;
    public GameObject checkPoint2;
    public GameObject checkPoint3;
    public GameObject checkPoint4;
    public GameObject checkPoint5;
    public GameObject checkPoint6;
    public GameObject checkPoint7;
    public GameObject checkPoint8;
    public GameObject checkPoint9;
    private Player[] playerScript = new Player[28]; // player 내부의 변수를 사용하기 위해 선언
    private Player cameraScript;
    private float[] playerScore = new float[28];    // 각 player의 점수들을 저장할 변수
    private int[] playerRank = new int[28];
    System.Random rand = new System.Random();       // 난수 생성용
    private float[,,] child_w1 = new float[28, 4, 5];
    private float[,,] child_w2 = new float[28, 2, 4];


    // Start is called before the first frame update
    void Start()
    {
        // 적절한 간격으로 자동차 10개를 생성함
        for (int i = 0; i < player.Length; i++)
        {
            player[i] = GameObject.Find("player");
            player[i] = Instantiate(player[i], transform.position, transform.rotation) as GameObject;
            Thread.Sleep(20);
        }

        for (int i = 0; i < player.Length; i++)
        {
            playerScript[i] = (Player)player[i].GetComponent(typeof(Player));
        }

        cameraObject = GameObject.Find("Main Camera");
        cameraScript = (Player)cameraObject.GetComponent(typeof(Player));
    }

    // Update is called once per frame
    void Update()
    {
        CheckRanking();         // 플레이어들의 순위(적합도)를 체크함
        if (checkAlive())       // 살아있는 플레이어가 있을 시
        {
            identifyPlayer();   // 상위권 플레이어를 식별하기위해 색을 바꿈
        }   
        else                    // 모든 플레이어가 죽었을 시, 유전알고리즘 연산을 하고 세팅후 재시작
        {
            geneticProcess();   // 체크된 순위(적합도)를 이용하여 선택,교차,변이를 수행
            resetPlayer();      // geneticProcess에서 산출된 결과를 이용하여 player들을 초기화하여 재시작
        }
    }

    void CheckRanking()
    {
        for (int i = 0; i < playerScore.Length; i++)
        {
            playerScore[i] = (playerScript[i].checkCount * 1000f) - CalcDist(player[i], playerScript[i].checkCount);
        }

        //for (int i = 0; i < playerScore.Length; i++)
        //    UnityEngine.Debug.Log("score_" + (i + 1) + " = " + playerScore[i]);


        // 플레이어 순위 구하기
        for (int i = 0; i < playerRank.Length; i++)
        {
            playerRank[i] = 1;
        }
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

        //for (int i = 0; i < playerRank.Length; i++)
        //{
        //    UnityEngine.Debug.Log("rank" + (i + 1) + " = " + playerRank[i]);
        //}
    }

    float CalcDist(GameObject x_player, int count)
    {
        float result = 0;
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
        int[] top4_selection = new int[4];          // 상위 4명
        int[] selection = new int[4];               // 골라진 4개
        selection = Rak_Ai.rankingSelection();      // 랭킹셀렉션 실행
        //for (int i = 0; i < selection.Length; i++)   // 1~4위까지는 엘리트로 뽑히므로 4를 더하여 selection의 최소값이 5가되게 만듬
        //{
        //    selection[i] += 4;
        //}
        //for (int i = 0; i < selection.Length; i++)
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

        // 교차 연산 시작 균등 교차를 사용--------------------------------------
        // 1번~4번 자식은 적합도가 가장 높은 4명의 부모 유전자를 그대로 물려받음
        for (int i = 0; i < 4; i++)         // w1 가중치
        {
            for (int j = 0; j < playerRank.GetLength(0); j++)
            {
                if (i + 1 == playerRank[j]) // 1~4등까지
                {
                    for (int x = 0; x < child_w1.GetLength(1); x++)
                    {
                        for (int y = 0; y < child_w1.GetLength(2); y++)
                        {
                            child_w1[i, x, y] = Rak_Ai.mutation(playerScript[j].w1[x, y]);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < 4; i++)         // w2 가중치
        {
            for (int j = 0; j < playerRank.GetLength(0); j++)
            {
                if (i + 1 == playerRank[j]) // 1~4등까지
                {
                    for (int x = 0; x < child_w2.GetLength(1); x++)
                    {
                        for (int y = 0; y < child_w2.GetLength(2); y++)
                        {
                            child_w2[i, x, y] = Rak_Ai.mutation(playerScript[j].w2[x, y]);
                        }
                    }
                }
            }
        }

        // 5번~8번 자식 1번,2번 부모를 이용하며 만듬
        float randFloat = 0f;
        for (int i = 4; i < 8; i++) // w1 가중치
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

                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]); // 변이 알고리즘
                }
            }
        }
        for (int i = 4; i < 8; i++) // w2 가중치
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

                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]); // 변이 알고리즘
                }
            }
        }

        // 9번~12번 자식 3번,4번 부모를 이용하며 만듬
        for (int i = 8; i < 12; i++) // w1 가중치
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

                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]); // 변이 알고리즘
                }
            }
        }
        for (int i = 8; i < 12; i++) // w2 가중치
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

                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]); // 변이 알고리즘
                }
            }
        }

        // 13번~16번 자식 1번,3번 부모를 이용하며 만듬
        for (int i = 12; i < 16; i++) // w1 가중치
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
                        child_w1[i, x, y] = selct_w1[2, x, y];
                    }

                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]); // 변이 알고리즘
                }
            }
        }
        for (int i = 12; i < 16; i++) // w2 가중치
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
                        child_w2[i, x, y] = selct_w2[2, x, y];
                    }

                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]); // 변이 알고리즘
                }
            }
        }

        // 17번~20번 자식 2번,4번 부모를 이용하며 만듬
        for (int i = 16; i < 20; i++) // w1 가중치
        {
            for (int x = 0; x < child_w1.GetLength(1); x++)
            {
                for (int y = 0; y < child_w1.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w1[i, x, y] = selct_w1[1, x, y];
                    }
                    else
                    {
                        child_w1[i, x, y] = selct_w1[3, x, y];
                    }

                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]); // 변이 알고리즘
                }
            }
        }
        for (int i = 16; i < 20; i++) // w2 가중치
        {
            for (int x = 0; x < child_w2.GetLength(1); x++)
            {
                for (int y = 0; y < child_w2.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w2[i, x, y] = selct_w2[1, x, y];
                    }
                    else
                    {
                        child_w2[i, x, y] = selct_w2[3, x, y];
                    }

                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]); // 변이 알고리즘
                }
            }
        }

        // 21번~24번 자식 1번,4번 부모를 이용하며 만듬
        for (int i = 20; i < 24; i++) // w1 가중치
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
                        child_w1[i, x, y] = selct_w1[3, x, y];
                    }

                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]); // 변이 알고리즘
                }
            }
        }
        for (int i = 20; i < 24; i++) // w2 가중치
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
                        child_w2[i, x, y] = selct_w2[3, x, y];
                    }

                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]); // 변이 알고리즘
                }
            }
        }

        // 25번~28번 자식 2번,3번 부모를 이용하며 만듬
        for (int i = 24; i < 28; i++) // w1 가중치
        {
            for (int x = 0; x < child_w1.GetLength(1); x++)
            {
                for (int y = 0; y < child_w1.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w1[i, x, y] = selct_w1[1, x, y];
                    }
                    else
                    {
                        child_w1[i, x, y] = selct_w1[2, x, y];
                    }

                    child_w1[i, x, y] = Rak_Ai.mutation(child_w1[i, x, y]); // 변이 알고리즘
                }
            }
        }
        for (int i = 24; i < 28; i++) // w2 가중치
        {
            for (int x = 0; x < child_w2.GetLength(1); x++)
            {
                for (int y = 0; y < child_w2.GetLength(2); y++)
                {
                    randFloat = (float)rand.NextDouble();
                    if (randFloat < 0.5f)
                    {
                        child_w2[i, x, y] = selct_w2[1, x, y];
                    }
                    else
                    {
                        child_w2[i, x, y] = selct_w2[2, x, y];
                    }

                    child_w2[i, x, y] = Rak_Ai.mutation(child_w2[i, x, y]); // 변이 알고리즘
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
            playerScript[i].transform.position = transform.position;    // 위치 이동
            playerScript[i].transform.rotation = transform.rotation;    // 정면 바라보게 회전
            playerScript[i].sprite.color = Color.yellow;                // 플레이어 색 복구
            playerScript[i].move_method = 0;                            // move_method 복구
            playerScript[i].checkCount = 0;                             // checkCount 복구
        }
    }

    void identifyPlayer()
    {
        Vector3 swap;
        for (int i = 0; i < playerRank.Length; i++)
        {
            if (playerRank[i] > 4)
            {
                playerScript[i].sprite.color = Color.yellow;
                swap = playerScript[i].transform.position;
                swap.z = 0;
                playerScript[i].transform.position = swap;
            }
            else if(playerRank[i] == 1)
            {
                playerScript[i].sprite.color = Color.green;
                swap = playerScript[i].transform.position;
                swap.z = -2;
                playerScript[i].transform.position = swap;
                cameraObject.transform.position = new Vector3(swap.x, swap.y, -100);
            }
            else if (playerRank[i] == 2)
            {
                playerScript[i].sprite.color = Color.red;
                swap = playerScript[i].transform.position;
                swap.z = -1;
                playerScript[i].transform.position = swap;
            }
            else if (playerRank[i] == 3)
            {
                playerScript[i].sprite.color = Color.red;
                swap = playerScript[i].transform.position;
                swap.z = -1;
                playerScript[i].transform.position = swap;
            }
            else if (playerRank[i] == 4)
            {
                playerScript[i].sprite.color = Color.red;
                swap = playerScript[i].transform.position;
                swap.z = -1;
                playerScript[i].transform.position = swap;
            }
        }
    }
}
