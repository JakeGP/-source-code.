using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(gameObject.name.Contains("Player1"))
            gameObject.GetComponent<SpriteRenderer>().sprite = manager.GetPlayer1Powerup();
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = manager.GetPlayer2Powerup();
    }
}