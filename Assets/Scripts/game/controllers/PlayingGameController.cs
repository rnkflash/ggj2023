using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingGameController : MonoBehaviour
{
    public enum State {
        Start,
        Exit
    }

    private State state;
    
    public IEnumerator StartPlaying() {
        state = State.Start;
        
        while (state != State.Exit)
        {
            yield return null;
        }
    }

}
