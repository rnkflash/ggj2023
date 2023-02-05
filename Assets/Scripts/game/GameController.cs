using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    Start,
    Win,
    Lose,
    Exit
}

public class GameController : MonoBehaviour
{
    private GameState state;

    private PlayingGameController playingGameController;

    void Awake() {
        state = GameState.Start;
        playingGameController = GetComponent<PlayingGameController>();

        EventBus<DeathMessage>.Sub(OnDeathMessage);
    }

    private void OnDeathMessage(DeathMessage message)
    {
        SceneController.Instance.LoadDeathScreen();
    }

    void OnDestroy() {
        EventBus<DeathMessage>.Unsub(OnDeathMessage);
    }
    
    private IEnumerator Start()
    {
        while (true)
        {
            switch (state)
            {
                case GameState.Start:
                    yield return playingGameController.StartPlaying();
                    break;
                case GameState.Win:
                    state = GameState.Exit;
                    break;
                case GameState.Lose:
                    state = GameState.Exit;
                    break;
                case GameState.Exit:
                    SceneController.Instance.LoadMainMenu();
                    yield break;
            }
        }
    }
}
