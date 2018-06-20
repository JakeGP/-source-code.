using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    GameManager manager;

    AudioSource source;
    public AudioClip beepSound;
    public AudioClip readyUpSound;
    public AudioClip menuClick;

    public GameObject countdownObject;
    public Sprite tutorialYes, tutorialNo; 
    GameObject player1, player2, newCountdown;

    bool player1_ready = false, player2_ready = false;
    bool tutorial = true;
    bool canChange = true;
    bool clicktoggle = false;
    float countdown = 5.0f;
    float transition = 1.0f;

	void Start ()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        source = GetComponent<AudioSource>();

        if (GameObject.Find("ScoreManager"))
            Destroy(GameObject.Find("ScoreManager"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        PlayerReady();

        if(manager.player1_ready && manager.player2_ready == true)
        {
            Countdown();
        }

        if (Input.GetAxis("Joystick1_Y") == 1 && canChange)
        {
            source.PlayOneShot(menuClick, 0.6f);
            if (GameObject.Find("Tutorial").GetComponent<SpriteRenderer>().sprite.name == "Tutorial_Yes")
            {
                GameObject.Find("Tutorial").GetComponent<SpriteRenderer>().sprite = tutorialNo;
                tutorial = false;
            }
            else
            {
                GameObject.Find("Tutorial").GetComponent<SpriteRenderer>().sprite = tutorialYes;
                tutorial = true;
            }
            canChange = false;
        }
        if (Input.GetAxis("Joystick1_Y") == 0 && !canChange)
        {
            canChange = true;
        }

        if ((Input.GetAxis("Joystick1_X") == 1 && !clicktoggle) || (Input.GetAxis("Joystick2_X") == 1) && !clicktoggle)
        {
            source.PlayOneShot(menuClick, 0.6f);
            SceneManager.LoadSceneAsync("Controls Scene");
            clicktoggle = true;
        }

        if((Input.GetAxis("Joystick1_B") == 1 && !clicktoggle) || (Input.GetAxis("Joystick2_B") == 1) && !clicktoggle)
        {
            source.PlayOneShot(menuClick, 0.6f);
            clicktoggle = true;
            Application.Quit();
        }
    }

    void Countdown()
    {
        //REMEMBER TO CHANGE
        if (manager.player1_ready && manager.player2_ready)
        {
            if (countdown == 5)
            {
                source.PlayOneShot(beepSound, 0.8f);
                newCountdown = Instantiate(countdownObject, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 3.5f, 10.0f), Quaternion.identity) as GameObject;
                newCountdown.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
            if (countdown <= 0)
            {
                SceneChange();
            }
            if (countdown <= -0.95)
            {
                Destroy(newCountdown);
            }
            if (countdown > -1.0f)
            {
                countdown -= 1 * Time.deltaTime;              
            }
        }
    }

    void PlayerReady()
    {
        if(manager.player1_ready)
        {
            if (!player1_ready)
            {
                manager.player1_physics = true;
                player1.GetComponent<InstructionsControl>().Falling();
                source.PlayOneShot(readyUpSound, 0.5f);
                player1_ready = true;
            }
        }
        if (Input.GetAxis("Joystick1_Back") > 0.1f)
        {
            //this.GetComponent<InstructionsControl>().Stop();
        }

        if (manager.player2_ready)
        {
            if (!player2_ready)
            {
                manager.player2_physics = true;
                player2.GetComponent<InstructionsControl>().Falling();
                source.PlayOneShot(readyUpSound, 0.5f);
                player2_ready = true;
            }
        }
        if (Input.GetAxis("Joystick2_Back") > 0.1f)
        {
            //this.GetComponent<InstructionsControl>().Stop();
        }
    }

    void SceneChange()
    {
        if (tutorial)
        {
            transition -= 1 * (Time.deltaTime / 2);
            GameObject.Find("Fade").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, (1.0f - transition));
            if (transition <= 0)
            {
                SceneManager.LoadScene("Tutorial");
            }
        }
        else
        {
            transition -= 1 * (Time.deltaTime / 2);
            GameObject.Find("Fade").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, (1.0f - transition));
            if (transition <= 0)
            {
                SceneManager.LoadScene("2 Player");
            }
        }
    }

}
