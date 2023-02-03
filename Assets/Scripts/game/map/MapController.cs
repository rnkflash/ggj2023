using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync("map", LoadSceneMode.Additive);
    }
}
