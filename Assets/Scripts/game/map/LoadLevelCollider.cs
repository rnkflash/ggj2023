using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelCollider : MonoBehaviour
{
    public string level;
    public Vector2 offset = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Player.Instance.map != level) {
            if (other.tag == "Player") {
                Player.Instance.entrancePosition = new Vector3(other.transform.position.x + offset.x, other.transform.position.y + offset.y, other.transform.position.z);
                SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
            }
        }
    }
}
