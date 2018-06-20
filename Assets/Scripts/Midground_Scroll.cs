using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Midground_Scroll : MonoBehaviour
{ 
    GameObject midground1, midground2;
    GameManager manager;

    float m_scrollSpeed;
    public float m_tileSizeX;

    private Vector3 startPosition;

    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        midground1 = GameObject.Find("Midground1");
        midground2 = GameObject.Find("Midground2");
        startPosition = transform.localPosition;
    }

    void Update()
    {
        if (transform.position.x < Camera.main.transform.position.x - (m_tileSizeX * 1.4))
        {
            if (this.name == "Midground1")
            {
                transform.localPosition = new Vector3(midground2.transform.localPosition.x + m_tileSizeX, startPosition.y, 1);
            }
            else
            {
                transform.localPosition = new Vector3(midground1.transform.localPosition.x + m_tileSizeX, startPosition.y, 1);
            }
        }
        Transform midground = GameObject.Find("Midground").transform;
        midground.position = new Vector3(midground.position.x, Camera.main.transform.position.y, midground.position.z);
    }
}