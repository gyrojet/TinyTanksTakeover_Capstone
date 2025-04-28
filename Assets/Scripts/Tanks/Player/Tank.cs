using UnityEngine;

public class Tank : MonoBehaviour
{
    /*
     * - Player Controller Class -
     * 
     * This class lets the player control their tank, allowing them to move and Shoot.
     * For now, I will focus on getting moving working, then focus on death and shooting.
     */

    [Header("Locomotion")]

    public float maxSpeed = 10;
    public float rotationSpeed = 90;
    public float cannonRotationSpeed = 110;

    public float cannonRotationOffset = 10;

    Vector2 movementVector;

    [SerializeField] Rigidbody2D playerRB;

    [Header("Components")]
    [SerializeField] GameObject tankBody;
    [SerializeField] GameObject tankCannon;

    [SerializeField] Transform cannonParent;

    SpriteRenderer playerBodySR;
    SpriteRenderer playerCannonSR;

    private void Awake()
    {
        // Get the required components
        GetComponents();
    }

    private void FixedUpdate()
    {
        Vector2 newMovementVector = (Vector2)transform.up * movementVector.y * maxSpeed * Time.fixedDeltaTime;

        playerRB.AddForce(newMovementVector);

        //playerRB.linearVelocity = newMovementVector;

        playerRB.MoveRotation(tankBody.transform.rotation *
            Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));
    }

    private void GetComponents()
    {
        if (tankBody != null)
        {
            playerBodySR = tankBody.GetComponent<SpriteRenderer>();
            playerRB = tankBody.GetComponent<Rigidbody2D>();
        }

        if (tankCannon != null)
            playerCannonSR = tankCannon.GetComponent<SpriteRenderer>();
    }

    public void HandleBodyMovement(Vector2 movementVector)
    {
        this.movementVector = movementVector;
    }

    public void HandleCannonMovement(Vector2 mousePos)
    {


        var cannonDirection = (Vector3)mousePos - cannonParent.position;

        var angleOfRotation = Mathf.Atan2(cannonDirection.y, cannonDirection.x) * Mathf.Rad2Deg;

        var roatationStep = cannonRotationSpeed * Time.deltaTime;

        tankCannon.transform.rotation = 
            Quaternion.RotateTowards(cannonParent.rotation,
                                     Quaternion.Euler(0, 0, angleOfRotation),
                                     roatationStep);
    }

    public void HandleShooting()
    {
        Debug.Log("BANG!");
    }

    public void HandleMines()
    {
        Debug.Log("Plop!");
    }
}
