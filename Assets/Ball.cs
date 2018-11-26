using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private Renderer render;
    private string planetTag = "Planet";
    [SerializeField]
    private Planet currentPlanet;
    private Planet STARTING_PLANET;
    [SerializeField]
    private bool inOrbit;

    GameController gc;

    Player lastTouchedBy;

    AudioSource source;
    public AudioClip soundZoom;

    bool reverseForceApplied; // a one time changed bool for checking if object as exited the view.

    int antiMassFactor = 2; // used to reduce speed of force added
    

	// Use this for initialization
	void Start () {
        gc = Camera.main.GetComponent<GameController>();
        source = GetComponent<AudioSource>();
        soundZoom = Resources.Load<AudioClip>("Sounds/BallMove");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        render = GetComponent<Renderer>();
        STARTING_PLANET = currentPlanet;
    }

    private void FixedUpdate()
    {
        CheckBounderies();
        IncreaseOrbitalSpeed();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.transform.tag);
        if (collision.transform.tag == planetTag)
        {
            rb.gravityScale = 0f;
            currentPlanet = collision.GetComponent<Planet>();
            Camera.main.GetComponent<ScreenShake>().ShakeScreen();
            inOrbit = true;
        }
        if(collision.transform.tag == "T1Goal")
        {
            gc.TeamTwoScores();
            lastTouchedBy.PlayVoice();
            print("GOAL");
        }
        if(collision.transform.tag == "T2Goal")
        {
            gc.TeamOneScores();
            lastTouchedBy.PlayVoice();
            print("GOAL");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == planetTag)
        {
            currentPlanet = null;
            inOrbit = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == planetTag)
        {
            inOrbit = true;
            currentPlanet = collision.GetComponent<Planet>();
        }
    }

    // maybe unnecessary, currently unused
    Transform GetNearestPlanet()
    {
        Transform t = null;
        Collider2D[] planets = new Collider2D[3];
        Physics2D.OverlapCircleNonAlloc(transform.position, 10, planets);
        print(planets.Length);
        for (int i = 0; i < planets.Length; i++)
        {
            if(planets[i].transform.tag == planetTag)
            {
                t = planets[i].transform;
                t.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            }
        }
        return t;
    }

    void CheckBounderies()
    {
        if(!render.isVisible && !reverseForceApplied)
        {
            rb.velocity *= -0.5f;
            reverseForceApplied = true;
        }
        if(render.isVisible)
        {
            reverseForceApplied = false;
        }
    }

    bool CheckIfInOrbit()
    {
        if(currentPlanet != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void IncreaseOrbitalSpeed()
    {
        if(rb.velocity.magnitude < 2 && currentPlanet != null)
        {
            rb.velocity *= 10;
        }
    }

    public Planet CurrentPlanet()
    {
        return currentPlanet;
    }

    public void Shoot()
    {
        if (CheckIfInOrbit())
        {
            source.PlayOneShot(soundZoom);
            rb.AddForce(rb.velocity * currentPlanet.GetMass() / antiMassFactor);
        }
    }

    public void SetLastTouchedBy(Player p)
    {
        lastTouchedBy = p;
    }

    public void ResetPosition()
    {
        currentPlanet = STARTING_PLANET;
    }
}
