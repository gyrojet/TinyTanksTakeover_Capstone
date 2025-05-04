using Unity;
using UnityEngine;

public class BaseTank : MonoBehaviour
{
    public virtual void HandleBodyMovement(Vector2 movementVector)
    {
        print("Tank Move Not assigned!");
    }

    public virtual void HandleCannonMovement(Vector2 movementVector)
    {
        print("Cannon Move Not assigned!");
    }

    public virtual void HandleShooting()
    {
        print("Shooting Not assigned!");
    }

    public virtual void HandleMines()
    {
        print("Mines Not assigned!");
    }
}
