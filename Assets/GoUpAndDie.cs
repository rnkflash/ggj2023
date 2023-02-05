using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUpAndDie : MonoBehaviour
{
    public float timeToDie = 1.0f;
    public float distanceToUp = 10.0f;

    void Start()
    {
        
    }

    void Update()
    {
        timeToDie -= Time.deltaTime;
        if (timeToDie <= 0) {
            Destroy(gameObject);
        }
    }
}
