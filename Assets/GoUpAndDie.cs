using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUpAndDie : MonoBehaviour
{
    public float timeToDie = 1.0f;
    public float distanceToUp = 10.0f;
    public Vector3 movement = Vector3.up;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;
        timeToDie -= Time.deltaTime;
        if (timeToDie <= 0) {
            Destroy(gameObject);
        }
    }
}
