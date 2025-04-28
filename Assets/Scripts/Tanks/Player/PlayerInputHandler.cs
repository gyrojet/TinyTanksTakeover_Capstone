using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    /*
     * Player input handler that uses events to control the player's tank!
     */

    [SerializeField] Camera mainCam;

    public UnityEvent OnShoot = new UnityEvent();
    public UnityEvent<Vector2> OnBodyMove = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnTurretMove = new UnityEvent<Vector2>();
    public UnityEvent OnLayMine = new UnityEvent();

    private void Update()
    {
        ApplyTankMovement();
        ApplyCannonMovement();
        ApplyShootingAction();
        ApplyMineAction();
    }

    private void ApplyMineAction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnLayMine?.Invoke();
        }
    }

    private void ApplyShootingAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnShoot?.Invoke();
        }
    }

    private void ApplyCannonMovement()
    {
        OnTurretMove?.Invoke(GetMousePos());
        
    }

    private void ApplyTankMovement()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        OnBodyMove?.Invoke(inputVector);
    }

    private Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = mainCam.nearClipPlane;

        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);

        return mouseWorldPos;
    }
}
