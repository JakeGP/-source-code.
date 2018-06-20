using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreValidator : MonoBehaviour {

    public GameObject[] Digits;
    public GameObject[] Multiplier;
    public GameObject sparkle;
    public Sprite[] Sprites;

    Score score;

    // Use this for initialization
    void Start () {
        score = GameObject.Find("ScoreManager").GetComponent<Score>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateScores();
	}

    void UpdateScores()
    {
        if (gameObject.name.Contains("Player1"))
        {
            UpdateScore(Mathf.RoundToInt(score.s_P1score));
            UpdateMultiplier(score.s_P1multuiplier);

        }
        else
        {
            UpdateScore(Mathf.RoundToInt(score.s_P2score));
            UpdateMultiplier(score.s_P2multuiplier);
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

    public void UpdateMultiplier(int multiplier)
    {
        int[] digits = GetIntArray(multiplier);

        if (multiplier > 0)
        {
            for (int i = 0; i < digits.Length; i++)
            {
                Multiplier[i].GetComponent<SpriteRenderer>().sprite = Sprites[digits[i]];
            }
        }
        else
        {
            Multiplier[1].GetComponent<SpriteRenderer>().sprite = Sprites[digits[0]];
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
