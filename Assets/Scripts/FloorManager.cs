using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorManager : MonoBehaviour
{
    GameManager manager;
    ThemeManager theme;
    public GameObject[] trackSection;
    public GameObject[] safeRoom;
    public GameObject[] trackPowerup;
    public GameObject countdownObject;
    public GameObject endGame;
    GameObject laneOne, laneTwo;
    GameObject newCountdown;

    AudioSource source;
    public AudioClip beepSound;

    float offset = 22.0f;
    float transition = 1.0f;
    public float countdown = 3.0f;

    private Camera mainCam;

    public bool respawn1 = false;
    public bool respawn2 = false;

    int GameMode = 0;
    int endGame_Count = 0;

 
    public bool runReset = false;
    bool timeFrozen = false;
    public bool spawnSafeRoom = false;
    bool safeRoomSpawned = false;

    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log(manager);
        if (GameObject.Find("ThemeManager"))
        {
            theme = GameObject.Find("ThemeManager").GetComponent<ThemeManager>();
        }

        source = GetComponent<AudioSource>();

        mainCam = Camera.main;
    }
    // Use this for initialization
    void Start()
    {
        GameMode = manager.GameMode;
    }

    // Update is called once per frame
    void Update()
    {
        GameModeBehaviour(GameMode);
    }

    void GameModeBehaviour(int gamemode)
    {
        switch (gamemode)
        {
            case 0:
                {
                    Countdown();
                    Tutorial();
                    break;
                }
            case 1:
                {
                    Countdown();
                    SpawnFloor();
                    break;
                }
        }
    }

    void Countdown()
    {
        //REMEMBER TO CHANGE
        if (manager.game_start)
        {
            if(countdown == 3)
            {
                source.PlayOneShot(beepSound, 0.8f);
                newCountdown = Instantiate(countdownObject, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 10.0f), Quaternion.identity) as GameObject;
                newCountdown.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
            if (countdown <= 0)
            {
                if(runReset)
                {
                    manager.P1enable_input = true;
                    manager.P2enable_input = true;
                    runReset = false;
                }
                manager.player1_speed = 10.0f;
                manager.player2_speed = 10.0f;
            }
            if(countdown <= -0.95)
            {
                Destroy(newCountdown);
            }
            if (countdown > -1.0f)
            {
                countdown -= 1 * Time.deltaTime;
            }
        }
    }

    void SpawnFloor()
    {
        GameObject[] lane1Tracks  = GameObject.FindGameObjectsWithTag("Lane1");
        int tracks1 = lane1Tracks.Length;
        GameObject[] lane2Tracks = GameObject.FindGameObjectsWithTag("Lane2");
        int tracks2 = lane2Tracks.Length;

        int totalTracks = tracks1 + tracks2;
        float xPos1 = 0, xPos2 = 0;
        float yPos1 = 0, yPos2 = 0;


        if (!spawnSafeRoom)
        {
            safeRoomSpawned = false;

            if (totalTracks != 0)
            {
                xPos1 = lane1Tracks[tracks1 - 1].transform.position.x + 24;
                xPos2 = lane2Tracks[tracks2 - 1].transform.position.x + 24;
                yPos1 = lane1Tracks[tracks1 - 1].transform.position.y;
                yPos2 = lane2Tracks[tracks2 - 1].transform.position.y;
            }

            if (totalTracks <= 0)
            {
                LevelStart();
            }
            if (respawn1)
            {
                RespawnFloor1(xPos1, yPos1);
            }
            else if (totalTracks > 0 && tracks1 < 2)
            {
                FloorRandomiser1(xPos1, yPos1);
            }
            if (respawn2)
            {
                RespawnFloor2(xPos2, yPos2);
            }
            else if (totalTracks > 0 && tracks2 < 2)
            {
                FloorRandomiser2(xPos2, yPos2);
            }
        }
        if (spawnSafeRoom && !safeRoomSpawned)
        {
            xPos2 = lane2Tracks[tracks2 - 1].transform.position.x + 24;
            yPos2 = lane2Tracks[tracks2 - 1].transform.position.y;
            SafeRoom(xPos2, yPos2);
        }

    }

    void LevelStart()
    {
        //Spawn Lane One
        Transform laneOneParent = GameObject.Find("Lane1").transform;
        GameObject newLaneOne = Instantiate(trackSection[0], laneOneParent) as GameObject;
        newLaneOne.tag = "Lane1";

        //Spawn Lane Two
        Transform laneTwoParent = GameObject.Find("Lane2").transform;
        GameObject newLaneTwo = Instantiate(trackSection[0], laneTwoParent) as GameObject;
        newLaneTwo.tag = "Lane2";
    }

    void FloorRandomiser1(float xPos, float yPos)
    {
        int randomNum = Mathf.RoundToInt(Random.Range(0, 5));

        switch (randomNum)
        {
            case 0:
                {
                    SpawnPowerup1(xPos, yPos);
                    break;
                }
            default:
                {
                    StandardRandom1(xPos, yPos);
                    break;
                }
        }
    }
    void FloorRandomiser2(float xPos, float yPos)
    {
        int randomNum = Mathf.RoundToInt(Random.Range(0, 5));

        switch (randomNum)
        {
            case 0:
                {
                    SpawnPowerup2(xPos, yPos);
                    break;
                }
            default:
                {
                    StandardRandom2(xPos, yPos);
                    break;
                }
        }
    }

    void RespawnFloor1(float xPos, float yPos)
    {
        //Spawn Lane One
        Transform laneOneParent = GameObject.Find("Lane1").transform;
        GameObject newLaneOne = Instantiate(trackSection[0], laneOneParent) as GameObject;
        newLaneOne.transform.position = new Vector3(xPos, yPos);
        newLaneOne.tag = "Lane1";

        respawn1 = false;
    }
    void RespawnFloor2(float xPos, float yPos)
    {
        //Spawn Lane Two
        Transform laneTwoParent = GameObject.Find("Lane2").transform;
        GameObject newLaneTwo = Instantiate(trackSection[0], laneTwoParent) as GameObject;
        newLaneTwo.transform.position = new Vector3(xPos, yPos);
        newLaneTwo.tag = "Lane2";

        respawn2 = false;
    }

    void StandardRandom1(float xPos, float yPos)
    {
        laneOne = trackSection[Mathf.RoundToInt(Random.RandomRange(1, trackSection.Length))];

        //Spawn Lane One
        Transform laneOneParent = GameObject.Find("Lane1").transform;
        GameObject newLaneOne = Instantiate(laneOne, laneOneParent) as GameObject;
        newLaneOne.transform.position = new Vector3(xPos, yPos);
        newLaneOne.tag = "Lane1";
    }
    void StandardRandom2(float xPos, float yPos)
    {
        laneTwo = trackSection[Mathf.RoundToInt(Random.RandomRange(1, trackSection.Length))];

        //Spawn Lane Two
        Transform laneTwoParent = GameObject.Find("Lane2").transform;
        GameObject newLaneTwo = Instantiate(laneTwo, laneTwoParent) as GameObject;
        newLaneTwo.transform.position = new Vector3(xPos, yPos);
        newLaneTwo.tag = "Lane2";
    }

    void SpawnPowerup1(float xPos, float yPos)
    {
        laneOne = trackPowerup[Mathf.RoundToInt(Random.RandomRange(0, trackPowerup.Length))];

        //Spawn Lane One
        Transform laneOneParent = GameObject.Find("Lane1").transform;
        GameObject newLaneOne = Instantiate(laneOne, laneOneParent) as GameObject;
        newLaneOne.transform.position = new Vector3(xPos, yPos);
        newLaneOne.tag = "Lane1";
    }
    void SpawnPowerup2(float xPos, float yPos)
    {
        laneTwo = trackPowerup[Mathf.RoundToInt(Random.RandomRange(0, trackPowerup.Length))];
        //Spawn Lane Two
        Transform laneTwoParent = GameObject.Find("Lane2").transform;
        GameObject newLaneTwo = Instantiate(laneTwo, laneTwoParent) as GameObject;
        newLaneTwo.transform.position = new Vector3(xPos, yPos);
        newLaneTwo.tag = "Lane2";
    }

    void SafeRoom(float xPos, float yPos)
    {
        if (manager.game_roundCount < 4)
        {
            laneTwo = safeRoom[Mathf.RoundToInt(Random.Range(0, safeRoom.Length))];

            //Spawn Lane Two
            Transform laneTwoParent = GameObject.Find("Lane2").transform;
            GameObject newLaneTwo = Instantiate(laneTwo, laneTwoParent) as GameObject;
            newLaneTwo.transform.position = new Vector3(xPos, yPos);

            manager.game_roundCount += 1;
            safeRoomSpawned = true;
        }
        else
        {
            if (endGame_Count < 1)
            {
                laneTwo = endGame;

                //Spawn Lane Two
                Transform laneTwoParent = GameObject.Find("Lane2").transform;
                GameObject newLaneTwo = Instantiate(laneTwo, laneTwoParent) as GameObject;
                newLaneTwo.transform.position = new Vector3(xPos, yPos);
                endGame_Count = 1;
            }
        }
    }


    void Tutorial()
    {
        if(timeFrozen)
        {
            switch(manager.tutorial_step)
            {
                case 1:
                    {
                        if (manager.player1_ready && manager.player2_ready)
                        {
                            GameObject.Find("Player1").GetComponent<InstructionsControl>().Jump();
                            GameObject.Find("Player2").GetComponent<InstructionsControl>().Jump();
                            UnfreezeTime();
                            manager.player1_ready = false;
                            manager.player2_ready = false;
                            manager.tutorial_step += 1;
                        }
                        break;
                    }
                case 2:
                    {
                        if (manager.player1_ready && manager.player2_ready)
                        {
                            GameObject.Find("Player1").GetComponent<InstructionsControl>().SlideDown();
                            GameObject.Find("Player2").GetComponent<InstructionsControl>().SlideDown();
                            UnfreezeTime();
                            manager.player1_ready = false;
                            manager.player2_ready = false;
                            manager.tutorial_step += 1;
                        }
                        break;
                    }
                case 3:
                    {
                        if (manager.player1_ready && manager.player2_ready)
                        {
                            GameObject.Find("Player1").GetComponent<InstructionsControl>().Dive();
                            GameObject.Find("Player2").GetComponent<InstructionsControl>().Dive();
                            UnfreezeTime();
                            manager.player1_ready = false;
                            manager.player2_ready = false;
                            manager.tutorial_step += 1;
                        }
                        break;
                    }
            }
        }
        if (manager.player1_ready && manager.player2_ready && (manager.tutorial_step == 4 || manager.tutorial_step == 1))
        {
            transition -= 1 * (Time.deltaTime / 3);
            GameObject.Find("Fade").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, (1.0f - transition));
            if (transition <= 0)
            {
                SceneManager.LoadScene("2 Player");
            }
        }
    }

    void FreezeTime()
    {
        timeFrozen = true;
        Time.timeScale = 0.005f;
    }
    void UnfreezeTime()
    {
        timeFrozen = false;
        Time.timeScale = 1.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FreezeTime();
        }
    }
}
