using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        //SoundSystem.ChangeTrack(Sounds.Instance.GetAudioClip("georgian_folk_rap"));
    }

    void Update()
    {
        
    }

    public void OnStartClicked() {
        SceneController.Instance.LoadGameplay();
    }
}
