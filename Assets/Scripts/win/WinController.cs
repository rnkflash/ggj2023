using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public void OnStartClicked() {
        SceneController.Instance.LoadMainMenu();
    }
}
