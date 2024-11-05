using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Creature
{

    public enum CowState{Walk,Angry,Jumpy};
    public CowState currentState = CowState.Walk;
    public int walkSpeed = 1;
    public int angrySpeed = 10;
    public int maxDistance = 80;
    public int jumpHeigth = 10;
    public float flipSpeed = 1;
    public float flipDelay = 0.5f;
    private float angle;
    private float startY;
    private bool rest = false;
    private bool angry = false;
    public float angryTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        StateBehavior();
    }

    void FixedUpdate()
    {
        if (currentState == CowState.Walk && UnityEngine.Random.Range(0,500) == 499) {
            ChangeBehavior(CowState.Jumpy);
        } else if (currentState == CowState.Jumpy && rest && UnityEngine.Random.Range(0,125) == 124) {
            ChangeBehavior(CowState.Walk);
        }
    }

    void StateBehavior() {
        switch (currentState) {
            case CowState.Walk:
                Walk();
                break;
            case CowState.Jumpy:
                Jump();
                break;
            case CowState.Angry:
                Angry();
                break;
        }
    }

    void ChangeBehavior(CowState newstate) {
        if (currentState == CowState.Jumpy) {
            angle = 0;
            transform.eulerAngles = new Vector3(0,0,0);
            transform.position = new Vector3(transform.position.x,startY,transform.position.z);
        }

        if (newstate == CowState.Angry) {
            Invoke("UnAngry",angryTime);
        }

        currentState = newstate;
    }

    void Walk() {

        transform.Translate(Vector2.right * myDirection * walkSpeed * Time.deltaTime);

        if (transform.position.x >= maxDistance) {
            myDirection = -1;
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            myDirection = 1;
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
    }

    void Jump() {

        if (!rest) {
            angle += flipSpeed * Time.deltaTime;

            if (angle >= 180) {
                angle = 0;
                transform.eulerAngles = new Vector3(0,0,0);
                transform.position = new Vector3(transform.position.x,startY,transform.position.z);
                rest = true;
                Invoke("UnRest",flipDelay);
                return;
            }

            double radangle = angle * Math.PI/180;

            transform.eulerAngles = new Vector3(0,0,-2*angle * myDirection);

            transform.position = new Vector3(transform.position.x,(float)(Math.Sin(radangle) * jumpHeigth) + startY,transform.position.z);
        }

    }

    void Angry() {
        transform.Translate(Vector2.right * myDirection * angrySpeed * Time.deltaTime);

        if (transform.position.x >= maxDistance) {
            myDirection = -1;
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            myDirection = 1;
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
    }

    void UnRest() {
        rest = false;
        if (angry) {
            ChangeBehavior(CowState.Angry);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Piss")) {
            if (currentState != CowState.Angry && !angry) {
                TakeDamage();
                if (currentState == CowState.Jumpy) {
                    angry = true;
                } else if (currentState == CowState.Walk) {
                    ChangeBehavior(CowState.Angry);
                }
            }
            Destroy(other.gameObject);
        }
        
        if (currentState == CowState.Angry && other.gameObject.CompareTag("Fox")) {
            other.gameObject.GetComponent<Creature>().TakeDamage();
        }
    }

    void UnAngry() {
        angry = false;
        ChangeBehavior(CowState.Walk);
    }

}
