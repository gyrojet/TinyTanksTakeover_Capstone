using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    Tank playerTank;

    public UnityEvent OnShoot = new UnityEvent();
    public UnityEvent<Vector2> OnBodyMove = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnTurretMove = new UnityEvent<Vector2>();
    public UnityEvent OnLayMine = new UnityEvent();

    private void Awake()
    {
        // Get Reference to tank class
        playerTank = GetComponent<Tank>();
    }

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

        mousePos.z = Camera.main.nearClipPlane;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        return mouseWorldPos;
    }
}
