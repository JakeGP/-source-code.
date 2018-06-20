using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1_Input : MonoBehaviour
{

    GameManager manager;

    public bool muddled = false;
    public bool toggle = false;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Character_Animations();
    }

    void Character_Animations()
    {
        if (manager.P1enable_input)
        {
            //TUTORIAL MODE
            if (manager.GameMode == 0)
            {
                if (Input.GetAxis("Joystick1_LeftVertical") > -0.1f)
                {
                    this.GetComponent<InstructionsControl>().SlideUp();
                    this.GetComponent<InstructionsControl>().canSlide = true;
                }

                switch (manager.tutorial_step)
                {
                    case 1:
                        {
                            if (Input.GetAxisRaw("Joystick1_A") == 1 || Input.GetKeyDown("w"))
                            {
                                manager.player1_ready = true;
                            }

                            if (Input.GetAxisRaw("Joystick1_Back") > 0.1f)
                            {
                                if (!manager.player1_ready)
                                {
                                    gameObject.GetComponent<InstructionsControl>().PlayClick();
                                }
                                manager.player1_ready = true;
                                manager.player2_ready = true;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (Input.GetAxisRaw("Joystick1_LeftVertical") < -0.1f)
                            {
                                manager.player1_ready = true;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (Input.GetAxisRaw("Joystick1_B") == 1 || Input.GetKeyDown("d"))
                            {
                                manager.player1_ready = true;
                            }
                            break;
                        }
                }
            }

            //AUTO RUN OR PLAYER CONTROL MODE
            if ((manager.GameMode == 1 && !muddled) || (manager.GameMode == 1 && muddled && manager.player1_shielded) || (manager.GameMode == 1 && !muddled && manager.player1_shielded))
            {
                //JUMP
                if (Input.GetAxis("Joystick1_A") == 1 || Input.GetKeyDown("w"))
                {
                    this.GetComponent<InstructionsControl>().Jump();
                }
                if (Input.GetAxis("Joystick1_A") == 0 || Input.GetKeyUp("w"))
                {
                    this.GetComponent<InstructionsControl>().canJump = true;
                }

                //DIVE
                if (!manager.paused)
                {
                    if (Input.GetAxis("Joystick1_B") == 1 || Input.GetKeyDown("d"))
                    {
                        this.GetComponent<InstructionsControl>().Dive();
                    }
                    if (Input.GetAxis("Joystick1_B") == 0 || Input.GetKeyUp("d"))
                    {
                        this.GetComponent<InstructionsControl>().canDive = true;
                    }
                }
                else
                {
                    if (Input.GetAxisRaw("Joystick1_B") == 1 || Input.GetKeyDown("d"))
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                }

                //SLIDE
                if (Input.GetAxis("Joystick1_LeftVertical") < -0.1f)
                {
                    this.GetComponent<InstructionsControl>().SlideDown();
                }
                if (Input.GetAxis("Joystick1_LeftVertical") > -0.1f)
                {
                    this.GetComponent<InstructionsControl>().SlideUp();
                    this.GetComponent<InstructionsControl>().canSlide = true;
                }

                if (manager.player1_hasPowerup)
                {
                    //POWERUP
                    if (Input.GetAxis("Joystick1_LeftBumper") > 0.1f)
                    {
                        this.GetComponent<InstructionsControl>().UsePowerup();
                    }
                    if (Input.GetAxis("Joystick1_RightBumper") > 0.1f)
                    {
                        this.GetComponent<InstructionsControl>().UsePowerup();
                    }
                }

                //START AND END
                if (Input.GetAxisRaw("Joystick1_Start") > 0.1f)
                {
                    if (SceneManager.GetActiveScene().name != "MainMenu")
                    {
                        if (!toggle)
                        {
                            if (manager.paused)
                            {
                                manager.paused = false;
                            }
                            else
                            {
                                manager.paused = true;
                            }
                        }
                        toggle = true;
                    }
                    else
                    {
                        manager.player1_ready = true;
                    }
                }
                if (Input.GetAxisRaw("Joystick1_Start") <= 0.0f)
                {
                    toggle = false;
                }
                if (Input.GetAxis("Joystick1_Back") > 0.1f)
                {
                    //this.GetComponent<InstructionsControl>().Stop();
                }
            }

            //Muddled mode
            if (manager.GameMode == 1 && muddled && !manager.player1_shielded)
            {
                //JUMP
                if (Input.GetAxis("Joystick1_A") == 1 || Input.GetKeyDown("w"))
                {
                    this.GetComponent<InstructionsControl>().Dive();
                }
                if (Input.GetAxis("Joystick1_A") == 0 || Input.GetKeyUp("w"))
                {
                    this.GetComponent<InstructionsControl>().canDive = true;
                }

                //DIVE
                if (!manager.pause)
                {
                    if (Input.GetAxis("Joystick1_B") == 1 || Input.GetKeyDown("d"))
                    {
                        this.GetComponent<InstructionsControl>().Jump();
                    }
                    if (Input.GetAxis("Joystick1_B") == 0 || Input.GetKeyUp("d"))
                    {
                        this.GetComponent<InstructionsControl>().canJump = true;
                    }
                }
                else
                {
                    if (Input.GetAxis("Joystick1_B") == 1 || Input.GetKeyDown("d"))
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                }

                //SLIDE
                if (Input.GetAxis("Joystick1_LeftVertical") > 0.1f)
                {
                    this.GetComponent<InstructionsControl>().SlideDown();
                }
                if (Input.GetAxis("Joystick1_LeftVertical") < 0.1f)
                {
                    this.GetComponent<InstructionsControl>().SlideUp();
                    this.GetComponent<InstructionsControl>().canSlide = true;
                }

                if (manager.player1_hasPowerup)
                {
                    //POWERUP
                    if (Input.GetAxis("Joystick1_LeftBumper") > 0.1f)
                    {
                        this.GetComponent<InstructionsControl>().UsePowerup();
                    }
                    if (Input.GetAxis("Joystick1_RightBumper") > 0.1f)
                    {
                        this.GetComponent<InstructionsControl>().UsePowerup();
                    }
                }
            }

            //Player Run Mode
            if (manager.GameMode == 2)
            {
                //JUMP
                if (Input.GetAxis("Joystick1_A") == 1 || Input.GetKeyDown("w"))
                {
                    this.GetComponent<InstructionsControl>().Jump();
                }
                if (Input.GetAxis("Joystick1_A") == 0 || Input.GetKeyUp("w"))
                {
                    this.GetComponent<InstructionsControl>().canJump = true;
                }
                if (Input.GetAxis("Joystick1_LeftHorizontal") < -0.1f)
                {
                    manager.player1_speed = -8.0f;
                }
                if (Input.GetAxis("Joystick1_LeftHorizontal") > 0.1f)
                {
                    manager.player1_speed = 8.0f;
                }
                if (Input.GetAxis("Joystick1_LeftHorizontal") == 0.0f)
                {
                    manager.player1_speed = 0.0f;
                }
                if(manager.pause)
                {
                    if (Input.GetAxisRaw("Joystick1_B") == 1 || Input.GetKeyDown("d"))
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                }

                //START AND END
                if (Input.GetAxisRaw("Joystick1_Start") > 0.1f)
                {
                    if (SceneManager.GetActiveScene().name != "MainMenu")
                    {
                        if (!toggle)
                        {
                            if (manager.paused)
                            {
                                manager.paused = false;
                            }
                            else
                            {
                                manager.paused = true;
                            }
                        }
                        toggle = true;
                    }
                    else
                    {
                        manager.player1_ready = true;
                    }
                }
                if (Input.GetAxisRaw("Joystick1_Start") <= 0.0f)
                {
                    toggle = false;
                }
                if (manager.tutorial_step == 4)
                {
                    if (Input.GetAxis("Joystick1_Back") > 0.1f)
                    {
                        manager.player1_ready = true;
                    }
                }
            }

            //Gameover
            if (manager.GameMode == 3)
            {
                if (Input.GetAxis("Joystick1_Back") > 0.1f)
                {
                    if(!manager.player1_ready)
                    {
                        gameObject.GetComponent<InstructionsControl>().PlayClick();
                    }
                    manager.player1_ready = true;
                    manager.player2_ready = true;
                }
            }
        }
    }
}
