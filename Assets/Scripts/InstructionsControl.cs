using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsControl : MonoBehaviour {

    GameManager manager;
    Score score;

    public GameObject smoke;
    public GameObject smokeGood;
    public GameObject sparkle;
    public GameObject shield;
    public GameObject jammed, muddled;

    Animator animator;
    Rigidbody2D rigid;

    AudioSource source;
    public AudioClip jumpSound;  
    public AudioClip rockBreakSound;
    public AudioClip pickupSound;
    public AudioClip thudSound;
    public AudioClip multiplierSound;
    public AudioClip deathSound;
    public AudioClip shieldSound;
    public AudioClip menuClick;
    
    public bool running = false;
    public bool jumped = false;
    public bool dived = false;
    public bool sliding = false;
    public bool falling = false;
    public bool respawning = false;
    public bool canSlide = true;
    public bool canJump = true;
    public bool canDive = true;

    float jumpTimer = 0.0f;
    float slideTimer = 0.0f;
    float slideSmoke = 0.1f;
    float respawnDelay = 2.0f;
    public float respawnTimer = 0.0f;
    public float powerupTimer = 11.0f;

    int p1_tick = 10, p2_tick = 10; 

    public int playerNumber;

    Vector2 standing_offset = new Vector2(0, -0.22f);
    Vector2 standing_size = new Vector2(1.04f, 2.07f);

    Vector2 crouch_offset = new Vector2(0, -0.77f);
    Vector2 crouch_size = new Vector2(1.04f, 1.0f);

    public Vector3 startPos;

    private void Awake()
    {
        playerNumber = gameObject.name == "Player1" ? 1 : 2;
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("ScoreManager"))
        {
            score = GameObject.Find("ScoreManager").GetComponent<Score>();
        }
        if (GameObject.Find("GameManager"))
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        animator = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();


        StartValues();
        InvokeRepeating("RandomisePowerup", 0.0f, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    public void StartValues()
    {
        startPos = transform.position;
        respawnTimer = manager.player_respawn;
    }

    private void UpdatePosition()
    {
        Running();

        transform.Translate(new Vector3(GetPlayerSpeed() * Time.deltaTime, 0));
        GetComponent<Rigidbody2D>().simulated = GetPlayerPhysics();
        if (jumpTimer > 0)
        {
            jumpTimer -= 1 * Time.deltaTime;
        }
        if(slideTimer > 0)
        {
            slideTimer -= 1 * Time.deltaTime;
        }
        if (powerupTimer < 10.1f)
        {
            powerupTimer -= 1 * Time.deltaTime;
        }

        if(sliding)
        {
            slideSmoke -= 1 * Time.deltaTime;
            if(slideSmoke <= 0)
            {
                GameObject slidingSmoke = Instantiate(smoke, new Vector3(transform.position.x, transform.position.y - 1), Quaternion.identity) as GameObject;
                slidingSmoke.transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
                slideSmoke = 0.1f;
            }
        }
        if(slideTimer <= 0)
        {
            SlideUp();
        }

        if(powerupTimer <= 0)
        {
            CancelPowerup();
        }

        if(respawning)
        {
            respawnTimer -= 1 * Time.deltaTime;

            if (respawnTimer < 3.0f)
            {
                manager.DisableInput(playerNumber, true);
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<Rigidbody2D>().gravityScale = 3;
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            }
            if (respawnTimer < 2.75f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            if (respawnTimer < 2.5f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            if (respawnTimer < 2.25f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
            if (respawnTimer < 2.0f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            if (respawnTimer < 1.75f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            if (respawnTimer < 1.5f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            if (respawnTimer < 1.25f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
            if (respawnTimer < 1.0f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            if (respawnTimer < .75f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            if (respawnTimer < .5f)
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            if (respawnTimer < 0.25f)
            {
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                respawning = false;
                respawnTimer = manager.player_respawn;
            }
        }

        if (rigid.velocity.y < 0.0f)
        {
            canJump = false;
        }
        if (rigid.velocity.y < -4.0f)
        {
            Falling();
        }
    }

    public void Running()
    {
        if(GetPlayerSpeed() == 0)
        {
            if (!jumped && !sliding)
            {
                running = false;
                animator.SetBool("Running", false);
            }
        }
        if (manager.GameMode == 0 || manager.GameMode == 1)
        {
            if (GetPlayerSpeed() > 0 || manager.player_runOverride)
            {
                transform.localScale = new Vector3(0.96f, transform.localScale.y);
                running = true;
                animator.SetBool("Running", true);
            }
        }
        if (manager.GameMode == 2 && GetPlayerSpeed() != 0)
        {
            if (GetPlayerSpeed() > 0.1f)
            {
                transform.localScale = new Vector3(0.96f, transform.localScale.y);
            }
            if(GetPlayerSpeed() < -0.1f)
            {
                transform.localScale = new Vector3(-0.96f, transform.localScale.y);
            }

            running = true;
            animator.SetBool("Running", true);
        }
    }

    public void Jump()
    {
        if (manager.GameMode == 0 || manager.GameMode == 1)
        {
            if (running && !sliding && !dived && canJump)
            {
                animator.SetBool("Jumping", true);
                animator.SetBool("JumpOverride", false);
                if (!jumped && !sliding && !dived)
                {
                    source.PlayOneShot(jumpSound, 1);
                    jumped = true;
                    rigid.AddForce(new Vector2(0, Mathf.Abs(manager.player_jumpHeight)));
                    canJump = false;
                    jumpTimer = 0.4f;
                    GetComponent<BoxCollider2D>().size = crouch_size;
                }
            }
        }
        if(manager.GameMode == 2)
        {
            if (!jumped && canJump && manager.player_jumpOverride)
            {
                source.PlayOneShot(jumpSound, 1);
                jumped = true;
                rigid.AddForce(new Vector2(0, Mathf.Abs(manager.player_jumpHeight)));
                canJump = false;
                jumpTimer = 0.4f;
            }
        }
    }

    public void Dive()
    {
        if (running && !sliding && !dived && !jumped && canDive)
        {
            animator.SetBool("Rolling", true);
            if (!dived && !jumped && !sliding)
            {
                dived = true;
                rigid.AddForce(new Vector2(0, Mathf.Abs(400)));
                canDive = false;
                jumpTimer = 0.38f;
            }
        }
    }

    public void Falling()
    {
        animator.SetBool("Falling", true);
        falling = true;
    }

    public void SlideDown()
    {
        if (running && !jumped && !dived && !sliding && canSlide)
        {
            animator.SetBool("Sliding", true);
            sliding = true;
            canSlide = false;
            slideTimer = 3.0f;

            GetComponent<BoxCollider2D>().offset = crouch_offset;
            GetComponent<BoxCollider2D>().size = crouch_size;
        }
    }

    public void SlideUp()
    {
        if(sliding)
        {
            sliding = false;
            animator.SetBool("Sliding", false);

            GetComponent<BoxCollider2D>().offset = standing_offset;
            GetComponent<BoxCollider2D>().size = standing_size;
        }
    }

    public void Respawn()
    {
        CancelPowerup();
        source.PlayOneShot(deathSound, 1);
        manager.DisableInput(playerNumber, false);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        float xPos;

        if(this.gameObject.name == "Player1")
        {
            xPos = GameObject.Find("Player2").transform.position.x;
        }
        else
        {
            xPos = GameObject.Find("Player1").transform.position.x;
        }

        transform.position = new Vector3(xPos, startPos.y);
        rigid.velocity = new Vector2(0, 0);
        respawning = true;
    }

    public void UsePowerup()
    {
        if (playerNumber == 1)
        {
            switch (manager.player1_powerup)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        manager.P2enable_input = false;
                        GameObject.Find("Player2").GetComponent<InstructionsControl>().powerupTimer = 5.0f;
                        GameObject P2Jam = Instantiate(jammed, GameObject.Find("Player2").transform) as GameObject;
                        P2Jam.name = "P2Jam";
                        break;
                    }
                case 2:
                    {
                        GameObject.Find("Player2").GetComponent<Player2_Input>().muddled = true;
                        GameObject.Find("Player2").GetComponent<InstructionsControl>().powerupTimer = 5.0f;
                        GameObject P2Muddle = Instantiate(muddled, GameObject.Find("Player2").transform) as GameObject;
                        P2Muddle.name = "P2Muddle";
                        break;
                    }
                case 3:
                    {
                        increaseMultiplier(playerNumber, 3);
                        break;
                    }
                case 4:
                    {
                        manager.player1_shielded = true;
                        source.PlayOneShot(shieldSound, 0.6f);
                        powerupTimer = 10.0f;
                        GameObject Player1_Shield = Instantiate(shield, transform.position, Quaternion.identity, transform) as GameObject;
                        Player1_Shield.name = "Player1_Shield";
                        break;
                    }
            }
            manager.player1_powerup = 0;
            manager.player1_hasPowerup = false;
            manager.player1_hitPowerup = false;

        }
        else if(playerNumber == 2)
        {
            switch (manager.player2_powerup)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        manager.P1enable_input = false;
                        GameObject.Find("Player1").GetComponent<InstructionsControl>().powerupTimer = 5.0f;
                        GameObject P1Jam = Instantiate(jammed, GameObject.Find("Player1").transform) as GameObject;
                        P1Jam.name = "P1Jam";
                        break;
                    }
                case 2:
                    {
                        GameObject.Find("Player1").GetComponent<Player1_Input>().muddled = true;
                        GameObject.Find("Player1").GetComponent<InstructionsControl>().powerupTimer = 5.0f;
                        GameObject P1Muddle = Instantiate(muddled, GameObject.Find("Player1").transform) as GameObject;
                        P1Muddle.name = "P1Muddle";
                        break;
                    }
                case 3:
                    {
                        increaseMultiplier(playerNumber, 3);
                        break;
                    }
                case 4:
                    {
                        manager.player2_shielded = true;
                        source.PlayOneShot(shieldSound, 0.6f);
                        powerupTimer = 10.0f;
                        GameObject Player2_Shield = Instantiate(shield, transform.position, Quaternion.identity, transform) as GameObject;
                        Player2_Shield.name = "Player2_Shield";
                        break;
                    }
            }
            manager.player2_powerup = 0;
            manager.player2_hasPowerup = false;
            manager.player2_hitPowerup = false;
        }
    }

    public void CancelPowerup()
    {
        if(playerNumber == 1)
        {
            if (GameObject.Find("P1Muddle"))
            {
                Destroy(GameObject.Find("P1Muddle"));
                GetComponent<Player1_Input>().muddled = false;
            }
            if (GameObject.Find("P1Jam"))
            {
                Destroy(GameObject.Find("P1Jam"));
                manager.P1enable_input = true;
            }
            if(GameObject.Find("Player1_Shield"))
            {
                Destroy(GameObject.Find("Player1_Shield"));
                manager.player1_shielded = false;
            }
        }
        else
        {
            if (GameObject.Find("P2Muddle"))
            {
                Destroy(GameObject.Find("P2Muddle"));
                GetComponent<Player2_Input>().muddled = false;
            }
            if (GameObject.Find("P2Jam"))
            {
                Destroy(GameObject.Find("P2Jam"));
                manager.P2enable_input = true;
            }
            if(GameObject.Find("Player2_Shield"))
            {
                Destroy(GameObject.Find("Player2_Shield"));
                manager.player2_shielded = false;
            }
        }
        powerupTimer = 11.0f;
    }

    public void AddPoints(int points)
    {
        if(playerNumber == 1)
        {
            score.s_p1_addedScore = points;
        }
        else
        {
            score.s_p2_addedScore = points;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint2D contact = collision.contacts[0];
            if ((contact.normal.y < 0.0f) && jumped)
            {
                jumped = false;
            }
        }
        if (falling)
        {
            falling = false;
            animator.SetBool("Falling", false);
            animator.SetBool("Land", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), true);
        if (collider.tag == "Floor" || collider.tag.Contains("Door") || collider.tag == "Drop" || collider.tag == "Wall")
        {
            Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
            GetComponent<Rigidbody2D>().gravityScale = 3;
        }

        if(collider.tag == "FloorP1" && playerNumber == 1) {
            Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }

        if (collider.tag == "Obstacle")
        {
            Instantiate(smoke, collision.gameObject.transform.position, Quaternion.identity);
            source.PlayOneShot(thudSound, 0.8f);
            increaseMultiplier(playerNumber, 0, true);
            Destroy(collision.gameObject);
        }
        if (collider.tag == "Breakable")
        {
            source.PlayOneShot(rockBreakSound, 0.4f);
            Destroy(collision.gameObject);

            if (dived)
            {
                Instantiate(smokeGood, collision.gameObject.transform.position, Quaternion.identity);
                increaseMultiplier(playerNumber, 1);
            }
            else
            {
                Instantiate(smoke, collision.gameObject.transform.position, Quaternion.identity);
                increaseMultiplier(playerNumber, 0, true);
            }

        }
        if (collider.tag == "Powerup")
        {
            source.PlayOneShot(pickupSound, 1);
            Instantiate(sparkle, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            if(playerNumber == 1)
            {
                manager.player1_hitPowerup = true;
                p1_tick = 10;
            }
            else
            {
                manager.player2_hitPowerup = true;
                p2_tick = 10;
            }
        }

        if (collision.gameObject.name == "DeathZone")
        {
            if(playerNumber == 1 && manager.player1_shielded)
            {
                Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
            }
            if (playerNumber == 2 && manager.player2_shielded)
            {
                Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
            }
        }


        if (collision.gameObject.tag == "Floor")
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint2D contact = collision.contacts[0];
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
                {
                    rigid.velocity = new Vector2(0, 0);
                    animator.SetBool("Jumping", false);
                    canJump = true;
                    jumped = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (jumped && jumpTimer <= 0)
        {
            animator.SetBool("Jumping", false);
            GetComponent<BoxCollider2D>().size = standing_size;
        }
        if (dived && jumpTimer <= 0)
        {
            dived = false;
            animator.SetBool("Rolling", false);
        }
        if(falling && collision.gameObject.tag == "Floor")
        {
            falling = false;
            animator.SetBool("Falling", false);
            animator.SetBool("Land", true);
        }
    }

    public void increaseMultiplier(int player, int _score, bool reset = false)
    {
        if (manager.GameMode == 1)
        {
            //if (_score > 0)
            //{
            //    source.PlayOneShot(multiplierSound, 0.2f);
            //}
            //else
            //{
            //    source.PlayOneShot(multiplierResetSound, 0.5f);
            //}

            if (player == 1)
            {
                if ((reset && !manager.player1_shielded) || !reset)
                {
                    score.UpdateP1Multiplier(_score, reset);
                }
            }
            if(player == 2)
            {
                if ((reset && !manager.player2_shielded) || !reset)
                {
                    score.UpdateP2Multiplier(_score, reset);
                }
            }
        }
    }

    public void RandomisePowerup()
    {
        if(playerNumber == 1 && manager.player1_hitPowerup && !manager.player1_hasPowerup)
        {
            if (p1_tick > 0)
            {   
                int rand = Mathf.RoundToInt(Random.Range(1, 5));
                manager.player1_powerup = rand;
                p1_tick -= 1;

            }
            if(p1_tick <= 0)
                manager.player1_hasPowerup = true;
        }
        else if(playerNumber == 2 && manager.player2_hitPowerup && !manager.player2_hasPowerup)
        {
            if (p2_tick > 0)
            {
                int rand = Mathf.RoundToInt(Random.Range(1, 5));
                manager.player2_powerup = rand;
                p2_tick -= 1;
            }
            if(p2_tick <= 0)
                manager.player2_hasPowerup = true;
        }
    }

    float GetPlayerSpeed()
    {
        if(playerNumber == 1)
        {
            return manager.player1_speed;
        }
        else
        {
            return manager.player2_speed;
        }
    }

    bool GetPlayerPhysics()
    {
        if (playerNumber == 1)
        {
            return manager.player1_physics;
        }
        else
        {
            return manager.player2_physics;
        }
    }

    public void PlayClick()
    {
        source.PlayOneShot(menuClick, 0.8f);
    }
}

