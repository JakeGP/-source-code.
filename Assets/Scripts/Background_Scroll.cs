using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scroll : MonoBehaviour
{
    GameObject background1, background2;
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
        background1 = GameObject.Find("Background1");
        background2 = GameObject.Find("Background2");
        startPosition = transform.localPosition;
    }

    void Update()
    {
        if (transform.position.x < Camera.main.transform.position.x - (m_tileSizeX * 1.4))
        {
            if (this.name == "Background1")
            {
                transform.localPosition = new Vector3(background2.transform.localPosition.x + m_tileSizeX, startPosition.y, 0);
            }
            else
            {
                transform.localPosition = new Vector3(background1.transform.localPosition.x + m_tileSizeX, startPosition.y, 0);
            }
        }
        Transform background = GameObject.Find("Background").transform;
        background.position = new Vector3(background.position.x, Camera.main.transform.position.y, background.position.z);
    }
}
