using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    //Score Variables
    public GameObject m_p1Score, m_p2Score;

    GameManager manager;

    public float s_P1score = 0;
    public int s_P1multuiplier = 1;
           
    public float s_P2score = 0;
    public int s_P2multuiplier = 1;

    public float s_p1_addedScore = 0;
    public float s_p2_addedScore = 0;

    public bool gameStart;
    bool P1recentlyUpdated = false, P2recentlyUpdated = false;

    float P1coolDown = 0.4f, P2coolDown = 0.4f;

    void Start ()
    {
        DontDestroyOnLoad(this);
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        UpdateScores();
        AddScore();
    }

    void AddScore()
    {
        if(s_p1_addedScore > 0)
        {
            s_P1score += 1 * (Time.deltaTime * 160);
            s_p1_addedScore -= 1 * (Time.deltaTime * 160);
        }
        if (s_p2_addedScore > 0)
        {
            s_P2score += 1 * (Time.deltaTime * 160);
            s_p2_addedScore -= 1 * (Time.deltaTime * 160);
        }
    }

    void UpdateScores()
    {
        if (manager.GameMode == 1)
        {
            if (P1recentlyUpdated)
            {
                P1coolDown -= 1 * Time.deltaTime;
                if (P1coolDown <= 0)
                {
                    P1recentlyUpdated = false;
                    GameObject.Find("P1Multiplier").GetComponent<Animator>().SetBool("Multiplier", false);
                }
            }
            if (P2recentlyUpdated)
            {
                P2coolDown -= 1 * Time.deltaTime;
                if (P2coolDown <= 0)
                {
                    P2recentlyUpdated = false;
                    GameObject.Find("P2Multiplier").GetComponent<Animator>().SetBool("Multiplier", false);
                }
            }
        }

        if (manager.player1_speed == 10.0f)
        {
            s_P1score += (1 * s_P1multuiplier) * (Time.deltaTime * 4);
            s_P2score += (1 * s_P2multuiplier) * (Time.deltaTime * 4);
        }
    } 

    public void UpdateP1Multiplier (int newValue, bool reset)
    {
        if (!P1recentlyUpdated && manager.GameMode == 1)
        {
            GameObject.Find("P1Multiplier").GetComponent<Animator>().SetBool("Multiplier", true);
            if (!reset)
            {
                s_P1multuiplier += newValue;
            }
            else
            {
                s_P1multuiplier = 1;
            }
            P1recentlyUpdated = true;
            P1coolDown = 0.4f;
        }
    }
    
    public void UpdateP2Multiplier(int newValue, bool reset)
    {
        if (!P2recentlyUpdated && manager.GameMode == 1)
        {
            GameObject.Find("P2Multiplier").GetComponent<Animator>().SetBool("Multiplier", true);
            if (!reset)
            {
                s_P2multuiplier += newValue;
            }
            else
            {
                s_P2multuiplier = 1;
            }
            P2recentlyUpdated = true;
            P2coolDown = 0.4f;
        }
    }
}

