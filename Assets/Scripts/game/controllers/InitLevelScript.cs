using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitLevelScript : MonoBehaviour
{
    void Start()
    {
        Player.Instance.map = SceneManager.GetActiveScene().name;
        Debug.Log("loaded map " + Player.Instance.map);
    }
}
