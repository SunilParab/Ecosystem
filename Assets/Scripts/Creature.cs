using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public int health = 3;
    public int myDirection = 1;

    public virtual void TakeDamage() {
        health--;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

}
