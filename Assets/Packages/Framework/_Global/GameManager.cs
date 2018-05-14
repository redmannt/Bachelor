﻿using AI.GOAP;
using Framework;
using Framework.Debugging;
using Framework.Messaging;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains game logic
/// </summary>
public sealed class GameManager : SingletonAsComponent<GameManager>
{
    #region Variables

    private GAME_STATE _gameState = GAME_STATE.DEFAULT;
    private GAME_STATE _oldState = GAME_STATE.DEFAULT;

    #endregion

    #region Properties

    public static GameManager Instance
    {
        get { return (GameManager)_Instance; }
    }

    public GAME_STATE GameState
    {
        get { return _gameState; }
    }

    public GAME_STATE OldState
    {
        get { return _oldState; }
    }

    #endregion

    /// <summary>
    /// Sets a new GameState
    /// </summary>
    /// <param name="newState">New GameState</param>
    public void SetGameState(GAME_STATE newState)
    {
        if (_gameState != newState)
        {
            _oldState = _gameState;
            _gameState = newState;
            OnGameStateChange(newState);
        }
        else
        {
            Debugger.LogFormat(LOG_TYPE.LOG,
                "GameState is already '{0}'!\n",
                newState);
        }
    }

    /// <summary>
    /// Called when the GameState changes
    /// </summary>
    /// <param name="newState">The new state</param>
    private void OnGameStateChange(GAME_STATE newState)
    {
        switch (newState)
        {
            case GAME_STATE.MAIN_SCENE:
                if (SceneManager.GetActiveScene().buildIndex != 1)
                    SceneManager.LoadScene(1);

                break;

            case GAME_STATE.TOWN:
                if (SceneManager.GetActiveScene().buildIndex != 2)
                    SceneManager.LoadScene(2);

                break;

            case GAME_STATE.SHUTDOWN:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                UnityEngine.Application.Quit();
#endif
                break;

            default:
                Debugger.LogFormat(LOG_TYPE.WARNING,
                    "GameState '{0}' not implemented!\n",
                    newState);
                break;
        }
    }

    public override void WakeUp()
    {
        SceneManager.activeSceneChanged += (a, b) =>
        {
            TimeManager.Instance.Clear();
            GC.Collect();
        };

        if (!MessagingSystem.IsAlive)
            MessagingSystem.Instance.WakeUp();

        if (!TimeManager.IsAlive)
            TimeManager.Instance.WakeUp();

        GOAPContainer.Init();
        GOAPReader.ReadAll();
    }
}