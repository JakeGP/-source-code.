using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackThemeSwitcher : MonoBehaviour
{
    ThemeManager manager;

    private void Start()
    {
        manager = GameObject.Find("ThemeManager").GetComponent<ThemeManager>();
        SetTheme();
    }

    void SetTheme()
    {
        foreach (Transform child in transform)
        {
            if (child != null && child.gameObject != null && child.GetComponent<SpriteRenderer>() != null)
            {
                switch (child.gameObject.name)
                {
                    case "Floor":
                        {
                            int floorType = int.Parse(child.GetComponent<SpriteRenderer>().sprite.name.Substring(5, 1));
                            if (floorType > 0)
                            {
                                child.GetComponent<SpriteRenderer>().sprite = manager.CurrentFloor[floorType - 1];
                            }
                            break;
                        }
                    case "Breakable":
                        {
                            child.GetComponent<SpriteRenderer>().sprite = manager.CurrentSmash;
                            break;
                        }
                    case "JumpSingle":
                        {
                            int random = Mathf.RoundToInt(Random.Range(0, manager.CurrentJump1.Length));
                            child.GetComponent<SpriteRenderer>().sprite = manager.CurrentJump1[random];
                            break;
                        }
                    case "JumpDouble":
                        {
                            child.GetComponent<SpriteRenderer>().sprite = manager.CurrentJump2;
                            break;
                        }
                    case "Duck":
                        {
                            child.GetComponent<SpriteRenderer>().sprite = manager.CurrentDuck;
                            break;
                        }
                    case "Duck2":
                        {
                            child.GetComponent<SpriteRenderer>().sprite = manager.CurrentDuck2;
                            break;
                        }
                    case "Death":
                        {
                            child.GetComponent<SpriteRenderer>().sprite = manager.CurrentDeath;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}

