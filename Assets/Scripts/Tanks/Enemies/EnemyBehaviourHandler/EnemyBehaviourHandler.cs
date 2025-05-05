using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviourHandler : MonoBehaviour
{
    //public EnemyTank enemyl;

    public bool canMove;
    public bool canShootBullets;
    public bool canUseMines;

    public UnityEvent OnShoot = new UnityEvent();
    public UnityEvent OnUseMines = new UnityEvent();

    public UnityEvent<Vector2> OnBodyMove = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnCannonMove = new UnityEvent<Vector2>();

    private void Update()
    {
        ApplyMovement();
        ApplyCannonMovement();
        ApplyShootingAction();
        ApplyMineAction();
    }

    private void ApplyMovement()
    {
        if (canMove)
        {
            // Do whatever! IDK yet!
        }
    }

    private void ApplyCannonMovement()
    {
        if (canMove)
        {
            // OnCannonMove?.Invoke()
        }
    }

    private void ApplyShootingAction()
    {
        if (canShootBullets)
        {

        }
    }

    private void ApplyMineAction()
    {
        if (canUseMines)
        {

        }
    }
}
