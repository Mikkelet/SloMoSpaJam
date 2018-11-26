using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    int scoreTeamOne = 0;
    int scoreTeamTwo = 0;
    Ball ball;
    Player[] players;
    AudioSource source;
    public Text teamOneText;
    public Text teamTwoText;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        ball = FindObjectOfType<Ball>();
        players = FindObjectsOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateScores();
        CheckBallDistance();

		if(!source.isPlaying)
        { source.Play(); }

        if(scoreTeamOne == 10 || scoreTeamTwo == 10)
        {
            ResetGame();
        }
	}

    public void TeamOneScores()
    {
        scoreTeamOne++;
    }
    public void TeamTwoScores()
    {
        scoreTeamTwo++;
    }

    void ResetGame()
    {
        foreach (Player p in players)
        {
            p.ResetPosition();
        }
        scoreTeamTwo = 0;
        scoreTeamOne = 0;
    }

    void UpdateScores()
    {
        teamOneText.text = "TEAM ONE SCORE: " + scoreTeamOne;
        teamTwoText.text = "TEAM TWO SCORE: " + scoreTeamTwo;
    }

    void CheckBallDistance()
    {
        if(Vector2.Distance(this.transform.position,ball.transform.position) > 50)
        {
            ball.ResetPosition();
        }
    }
}
