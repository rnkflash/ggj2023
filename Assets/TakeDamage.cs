using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int health = 100;
    public GameObject damageNumbersPrefab;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Hit(int damage) {
        var obj = Instantiate(damageNumbersPrefab, new Vector3(transform.position.x, transform.position.y, -2.0f), transform.rotation);
        var text = obj.GetComponent<TMPro.TMP_Text>();
        text.text = damage.ToString();

        health -= damage;
        if (health <= 0)
        {
		    Destroy(gameObject);
        }
    }
}
