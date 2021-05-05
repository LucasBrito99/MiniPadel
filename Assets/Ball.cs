using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Vector3 initialPos; // ball's initial position
    public string hitter;

    int playerScore;    
    int botScore;

    [SerializeField] Text playerScoreText;
    [SerializeField] Text botScoreText;

    public bool playing = true;

    private void Start()
    {
        initialPos = transform.position; // default it to where we first place it in the scene
        playerScore = 0;
        botScore = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Out")) // if the ball hits a wall
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // reset it's velocity to 0 so it doesn't move anymore
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //transform.position = initialPos; // reset it's position 

            GameObject.Find("player").GetComponent<Player>().Reset();

            if(hitter == "player"){
                playerScore++;
            }
            else if(hitter == "bot"){
                botScore++;
            }
            playing = false;
            updateScores();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Out") && playing){
            if(hitter == "player"){
                botScore++;
            }
            else if(hitter == "bot"){
                playerScore++;
            }
            playing = false;
            updateScores();
        }
    }

    private void updateScores(){
        playerScoreText.text = "Player: " + playerScore;
        botScoreText.text = "Bot: " + botScore;
    }

}