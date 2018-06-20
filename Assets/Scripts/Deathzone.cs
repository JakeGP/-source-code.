using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    public GameObject smoke;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.gameObject.name == "Player1")
            {
                gameObject.GetComponentInParent<FloorManager>().respawn1 = true;
                collision.gameObject.GetComponentInParent<FloorManager>().respawn2 = true;

                Instantiate(smoke, transform.position, Quaternion.identity);
                collision.gameObject.GetComponent<InstructionsControl>().Respawn();
                collision.gameObject.GetComponent<InstructionsControl>().increaseMultiplier(1, 0, true);
            }
            if (collision.gameObject.name == "Player2")
            {
                gameObject.GetComponentInParent<FloorManager>().respawn1 = true;
                collision.gameObject.GetComponentInParent<FloorManager>().respawn2 = true;

                Instantiate(smoke, transform.position, Quaternion.identity);
                collision.gameObject.GetComponent<InstructionsControl>().Respawn();
                collision.gameObject.GetComponent<InstructionsControl>().increaseMultiplier(2, 0, true);
            }
        }
    }
}