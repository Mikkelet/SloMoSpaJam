using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    BoxCollider2D coll;
    [SerializeField]
    Transform sprite;
    [SerializeField]
    Planet STARTING_PLANET;
    [SerializeField]
    Planet currentPlanet;
    [SerializeField]
    Planet targetPlanet;
    [SerializeField]
    Planet lastTarget;
    Planet[] allPlanets;
    [SerializeField]
    Ball ball;
    public int playerNumber = 1;

    AudioSource source;
    AudioSource mainCamSource;
    public AudioClip soundZoom;
    public AudioClip voiceLine;
    AudioClip musicNormal;
    AudioClip musicSloMo;

    bool pauseInput;
    string hAxis;
    string vAxis;
    KeyCode aButton;

    // Use this for initialization
    void Start ()
    {
        //soundZoom = Resources.Load<AudioClip>("Sounds/PlayerMove");
        musicNormal = Resources.Load<AudioClip>("Sounds/MusicNormal");
        musicSloMo = Resources.Load<AudioClip>("Sounds/MusicSlowMo");
        source = GetComponent<AudioSource>();
        mainCamSource = Camera.main.GetComponent<AudioSource>();

        lastTarget = currentPlanet;
        STARTING_PLANET = currentPlanet;
        SetControllerAxis();
        UpdateRotation();
    }
	
	// Update is called once per frame
	void Update () {
        ControllerInput();
    }
    void UpdateRotation()
    {
        Vector2 thisPos = transform.position;
        Vector2 planetPos = lastTarget.transform.position;
        if(currentPlanet != null)
        {
           planetPos = currentPlanet.transform.position;
        }
        if (targetPlanet != null)
            planetPos = targetPlanet.transform.position;


        Vector2 diff = (planetPos - thisPos);
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360;

        if (angle > 360) angle -= 360;

        angle -= 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        UpdatePosition();
    }
    void UpdatePosition()
    {
        transform.position = currentPlanet.transform.position;
        float planetRadius = currentPlanet.GetRadius();
        Vector2 newPos = new Vector2(0, planetRadius * 2);
        sprite.localPosition = newPos;
    }
    void SetControllerAxis()
    {
        hAxis = "J" + playerNumber + "Horizontal";
        vAxis = "J" + playerNumber + "Vertical";
        switch(playerNumber)
        {
            case 1:
                aButton = KeyCode.Joystick1Button0;
                break;
            case 2:
                aButton = KeyCode.Joystick2Button0;
                break;
            case 3:
                aButton = KeyCode.Joystick3Button0;
                break;
            case 4:
                aButton = KeyCode.Joystick4Button0;
                break;
            default:
                break;
        }
    }
    void ControllerInput()
    {
        if (Input.GetKeyDown(aButton))
        {
            if (currentPlanet == ball.CurrentPlanet())
            {
                ball.Shoot();
                ball.SetLastTouchedBy(this);
            }
            else
            {
                if (targetPlanet != null)
                {
                    if (targetPlanet != currentPlanet)
                    {
                        source.clip = soundZoom;
                        source.Play();
                    }
                    currentPlanet = targetPlanet;
                    TargetPlanetWithRay();
                    UpdateRotation();
                }
            }
        }
        TargetPlanetWithRay();
    }
    Vector2 JoystickVelocity()
    {
        float velocityX = Input.GetAxis(hAxis);
        float velocityY = Input.GetAxis(vAxis);
        Vector2 v2 = new Vector2(velocityX, -velocityY);
        return v2;
    }
    void TargetPlanetWithRay()
    {
        RaycastHit2D[] hit;
        Vector2 dir = JoystickVelocity();
        Ray2D ray = new Ray2D(currentPlanet.transform.position, dir);
        hit = Physics2D.RaycastAll(ray.origin, ray.direction, 40);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.name != currentPlanet.name)
            {
                if (hit[i].collider.GetComponent<Planet>() != null)
                {
                    targetPlanet = hit[i].collider.GetComponent<Planet>();
                    Debug.DrawRay(transform.position, JoystickVelocity() * 40, Color.white, 200);
                }
            }

        }
        UpdateRotation();
    }
    public void ResetPosition()
    {
        currentPlanet = STARTING_PLANET;
    }
    void ApplySlowMotion()
    {
        if(currentPlanet == ball.CurrentPlanet())
        {
            Time.timeScale = 0.5f;
            mainCamSource.clip = musicSloMo;
        }
        else { Time.timeScale = 1;
            mainCamSource.clip = musicNormal;
            }
    }

    public void PlayVoice()
    {
        source.clip = voiceLine;
        source.Play();
    }


    // method graveyard
    void ResetCurrent()
    {
        if(currentPlanet == null)
        { currentPlanet = lastTarget; }
    }
    void TargetColor()
    {
        foreach (Planet p in allPlanets)
        {
            if (p == allPlanets[0])
            {
                p.transform.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                p.transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    void UpdateTargetPlanets()
    {
        allPlanets = currentPlanet.AllPlanets();
    }
}
