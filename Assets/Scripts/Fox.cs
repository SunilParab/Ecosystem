using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Creature
{

    public enum FoxState{Walk,Scared,Hunting,Jumping};
    public FoxState currentState = FoxState.Walk;
    public int walkSpeed = 10;
    public int scaredSpeed = 30;
    public int maxDistance = 80;
    private float hunger = 0;
    public float maxHunger = 10;
    private bool jumped = false;
    public BoxCollider2D detection;
    private float startY;
    public int jumpHeigth = 40;
    public int jumpSpeed = 5;
    public float scaredTime = 0;
    public float maxScaredTime = 5;

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

    void StateBehavior() {
        switch (currentState) {
            case FoxState.Walk:
                Walk();
                break;
            case FoxState.Scared:
                Scared();
                break;
            case FoxState.Jumping:
                Jump();
                break;
        }
    }

    void ChangeBehavior(FoxState newstate) {
        if (newstate == FoxState.Scared) {
            scaredTime = 0;
        }
        if (newstate == FoxState.Jumping) {
            hunger = 0;
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

        scaredTime += Time.deltaTime;
        if (scaredTime >= maxScaredTime) {
            ChangeBehavior(FoxState.Walk);
        }

    }

    void Jump() {
        if (!jumped) {
            transform.Translate(Vector2.up * jumpSpeed * Time.deltaTime);
            if (transform.position.y > jumpHeigth) {
                jumped = true;
            }
        } else {
            transform.Translate(Vector2.up * -jumpSpeed * Time.deltaTime);
            if (transform.position.y < startY) {
                jumped = false;
                transform.position = new Vector3(transform.position.x,startY,transform.position.z);
                ChangeBehavior(FoxState.Walk);
            }
        }
    }

    public override void TakeDamage() {

        if (currentState == FoxState.Walk || currentState == FoxState.Hunting) {
            ChangeBehavior(FoxState.Scared);
        }

        if (currentState != FoxState.Scared) {
            health--;
            if (health <= 0) {
                Destroy(gameObject);
            }
        }

    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Bird")) {
            print("hi");
            if (other.otherCollider == detection) {
                if (currentState == FoxState.Hunting) {
                    ChangeBehavior(FoxState.Jumping);
                }
            } else {
                Destroy(other.gameObject);
            }
        }
    }

}
