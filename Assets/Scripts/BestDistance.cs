using UnityEngine;

public class BestDistance
{
    private Collider2D collider;
    private float distance;

    public Collider2D Collider { get { return collider; } }
    public float Distance { get { return distance; } }

    public BestDistance(Collider2D collider, float distance)
    {
        this.collider = collider;
        this.distance = distance;
    }
}
