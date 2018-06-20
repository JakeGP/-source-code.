using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    GameObject player1, player2;
    GameManager manager;
    public float distance, currentdistance, startGameDistance;
    float speed;
    public bool endGame = false;
    public bool reset = false;
    Vector3 middle;

	// Use this for initialization
	void Start () {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        distance = (transform.position.x * -1) - (player1.transform.position.x);
        middle = (player1.transform.position + player2.transform.position) * 0.5f;
    }

    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update () {
        Reset();
        if ((manager.GameMode == 1 || manager.GameMode == 0) && !endGame)
        {
            GetPlayerPosition();
        }
        if (manager.GameMode == 2)
        {
            transform.position = SetCameraPos();
            SetCameraSize();
        }
    }

    private void Reset()
    { 
        if(!reset)
        {
            middle = (player1.transform.position + player2.transform.position) * 0.5f;
            reset = true;
        }
    }

    void GetPlayerPosition()
    {
        currentdistance = (transform.position.x * -1) - (player1.transform.position.x);
        startGameDistance = (transform.position.x * -1) - ((player2.transform.position.x + 7) * -1);

        if (!manager.game_start)
        {
            speed = ((transform.position.x * -1) - (player1.transform.position.x)) / 6;
        }
        else
        {
            speed = 10.0f;
        }

        if (manager.GameMode == 1)
        {
            if (player1.transform.position.x < player2.transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player1.transform.position.x + 7, middle.y, -16.5f), speed * Time.deltaTime);
            }
            else if (player1.transform.position.x > player2.transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player2.transform.position.x + 7, middle.y, -16.5f), speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player2.transform.position.x + 7, middle.y, -16.5f), speed * Time.deltaTime);
            }
        }
        if(manager.GameMode == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player2.transform.position.x + 7, transform.position.y, -16.5f), speed * Time.deltaTime);
        }


        if (startGameDistance <= 0)
        {
            manager.game_start = true;
        }
    }

    Vector3 SetCameraPos()
    {
        middle = (player1.transform.position + player2.transform.position) * 0.5f;

        return new Vector3(
            middle.x,
            middle.y,
            this.transform.position.z
        );
    }

    void SetCameraSize()
    {
        //horizontal size is based on actual screen ratio
        float minSizeX = 5 * Screen.width / Screen.height;

        //multiplying by 0.5, because the ortographicSize is actually half the height
        float width = Mathf.Abs(player1.transform.position.x - player2.transform.position.x) * 0.6f;
        float height = Mathf.Abs(player1.transform.position.y - player2.transform.position.y) * 0.8f;

        //computing the size
        float camSizeX = Mathf.Max(width, minSizeX);
        Camera.main.orthographicSize = Mathf.Max(height,
            camSizeX * Screen.height / Screen.width, 5);
    }
}
