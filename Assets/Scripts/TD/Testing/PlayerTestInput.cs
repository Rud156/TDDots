using UnityEngine;
using UnityEngine.InputSystem;

namespace Testing
{
    public class PlayerTestInput : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        private Vector2 _movement;

        #region Unity Functions

        private void Awake()
        {
            _inputActions = new PlayerInputActions();

            _inputActions.PlayerControls.Move.performed += HandlePlayerMoved;
            _inputActions.PlayerControls.Shoot.performed += HandlePlayerShot;
        }

        private void OnEnable() => _inputActions.Enable();

        private void OnDisable() => _inputActions.Disable();

        #endregion

        #region Utility Functions

        private void HandlePlayerMoved(InputAction.CallbackContext obj)
        {
            _movement = obj.ReadValue<Vector2>();
            Debug.Log($"Player Movement: {_movement}");
        }

        private void HandlePlayerShot(InputAction.CallbackContext obj) => Debug.Log("Player Shot");

        #endregion
    }
}