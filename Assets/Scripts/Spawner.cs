using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Cow;
    public GameObject Fox;
    public GameObject Bird;
    public int groundTop = -10;
    public int groundBot = -40;
    public int skyTop = 45;
    public int skyBot = 15;
    public int maxDistance = 80;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Cow,new Vector3(Random.Range(-maxDistance,maxDistance),Random.Range(groundBot,groundTop),0), new Quaternion());
        Instantiate(Fox,new Vector3(Random.Range(-maxDistance,maxDistance),Random.Range(groundBot,groundTop),0), new Quaternion());
        Instantiate(Bird,new Vector3(Random.Range(-maxDistance,maxDistance),Random.Range(skyBot,skyTop),0), new Quaternion());
        InvokeRepeating("Spawn",1f,3f);
    }

    void Spawn() {
        int rando = Random.Range(0,4);
        switch (rando) {
            case 0:
                Instantiate(Cow,new Vector3(Random.Range(-maxDistance,maxDistance),Random.Range(groundBot,groundTop),0), new Quaternion());
                break;
            case 1:
                Instantiate(Fox,new Vector3(Random.Range(-maxDistance,maxDistance),Random.Range(groundBot,groundTop),0), new Quaternion());
                break;
            case 2:
                Instantiate(Bird,new Vector3(Random.Range(-maxDistance,maxDistance),Random.Range(skyBot,skyTop),0), new Quaternion());
                break;
            case 3:
                Instantiate(Bird,new Vector3(Random.Range(-maxDistance,maxDistance),Random.Range(skyBot,skyTop),0), new Quaternion());
                break;
        }
    }
}
