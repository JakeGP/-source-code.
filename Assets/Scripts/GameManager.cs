using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float player1_speed = 0.0f, player2_speed = 0.0f;
    public float player_jumpHeight = 600.0f;
    public float player_respawn = 6.0f;

    public bool player1_physics = false, player2_physics = false;
    public bool player1_hitPowerup = false, player2_hitPowerup = false;
    public bool player1_hasPowerup = false, player2_hasPowerup = false;
    public bool player1_shielded = false, player2_shielded = false;
    public bool player_runOverride = false;
    public bool player_jumpOverride = false;

    public bool player1_ready = false;
    public bool player2_ready = false;

    public float game_roundTime = 60.0f;
    public float transition = 1.0f;

    public int tutorial_step = 1;
    public int game_roundCount = 0;

    public bool game_start = false;
    public bool game_end = false;
    public bool game_inEnd = false;
    public bool paused = false;
    public bool P1enable_input = true, P2enable_input = true;

    public int GameMode = 0;
    public int pauseCount = 0;

    public string game_theme = "Jungle";
    public int player1_powerup = 0, player2_powerup = 0;
    public string[] powerups = { "None", "ControlJam", "ControlMuddle", "MultiplyIncrease", "Shield" }; 

    GameObject Background, Midground, Foreground, UI;
    public GameObject pause;
    public Sprite[] powerupSprites;

    void Awake() {
        SetValuesBasedOnScene();
    }

    // Use this for initialization
    void Start() {
        if (GameObject.Find("Background"))
            Background = GameObject.Find("Background");
        if (GameObject.Find("Midground"))
            Midground = GameObject.Find("Midground");
        if (GameObject.Find("Foreground"))
            Foreground = GameObject.Find("Foreground");
        if (GameObject.Find("UI"))
            UI = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update() {
        NewRound();
        CheckShield();
        GameEnd();
        CheckPause();
    }

    public void DisableInput(int playerNum, bool disable)
    {
        if(playerNum == 1)
        {
            P1enable_input = disable;
        }
        else
        {
            P2enable_input = disable;
        }
    }

    void CheckPause()
    {
        if (paused && pauseCount == 0)
        {
            Time.timeScale = 0.0f;
            GameObject _pause = Instantiate(pause, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 17), Quaternion.identity, Camera.main.transform) as GameObject;
            _pause.name = "Pause";
            pauseCount = 1;
        }
        else if(!paused && pauseCount == 1)
        {
            if (GameObject.Find("Pause"))
            {
                Destroy(GameObject.Find("Pause"));
                Time.timeScale = 1.0f;
                pauseCount = 0;
            }
        }
    }

    void CheckShield()
    {
        if(player1_shielded)
        {
            P1enable_input = true;
        }
        if (player2_shielded)
        {
            P2enable_input = true;
        }
    }

    public void SetValuesBasedOnScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "2 Player":
                {
                    player1_speed = 0.0f;
                    player2_speed = 0.0f;
                    player_jumpHeight = 600.0f;
                    player_respawn = 6.0f;
                    GameMode = 1;
                    player1_ready = false;
                    player2_ready = false;
                    player1_physics = true;
                    player2_physics = true;
                    player_runOverride = false;
                    break;
                }
            case "Tutorial":
                {
                    player1_speed = 0.0f;
                    player2_speed = 0.0f;
                    player_jumpHeight = 600.0f;
                    player_respawn = 6.0f;
                    GameMode = 0;
                    player1_ready = false;
                    player2_ready = false;
                    player1_physics = true;
                    player2_physics = true;
                    player_runOverride = false;
                    break;
                }
            case "MainMenu":
                {
                    GameMode = 1;
                    paused = false;
                    Time.timeScale = 1.0f;
                    player1_physics = false;
                    player2_physics = false;
                    player_runOverride = false;
                    break;
                }
            case "Controls Scene":
                {
                    GameMode = 1;
                    player_jumpHeight = 600.0f;
                    player1_physics = true;
                    player2_physics = true;
                    player_runOverride = true;
                    break;
                }
            case "Gameover":
                {
                    GameMode = 3;
                    game_inEnd = true;
                    player_jumpHeight = 0.0f;
                    player1_physics = true;
                    player2_physics = true;
                    player_runOverride = false;
                    player1_ready = false;
                    player2_ready = false;
                    break;
                }
        }
    }  

    void GameEnd()
    {
        if(game_end)
        {
            transition -= 1 * (Time.deltaTime / 2);
            GameObject.Find("Fade").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, (1.0f - transition));
            if (transition <= 0)
            {
                SceneManager.LoadScene("Gameover");
            }
        }
        if(game_inEnd)
        {
            if(player1_ready && player2_ready)
            {
                transition -= 1 * (Time.deltaTime / 2);
                GameObject.Find("Fade").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, (1.0f - transition));
                if (transition <= 0)
                {
                    SceneManager.LoadScene("Mainmenu");
                }
            }
        }
    }

    public void SetGamemode1()
    {
        GameMode = 1;
        player_jumpOverride = false;
        player_jumpHeight = 600;
        game_start = true;
        game_roundTime = 60.0f;
        Camera.main.orthographic = false;
        GameObject.Find("FloorManager").GetComponent<FloorManager>().countdown = 3.0f;
        GameObject.Find("FloorManager").GetComponent<FloorManager>().spawnSafeRoom = false;
        GameObject.Find("Player1").GetComponent<InstructionsControl>().startPos = GameObject.Find("Player1").transform.position;
        GameObject.Find("Player2").GetComponent<InstructionsControl>().startPos = GameObject.Find("Player2").transform.position;
        ToggleBackground(true);
        if(UI != null)
            UI.SetActive(true);
        game_theme = NewTheme();
    }

    public void SetGamemode2()
    {
        GameMode = 2;
        player_jumpOverride = true;
        player_jumpHeight = 750;
        game_start = false;
        Camera.main.orthographic = true;
        ToggleBackground(false);
        if(UI != null)
            UI.SetActive(false);
    }

    public void ToggleBackground(bool toggle)
    {
        if (Background != null)
            Background.SetActive(toggle);
        if (Midground != null)
            Midground.SetActive(toggle);
        if (Foreground != null)
            Foreground.SetActive(toggle);        
    }

    void NewRound()
    {
        if (game_start)
        {
            game_roundTime -= 1 * Time.deltaTime;

            if (game_roundTime <= 0)
            {
                GameObject.Find("FloorManager").GetComponent<FloorManager>().spawnSafeRoom = true;
            }
        }
    }

    string NewTheme()
    {
        string newtheme = game_theme;
        while (newtheme == game_theme)
        { 
            int rand = Mathf.RoundToInt(Random.Range(0, 2));

            switch (rand)
            {
                case 0:
                    {
                        newtheme = "Jungle";
                        break;
                    }
                case 1:
                    {
                        newtheme = "Desert";
                        break;
                    }
            }
        }
        return newtheme;
    }

    public Sprite GetPlayer1Powerup()
    {
        return powerupSprites[player1_powerup];
    }
    public Sprite GetPlayer2Powerup()
    {
        return powerupSprites[player2_powerup];
    }
}
