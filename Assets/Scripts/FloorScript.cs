using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour {

    Camera mainCam;

    bool player1_triggered = false;
    bool player2_triggered = false;

    float countdown = 0.8f;

    bool toggle = false;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;

	}

    // Update is called once per frame
    void Update() {
        if (gameObject.name.Contains("ThemeChange"))
        {
            if (transform.position.x < mainCam.transform.position.x - 48 && GameObject.Find("GameManager").GetComponent<GameManager>().GameMode == 1)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (transform.position.x < mainCam.transform.position.x - 24 && GameObject.Find("GameManager").GetComponent<GameManager>().GameMode == 1)
            {
                Destroy(this.gameObject);
            }
        }

        if (player1_triggered && player2_triggered)
        {
            if (this.gameObject.name.Contains("DoorClose"))
            {
                countdown -= 1 * Time.deltaTime;

                if (countdown <= 0)
                {
                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.tag == "Door")
                        {
                            child.GetComponent<BoxCollider2D>().enabled = true;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                        }
                    }
                    if (!toggle)
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>().SetGamemode2();
                        toggle = true;
                    }
                }
            }
            if (this.gameObject.name.Contains("DoorOpen"))
            {
                countdown -= 1 * Time.deltaTime;

                if (countdown <= 0)
                {
                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.tag == "DoorOpen")
                        {
                            child.GetComponent<BoxCollider2D>().enabled = false;
                            child.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                    if (!toggle)
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>().P1enable_input = false;
                        GameObject.Find("GameManager").GetComponent<GameManager>().P2enable_input = false;
                        GameObject.Find("GameManager").GetComponent<GameManager>().SetGamemode1();
                        GameObject.Find("FloorManager").GetComponent<FloorManager>().runReset = true;
                        toggle = true;
                    }
                }
            }
            if (this.gameObject.name.Contains("Drop"))
            {
                countdown -= 1 * Time.deltaTime;

                if (countdown <= 0)
                {
                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.tag == "Drop")
                        {
                            child.GetComponent<BoxCollider2D>().enabled = false;
                            child.GetComponent<SpriteRenderer>().enabled = false;
                        }

                        if (!toggle)
                        {
                            GameObject.Find("Player1").GetComponent<InstructionsControl>().Falling();
                            GameObject.Find("Player2").GetComponent<InstructionsControl>().Falling();
                            GameObject.Find("Player1").GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                            GameObject.Find("Player2").GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                            toggle = true;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.name.Contains("Door") || this.gameObject.name.Contains("Drop"))
        {
            if (collision.gameObject.name == "Player1")
            {
                player1_triggered = true;
            }
            if (collision.gameObject.name == "Player2")
            {
                player2_triggered = true;
            }
        }
        if (this.gameObject.name.Contains("ThemeChange_Climb"))
        {
            if (!toggle)
            {
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<InstructionsControl>().AddPoints(300);
                    toggle = true;
                }
            }
        }
        if(this.gameObject.name.Contains("ThemeChange_GameEnd"))
        {
            Camera.main.GetComponent<CameraBehaviour>().endGame = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().game_end = true;
        }
    }
}
