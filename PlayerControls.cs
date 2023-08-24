using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerControls : MonoBehaviour
{
    private MyControlScheme _inputControl;
    public delegate void MovementHandler(Vector2 aVector2);
    public static event MovementHandler OnMovementDirectionChange;
    public delegate void ShootingHandler();
    public static event ShootingHandler OnShootButton;
    public delegate void PortalHandler(bool aStarted);
    public static event PortalHandler OnPortalButton;

    private void Awake()
    { 
        _inputControl = new MyControlScheme();
    }

    private void OnEnable()
    {
        _inputControl.PlayerControls.Enable();
        _inputControl.PlayerControls.PlayerMovement.performed += ReadMovementDirection;
        _inputControl.PlayerControls.PlayerFire.started += value => ReceiveShootingDirectives();

        _inputControl.PlayerControls.PlayerPortalPlacement.started += value => ReceivePortalPlacementDirectives(true);
        _inputControl.PlayerControls.PlayerPortalPlacement.canceled += value => ReceivePortalPlacementDirectives(false);

        // temporarily in player controls
        _inputControl.PlayerControls.RestartLevel.started += value => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _inputControl.PlayerControls.QuitGame.started += value => Application.Quit();
    }

    private void OnDisable()
    {
        _inputControl.PlayerControls.Disable();
        _inputControl.PlayerControls.PlayerMovement.performed -= ReadMovementDirection;
        _inputControl.PlayerControls.PlayerFire.started -= value => ReceiveShootingDirectives();

        _inputControl.PlayerControls.PlayerPortalPlacement.started -= value => ReceivePortalPlacementDirectives(true);
        _inputControl.PlayerControls.PlayerPortalPlacement.canceled -= value => ReceivePortalPlacementDirectives(false);

        // temporarily in player controls
        _inputControl.PlayerControls.RestartLevel.started -= value => SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        _inputControl.PlayerControls.QuitGame.started -= value => Application.Quit();
    }

    private void ReadMovementDirection(InputAction.CallbackContext aCallbackContext)
    { 
        OnMovementDirectionChange?.Invoke(aCallbackContext.ReadValue<Vector2>());
    }

    private void ReceiveShootingDirectives()
    { OnShootButton?.Invoke(); }

    private void ReceivePortalPlacementDirectives(bool aPortalPlacementBool)
    { OnPortalButton?.Invoke(aPortalPlacementBool); }
}
