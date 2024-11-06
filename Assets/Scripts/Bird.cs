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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StateBehavior();
    }

    void StateBehavior() {
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

        if (newstate == BirdState.Pissy) {
            Invoke("Pee",0.1f);
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
        transform.Translate(Vector2.right * myDirection * FlyingSpeed * Time.deltaTime);

        if (transform.position.x >= maxDistance) {
            myDirection = -1;
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            myDirection = 1;
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
    }

    void Pee() {
        Instantiate(pissDrop,new Vector3(transform.position.x,transform.position.y,transform.position.z), new Quaternion());
        piss -= pissRate;

        if (piss <= 0) {
            ChangeBehavior(BirdState.Flying);
        } else {
            Invoke("Pee",0.1f);
        }
    }

}
