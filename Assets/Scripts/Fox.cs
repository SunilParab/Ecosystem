using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Creature
{

    public enum FoxState{Walk,Scared,Hunting,Resting};
    public FoxState currentState = FoxState.Walk;
    public int walkSpeed = 10;
    public int scaredSpeed = 30;
    public int maxDistance = 80;
    private float hunger = 0;
    public float maxHunger = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StateBehavior();
    }

    void FixedUpdate()
    {

    }

    void StateBehavior() {
        switch (currentState) {
            case FoxState.Walk:
                Walk();
                break;
            case FoxState.Scared:
                Scared();
                break;
        }
    }

    void ChangeBehavior(FoxState newstate) {
        if (currentState == FoxState.Walk) {

        }

        currentState = newstate;
    }

    void Walk() {

        hunger += Time.deltaTime;

        if (hunger > maxHunger) {
            ChangeBehavior(FoxState.Hunting);
        }

        transform.Translate(Vector2.right * myDirection * walkSpeed * Time.deltaTime);

        if (transform.position.x >= maxDistance) {
            myDirection = -1;
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            myDirection = 1;
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
    }

    void Scared() {

        hunger += Time.deltaTime;

        transform.Translate(Vector2.right * myDirection * scaredSpeed * Time.deltaTime);

        if (transform.position.x >= maxDistance) {
            myDirection = -1;
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        } else if (transform.position.x <= -maxDistance) {
            myDirection = 1;
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
    }

    public override void TakeDamage() {

        ChangeBehavior(FoxState.Scared);

        health--;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

}
