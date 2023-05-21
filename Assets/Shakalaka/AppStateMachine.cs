using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Shakalaka
{
    [CreateAssetMenu(menuName = "Shakalaka/AppStateMachine", fileName = "AppStateMachine")]
    public class AppStateMachine : SerializedScriptableObject
    {
        [OdinSerialize] private Dictionary<AppStateType, AppState> _states = new Dictionary<AppStateType, AppState>();
        [SerializeField] private AppStateType initialState;

        private AppState _currentState = null;

        private AppScope _appScope;

        public async UniTaskVoid Launch(AppScope appScope)
        {
            _appScope = appScope;
            _currentState = null;
            
            foreach (var state in _states)
            {
                state.Value.Init(this, _appScope);
            }
            await TransitionTo(initialState);
        }

        public async UniTask TransitionTo(AppStateType newStateType)
        {
            if (_states.TryGetValue(newStateType, out var newState))
            {
                await TransitionTo(newState);
            }
            else
            {
                Debug.LogError($"There is no state of type {newStateType} in AppStateMachine!");
            }
        }

        private async UniTask TransitionTo(AppState newState)
        {
            if (newState == null)
            {
                Debug.LogError($"Trying to transition to a null state!");
                return;
            }

            if (_currentState != null)
                await _currentState.Exit();

            await newState.Enter();
            _currentState = newState;
        }
    }
}