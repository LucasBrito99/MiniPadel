﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget; // the target where we aim to land the ball
    float speed = 5f; // move speed

    bool hitting; // boolean to know if we are hitting the ball or not 

    public Transform ball; // the ball 
    Animator animator;

    Vector3 aimTargetInitialPosition; // initial position of the aiming gameObject which is the center of the opposite court

    ShotManager shotManager; // reference to the shotmanager component
    Shot currentShot; // the current shot we are playing to acces it's attributes

    [SerializeField] Transform serveRight;
    [SerializeField] Transform serveLeft;
    bool servedRight = true;
    private void Start()
    {
        animator = GetComponent<Animator>(); // referennce out animator
        aimTargetInitialPosition = aimTarget.position; // initialise the aim position to the center( where we placed it in the editor )
        shotManager = GetComponent<ShotManager>(); // accesing our shot manager component 
        currentShot = shotManager.topSpin; // defaulting our current shot as topspin
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // get the horizontal axis of the keyboard
        float v = Input.GetAxisRaw("Vertical"); // get the vertical axis of the keyboard

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.topSpin; // set our current shot to top spin
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false; // we let go of the key so we are not hitting anymore and this 
        }                    // is used to alternate between moving the aim target and ourself

        if (Input.GetKeyDown(KeyCode.G))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.flat; // set our current shot to top spin
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.flatServe; // set our current shot to top spin
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.kickServe; // set our current shot to top spin
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");
        }
        
        if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T))
        {
            hitting = false;
            GetComponent<BoxCollider>().enabled = true;
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
            Vector3 dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            animator.Play("serve");
            ball.GetComponent<Ball>().hitter = "player";
            ball.GetComponent<Ball>().playing = true;
            //currentShot = shotManager.topSpin;
        }



        if (hitting)  // if we are trying to hit the ball
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime); //translate the aiming gameObject on the court horizontallly
        }


        if ((h != 0 || v != 0) && !hitting) // if we want to move and we are not hitting the ball
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime); // move on the court
        }



    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if we collide with the ball 
        {
            Vector3 dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            //add force to the ball plus some upward force according to the shot being played

            Vector3 ballDir = ball.position - transform.position; // get the direction of the ball compared to us to know if it is
            if (ballDir.x >= 0)                                   // on out right or left side 
            {
                animator.Play("forehand");                        // play a forhand animation if the ball is on our right
            }
            else                                                  // otherwise play a backhand animation 
            {
                animator.Play("backhand");
            }

            ball.GetComponent<Ball>().hitter = "player";
            aimTarget.position = aimTargetInitialPosition; // reset the position of the aiming gameObject to it's original position ( center)

        }
    }

    public void Reset(){
        if(servedRight){
            transform.position = serveRight.position;
        }
        else{
            transform.position = serveLeft.position;
        }
        servedRight = !servedRight;
    }


}