using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Bird : Creature
{

    public enum BirdState{Flying, Pissy, Panic};
    public BirdState currentState = BirdState.Flying;
    public int FlyingSpeed = 1;
    public int maxDistance = 80;
    private float piss = 0;
    public float bladder = 1;
    public GameObject pissDrop;
    public float pissGrowth = 1;
    public float pissRate = 0.2f;
    public static bool panicking = false;
    public static GameObject panicSource;
    private int ranDir;
    public AudioSource panicSound;
    public static bool panicPlayed = true;

    // Start is called before the first frame update
    void Start()
    {
        piss = UnityEngine.Random.Range(0,bladder);
    }

    // Update is called once per frame
    void Update()
    {
        if (!panicPlayed && panicSource != this.gameObject) {
            panicPlayed = true;
            panicSound.Play();
        }
        StateBehavior();
    }

    void StateBehavior() {

        if (panicking && currentState != BirdState.Panic) {
            ChangeBehavior(BirdState.Panic);
        } else if (!panicking && currentState == BirdState.Panic) {
            ChangeBehavior(BirdState.Flying);
        }

        switch (currentState) {
            case BirdState.Flying:
                Flying();
                break;
            case BirdState.Pissy:
                Pissy();
                break;
            case BirdState.Panic:
                Panic();
                break;
        }
    }

    void ChangeBehavior(BirdState newstate) {

        if (currentState == BirdState.Panic) {
            if (myDirection == -1) {
                transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
            } else if (transform.position.x <= -maxDistance) {
                transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
            }
        }

        if (newstate == BirdState.Pissy) {
            Invoke("Pee",0.1f);
        }
        if (newstate == BirdState.Panic) {
            NewRanDir();
        }

        currentState = newstate;
    }

    void Flying() {

        transform.Translate(Vector2.right * myDirection * FlyingSpeed * Time.deltaTime);

        if (transform.position.x >= maxDistance) {
            myDirection = -1;
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            myDirection = 1;
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }

        piss += Time.deltaTime * pissGrowth;

        if (piss > bladder) {
            piss = bladder;
            ChangeBehavior(BirdState.Pissy);
        }

    }

    void Pissy() {

        transform.Translate(Vector2.right * myDirection * FlyingSpeed * Time.deltaTime);

        if (transform.position.x >= maxDistance) {
            myDirection = -1;
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            myDirection = 1;
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }

    }

    void Panic() {

        

        transform.Translate(Vector2.right * ranDir * FlyingSpeed * 2.5f * Time.deltaTime);

        if (ranDir < 0) {
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
    }

    void Pee() {
        Instantiate(pissDrop,new Vector3(transform.position.x,transform.position.y,transform.position.z), new Quaternion());
        piss -= pissRate;

        if (piss <= 0) {
            if (currentState != BirdState.Panic) {
                ChangeBehavior(BirdState.Flying);
            }
        } else {
            if (currentState == BirdState.Pissy && health > 0) {
                Invoke("Pee",0.1f);
            }
        }
    }

    public override void TakeDamage() {
        health--;
        if (health <= 0) {
            panicking = true;
            panicPlayed = false;
            gameObject.SetActive(false);
            panicSource = this.gameObject;
            Invoke("Unpanic",2);
        }
    }

    public void Unpanic() {
        if (panicSource == this.gameObject) {
            panicking = false;
        }
        Destroy(gameObject);
    }

    void NewRanDir() {
        ranDir = UnityEngine.Random.Range(0,2)*2-1;
        if (currentState == BirdState.Panic) {
            Invoke("NewRanDir",0.2f);
        }
    }

}
