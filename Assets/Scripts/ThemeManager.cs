using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour {

    GameManager manager;

    public Sprite[] CurrentFloor;
    public Sprite[] CurrentJump1;
    public Sprite CurrentJump2;
    public Sprite CurrentSmash;
    public Sprite CurrentDuck;
    public Sprite CurrentDuck2;
    public Sprite CurrentDeath;

    //JUNGLE
    public Sprite JungleBackground;
    public Sprite JungleMidground;
    public Sprite JungleForeground;
    public Sprite[] JungleFloor;
    public Sprite[] JungleJump1;
    public Sprite JungleJump2;
    public Sprite JungleSmash;
    public Sprite JungleDuck;
    public Sprite JungleDeath;

    //DESERT
    public Sprite DesertBackground;
    public Sprite DesertMidground;
    public Sprite DesertForeground;
    public Sprite[] DesertFloor;
    public Sprite[] DesertJump1;
    public Sprite DesertJump2;
    public Sprite DesertSmash;
    public Sprite DesertDuck;
    public Sprite DesertDeath;


    // Use this for initialization
    void Start () {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (manager.GameMode == 1)
        {
            ChangeTheme(manager.game_theme);
        }
	}

    public void ChangeTheme(string theme)
    {
        switch(theme)
        {
            case "Jungle":
                {
                    Jungle();
                    break;
                }
            case "Desert":
                {
                    Desert();
                    break;
                }
        }
    }


    void Jungle()
    {
        GameObject.FindGameObjectsWithTag("Foreground")[0].GetComponent<SpriteRenderer>().sprite = JungleForeground;
        GameObject.FindGameObjectsWithTag("Foreground")[1].GetComponent<SpriteRenderer>().sprite = JungleForeground;

        GameObject.FindGameObjectsWithTag("Midground")[0].GetComponent<SpriteRenderer>().sprite = JungleMidground;
        GameObject.FindGameObjectsWithTag("Midground")[1].GetComponent<SpriteRenderer>().sprite = JungleMidground;

        GameObject.FindGameObjectsWithTag("Background")[0].GetComponent<SpriteRenderer>().sprite = JungleBackground;
        GameObject.FindGameObjectsWithTag("Background")[1].GetComponent<SpriteRenderer>().sprite = JungleBackground;

        CurrentFloor = JungleFloor;
        CurrentJump1 = JungleJump1;
        CurrentJump2 = JungleJump2;
        CurrentSmash = JungleSmash;
        CurrentDuck = JungleJump1[0];
        CurrentDuck2 = JungleDuck;
        CurrentDeath = JungleDeath;
    }

    void Desert()
    {
        GameObject.FindGameObjectsWithTag("Foreground")[0].GetComponent<SpriteRenderer>().sprite = DesertForeground;
        GameObject.FindGameObjectsWithTag("Foreground")[1].GetComponent<SpriteRenderer>().sprite = DesertForeground;

        GameObject.FindGameObjectsWithTag("Midground")[0].GetComponent<SpriteRenderer>().sprite = DesertMidground;
        GameObject.FindGameObjectsWithTag("Midground")[1].GetComponent<SpriteRenderer>().sprite = DesertMidground;

        GameObject.FindGameObjectsWithTag("Background")[0].GetComponent<SpriteRenderer>().sprite = DesertBackground;
        GameObject.FindGameObjectsWithTag("Background")[1].GetComponent<SpriteRenderer>().sprite = DesertBackground;

        CurrentFloor = DesertFloor;
        CurrentJump1 = DesertJump1;
        CurrentJump2 = DesertJump2;
        CurrentSmash = DesertSmash;
        CurrentDuck = DesertJump1[2];
        CurrentDuck2 = DesertDuck;
        CurrentDeath = DesertDeath;
    }
}
