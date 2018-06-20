using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreValidator_Endgame : MonoBehaviour {

    public GameObject[] Digits;
    public GameObject sparkle;
    public Sprite[] Sprites;
    public Transform player1, player2;

    Score score;
    float player1_score = 0, player2_score = 0;
    float p1_ypos = 0, p2_ypos = 0;
    float p1_target, p2_target;
    float p1_start, p2_start;
    Vector3 p1_new, p2_new;

    float distance = 4.0f;
    float sparkletimer = 1.5f;

    // Use this for initialization
    void Start () {
        score = GameObject.Find("ScoreManager").GetComponent<Score>();
        GetPercentage();
        player1 = GameObject.Find("Player1_Podium").transform;
        player2 = GameObject.Find("Player2_Podium").transform;
        p1_start = player1.position.y;
        p2_start = player2.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateScores();

	}

    void UpdateScores()
    {
        if (gameObject.name.Contains("Player1"))
        {
            if (player1_score < score.s_P1score)
            {
                player1_score += 1 * (Time.deltaTime * (score.s_P1score / 5.0f));
                p1_ypos += 1 * (Time.deltaTime * (p1_target / 5.0f));
                player1.position = new Vector3(player1.position.x, p1_start + p1_ypos);
            }
            else
            {
                Winner();
            }
            UpdateScore(Mathf.RoundToInt(player1_score));
        }
        else
        {
            if (player2_score < score.s_P2score)
            {
                player2_score += 1 * (Time.deltaTime * (score.s_P2score / 5.0f));
                p2_ypos += 1 * (Time.deltaTime * (p2_target / 5.0f));
                player2.position = new Vector3(player2.position.x, p2_start + p2_ypos);
            }
            UpdateScore(Mathf.RoundToInt(player2_score));
        }

    }

    public void Winner()
    {
        sparkletimer -= 1 * Time.deltaTime;
        if (sparkletimer <= 0)
        {
            GameObject.Find("ReturnToMenu").GetComponent<SpriteRenderer>().enabled = true;
            if (score.s_P1score > score.s_P2score)
            {
                Instantiate(sparkle, new Vector3(GameObject.Find("Player1").transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f), GameObject.Find("Player1").transform.position.y + UnityEngine.Random.Range(-2.0f, 2.0f)), Quaternion.identity);
            }
            else
            {
                Instantiate(sparkle, new Vector3(GameObject.Find("Player2").transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f), GameObject.Find("Player2").transform.position.y + UnityEngine.Random.Range(-2.0f, 2.0f)), Quaternion.identity);
            }
            sparkletimer = 0.5f;
        }
    }

    public void GetPercentage()
    {
        if(score.s_P1score > score.s_P2score)
        {
            float percentage = score.s_P2score / score.s_P1score;
            p1_target = 4.0f;
            p2_target = percentage * 4.0f;
        }
        else
        {
            float percentage = score.s_P1score / score.s_P2score;
            p2_target = 4.0f;
            p1_target = percentage * 4.0f;
        }
    }

    public void UpdateScore(int score)
    {
        int[] digits = GetIntArray(score);

        for(int i = 0; i < digits.Length; i++)
        {
            Digits[i].GetComponent<SpriteRenderer>().sprite = Sprites[digits[i]];
        }
    }

    int[] GetIntArray(int num)
    {
        var numbers = new Stack<int>();

        for (; num > 0; num /= 10)
            numbers.Push(num % 10);

        int[] dingo = numbers.ToArray();
        Array.Reverse(dingo);
        return dingo;
    }
}
