using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
    private Player playerScript1;
    private Player playerScript2;
    private Player playerScript3;
    private Player playerScript4;
    private Player playerScript5;
    private Player playerScript6;
    private Player playerScript7;
    private Player playerScript8;
    private Player playerScript9;
    private Player playerScript10;
    public float[] score = new float[10];

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
        playerScript1 = (Player)player1.GetComponent(typeof(Player));
        playerScript2 = (Player)player2.GetComponent(typeof(Player));
        playerScript3 = (Player)player3.GetComponent(typeof(Player));
        playerScript4 = (Player)player4.GetComponent(typeof(Player));
        playerScript5 = (Player)player5.GetComponent(typeof(Player));
        playerScript6 = (Player)player6.GetComponent(typeof(Player));
        playerScript7 = (Player)player7.GetComponent(typeof(Player));
        playerScript8 = (Player)player8.GetComponent(typeof(Player));
        playerScript9 = (Player)player9.GetComponent(typeof(Player));
        playerScript10 = (Player)player10.GetComponent(typeof(Player));

    }

    // Update is called once per frame
    void Update()
    {
        CheckRanking();
    }

    void CheckRanking()
    {
        UnityEngine.Debug.Log("v1 = " + Vector2.Distance(player1.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v2 = " + Vector2.Distance(player2.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v3 = " + Vector2.Distance(player3.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v4 = " + Vector2.Distance(player4.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v5 = " + Vector2.Distance(player5.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v6 = " + Vector2.Distance(player6.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v7 = " + Vector2.Distance(player7.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v8 = " + Vector2.Distance(player8.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v9 = " + Vector2.Distance(player9.transform.position, checkPoint1.transform.position));
        UnityEngine.Debug.Log("v10 = " + Vector2.Distance(player10.transform.position, checkPoint1.transform.position));
        
        UnityEngine.Debug.Log("" + playerScript1.checkCount);
        UnityEngine.Debug.Log("" + playerScript2.checkCount);
        UnityEngine.Debug.Log("" + playerScript3.checkCount);
        UnityEngine.Debug.Log("" + playerScript4.checkCount);
        UnityEngine.Debug.Log("" + playerScript5.checkCount);
        UnityEngine.Debug.Log("" + playerScript6.checkCount);
        UnityEngine.Debug.Log("" + playerScript7.checkCount);
        UnityEngine.Debug.Log("" + playerScript8.checkCount);
        UnityEngine.Debug.Log("" + playerScript9.checkCount);
        UnityEngine.Debug.Log("" + playerScript10.checkCount);
    }

}
