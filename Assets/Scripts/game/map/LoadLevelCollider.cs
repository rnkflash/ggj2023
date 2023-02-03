using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelCollider : MonoBehaviour
{
    public string level;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Player.Instance.map != level) {
            if (other.tag == "Player") {
                var offset = 5.0f;
                var coords = other.transform.position;
                var collider = GetComponent<Collider2D>();
                var closest = collider.bounds.ClosestPoint(coords);
                var direction = (closest - coords).normalized;
                
                Player.Instance.entrancePosition = new Vector3(
                    other.transform.position.x + direction.x * offset, 
                    other.transform.position.y + direction.y * offset, 
                    other.transform.position.z
                );
                SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
            }
        }
    }
}
