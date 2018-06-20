using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmokeScript : MonoBehaviour {

    float timer = 0.41f;
    float randomUpward;

	// Use this for initialization
	void Start () {
        randomUpward = Random.Range(0.1f, 4.0f);
    }
	
	// Update is called once per frame
	void Update () {
        string currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "Controls Scene")
        {
            transform.Translate(new Vector3(-8.0f * Time.deltaTime, 0));
        }

        timer -= 1 * Time.deltaTime;

        if(timer <= 0)
        {
            Destroy(this.gameObject);
        }

        transform.Translate(new Vector3(0, randomUpward * Time.deltaTime));
    }

}
