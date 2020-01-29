using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TD.Managers.MainScene.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        private Vector2 _playerMovement;
        private bool _lastFrameShot;

        #region Unity Functions

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.PlayerControls.Move.performed += HandlePlayerMoved;
            _playerInputActions.PlayerControls.Shoot.performed += HandlePlayerShot;
        }

        private void Update()
        {
            // Reset Data for Shooting
            _lastFrameShot = false;
        }

        private void OnEnable() => _playerInputActions.Enable();

        private void OnDisable() => _playerInputActions.Disable();

        private void OnDestroy()
        {
            _playerInputActions.PlayerControls.Move.performed -= HandlePlayerMoved;
            _playerInputActions.PlayerControls.Shoot.performed -= HandlePlayerShot;
        }

        #endregion

        #region External Functions

        public float2 GetMovement() => _playerMovement;

        public bool GetShooting() => _lastFrameShot;

        #endregion

        #region Utility Functions

        private void HandlePlayerMoved(InputAction.CallbackContext obj) => _playerMovement = obj.ReadValue<Vector2>();

        private void HandlePlayerShot(InputAction.CallbackContext obj) => _lastFrameShot = true;

        #endregion
    }
}