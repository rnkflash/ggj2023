using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonController : MonoBehaviour
{
    public void OnButtonClicked()
    {
         SceneController.Instance.LoadMainMenu();
    }
}
