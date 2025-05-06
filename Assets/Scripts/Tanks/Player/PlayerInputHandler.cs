using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    /*
     * Player input handler that uses events to control the player's tank!
     */

    public static PlayerInputHandler Instance;

    [SerializeField] Camera mainCam;
    public bool canPlayerMove;

    public UnityEvent OnShoot = new UnityEvent();
    public UnityEvent<Vector2> OnBodyMove = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnCannonMove = new UnityEvent<Vector2>();
    public UnityEvent OnUseMines = new UnityEvent();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
        if (canPlayerMove)
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnUseMines?.Invoke();
            }
        }
    }

    private void ApplyShootingAction()
    {
        if (canPlayerMove)
        { 
            if (Input.GetMouseButtonDown(0))
            {
                OnShoot?.Invoke();
            }
        }
    }

    private void ApplyCannonMovement()
    {
        if (canPlayerMove)
            OnCannonMove?.Invoke(GetMousePos());
    }

    private void ApplyTankMovement()
    {
        if (canPlayerMove)
        {
            Vector2 inputVector = Vector2.zero;

            inputVector.x = Input.GetAxis("Horizontal");
            inputVector.y = Input.GetAxis("Vertical");

            OnBodyMove?.Invoke(inputVector);
        }
    }

    private Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = mainCam.nearClipPlane;

        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);

        return mouseWorldPos;
    }
}
