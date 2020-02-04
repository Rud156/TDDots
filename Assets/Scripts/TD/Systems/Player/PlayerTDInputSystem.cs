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
        private bool _lastFrameBoosted;

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
                .ForEach((ref PlayerMovementInputData playerMovementInputData,
                    ref PlayerShootingInputData playerShootingInputData) =>
                {
                    playerMovementInputData._movementData = _movement;
                    playerMovementInputData._boostPressedLastFrame = _lastFrameBoosted;
                    playerShootingInputData._lastFrameShot = _lastFrameShot;

                    _lastFrameBoosted = false;
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
            // We only want a single action for each button pressed
            if (!context.performed)
            {
                return;
            }

            _lastFrameShot = !_lastFrameShot;
        }

        public void OnBoost(InputAction.CallbackContext context)
        {
            // We only want a single action for each button pressed
            if (!context.performed)
            {
                return;
            }

            _lastFrameBoosted = true;
        }

        #endregion
    }
}