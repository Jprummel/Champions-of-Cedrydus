/*
	ControlGameState.cs
	Created 11/10/2017 1:36:53 PM
	Project Boardgame by Base Games
*/
using UnityEngine;

namespace Utility
{
	public class ControlGameState : MonoBehaviour 
	{
        private bool _isQuitting;

        private void OnEnable()
        {
            GameStateManager.instance.ChangeTimeState(GameState.PAUSED);
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void OnDisable()
        {
            if(!_isQuitting)
                GameStateManager.instance.ChangeTimeState(GameState.PLAYING);
        }
    }
}