using TD.Data.Player;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TD.Systems.Player
{
    public class PlayerTDInputSystem : JobComponentSystem, PlayerInputActions.IPlayerControlsActions
    {
        private PlayerInputActions _playerInputActions;

        private float2 _movement;
        private bool _lastFrameShot;

        protected override void OnCreate()
        {
            base.OnCreate();

            _playerInputActions = new PlayerInputActions();
            _playerInputActions.PlayerControls.SetCallbacks(this);
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Entities
                .WithAll<PlayerTag>()
                .WithoutBurst()
                .ForEach((ref PlayerMovementInputData playerMovementInputData, ref PlayerShootingInputData playerShootingInputData) =>
                {
                    playerMovementInputData._movementData = _movement;
                    playerShootingInputData._lastFrameShot = _lastFrameShot;
                }).Run();

            return default;
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            _playerInputActions.Enable();
        }

        protected override void OnStopRunning()
        {
            base.OnStopRunning();

            _playerInputActions.Disable();
        }

        #region Player Input

        public void OnMove(InputAction.CallbackContext context) => _movement = context.ReadValue<Vector2>();

        public void OnShoot(InputAction.CallbackContext context)
        {
            Debug.Log("Player Shot");

            if (_lastFrameShot)
            {
                _lastFrameShot = false;
            }
            else
            {
                _lastFrameShot = true;
            }
        }

        #endregion
    }
}