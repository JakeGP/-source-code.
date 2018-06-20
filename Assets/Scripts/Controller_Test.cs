using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Test : MonoBehaviour {

    GameManager manager;
    AudioSource source;
    public AudioClip menuClick;

    GameObject joystick1;
    Vector3 joystick1_StartPos;
    GameObject joystick2;
    Vector3 joystick2_StartPos;

    GameObject leftBumper;
    Vector3 leftBumper_StartPos;
    GameObject rightBumper;
    Vector3 rightBumper_StartPos;

    GameObject A;
    Vector3 A_StartPos;
    GameObject B;
    Vector3 B_StartPos;
    GameObject X;
    Vector3 X_StartPos;
    GameObject Y;
    Vector3 Y_StartPos;

    GameObject start;
    Vector3 start_StartPos;
    GameObject back;
    Vector3 back_StartPos;

    GameObject up;
    Vector3 up_StartPos;
    GameObject down;
    Vector3 down_StartPos;
    GameObject left;
    Vector3 left_StartPos;
    GameObject right;
    Vector3 right_StartPos;

    GameObject character;

    bool toggle = false;

    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start () {
        joystick1 = GameObject.FindGameObjectWithTag("Joystick_Left");
        joystick1_StartPos = joystick1.transform.position;

        joystick2 = GameObject.FindGameObjectWithTag("Joystick_Right");
        joystick2_StartPos = joystick2.transform.position;

        leftBumper = GameObject.FindGameObjectWithTag("Bumper_Left");
        leftBumper_StartPos = leftBumper.transform.position;
        rightBumper = GameObject.FindGameObjectWithTag("Bumper_Right");
        rightBumper_StartPos = rightBumper.transform.position;

        A = GameObject.FindGameObjectWithTag("A");
        A_StartPos = A.transform.position;
        B = GameObject.FindGameObjectWithTag("B");
        B_StartPos = B.transform.position;
        X = GameObject.FindGameObjectWithTag("X");
        X_StartPos = X.transform.position;
        Y = GameObject.FindGameObjectWithTag("Y");
        Y_StartPos = Y.transform.position;

        start = GameObject.FindGameObjectWithTag("Start");
        start_StartPos = start.transform.position;
        back = GameObject.FindGameObjectWithTag("Select");
        back_StartPos = back.transform.position;

        up = GameObject.FindGameObjectWithTag("Up");
        up_StartPos = up.transform.position;
        down = GameObject.FindGameObjectWithTag("Down");
        down_StartPos = down.transform.position;
        left = GameObject.FindGameObjectWithTag("Left");
        left_StartPos = left.transform.position;
        right = GameObject.FindGameObjectWithTag("Right");
        right_StartPos = right.transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        Input_Detect();
        
        if(Input.GetAxis("Joystick1_Back") == 1 && !toggle)
        {
            source.PlayOneShot(menuClick);
            toggle = true;
            SceneManager.LoadScene("MainMenu");
        }
	}

    void Input_Detect()
    { 
        //Left Joystick
        Vector3 joystick1Offset = new Vector3(Input.GetAxis("Joystick1_LeftHorizontal"), Input.GetAxis("Joystick1_LeftVertical")) / 2;
        joystick1.transform.position = (joystick1_StartPos + joystick1Offset);

        //Right Joystick
        Vector3 joystick2Offset = new Vector3(Input.GetAxis("Joystick1_RightHorizontal"), Input.GetAxis("Joystick1_RightVertical")) / 2;
        joystick2.transform.position = (joystick2_StartPos + joystick2Offset);

        //Bumpers
        leftBumper.transform.position = (leftBumper_StartPos - new Vector3(0, Input.GetAxis("Joystick1_LeftBumper") / 8));
        rightBumper.transform.position = (rightBumper_StartPos - new Vector3(0, Input.GetAxis("Joystick1_RightBumper") / 8));

        //Main Buttons
        A.transform.position = (A_StartPos - new Vector3(0, Input.GetAxis("Joystick1_A") / 8));
        B.transform.position = (B_StartPos - new Vector3(0, Input.GetAxis("Joystick1_B") / 8));
        X.transform.position = (X_StartPos - new Vector3(0, Input.GetAxis("Joystick1_X") / 8));
        Y.transform.position = (Y_StartPos - new Vector3(0, Input.GetAxis("Joystick1_Y") / 8));

        //Start and Back
        start.transform.position = (start_StartPos - new Vector3(0, Input.GetAxis("Joystick1_Start") / 8));
        back.transform.position = (back_StartPos - new Vector3(0, Input.GetAxis("Joystick1_Back") / 8));

        //DPad
        float dpad_horizontal = Input.GetAxis("Joystick1_DPadHorizontal");
        float dpad_vertical = Input.GetAxis("Joystick1_DPadVertical");
        Vector3 dpadXOffset = new Vector3(Input.GetAxis("Joystick1_DPadHorizontal"), 0) / 8;
        Vector3 dpadYOffset = new Vector3(0, Input.GetAxis("Joystick1_DPadVertical")) / 8;

        if (dpad_horizontal >= 0)
        {
            right.transform.position = (right_StartPos + dpadXOffset);
        }
        if(dpad_horizontal <= 0)
        {
            left.transform.position = (left_StartPos + dpadXOffset);
        }
        if (dpad_vertical >= 0)
        {
            up.transform.position = (up_StartPos + dpadYOffset);
        }
        if (dpad_vertical <= 0)
        {
            down.transform.position = (down_StartPos + dpadYOffset);
        }
    }
}