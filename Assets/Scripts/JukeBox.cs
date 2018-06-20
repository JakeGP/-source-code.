using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour {

    static bool m_AudioBegin = false;
    public GameObject m_musicPlayer;

    private void Awake()
    {
        AudioSource audio = GetComponent<AudioSource>();

        //Find object called "MUSIC"
        m_musicPlayer = GameObject.Find("MUSIC");
        if(m_musicPlayer == null)
        {
            //If object doesnt exist then make this object the music player
            m_musicPlayer = this.gameObject;
            //Rename this object
            m_musicPlayer.name = "MUSIC";

            DontDestroyOnLoad(m_musicPlayer);
        }
        else
        {
            if(this.gameObject.name != "MUSIC")
            {
                Destroy(this.gameObject);
            }
        }

        //Play music
        if(!m_AudioBegin)
        {
            audio.Play();
            DontDestroyOnLoad(gameObject);
            m_AudioBegin = true;
        }
    }
}
