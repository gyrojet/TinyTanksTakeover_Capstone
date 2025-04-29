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
    public float cannonRotationCompAngle = 90;
    public float cannonBulletFiringCompAngle = -90;

    bool isMoving;

    private Quaternion toAngle;

    Vector2 movementVector;

    [SerializeField] Rigidbody2D playerRB;

    [Header("Components")]
    [SerializeField] GameObject tankBody;
    [SerializeField] GameObject tankCannon;

    [SerializeField] Transform cannonParent;
    [SerializeField] Transform cannonFiringPoint;

    [SerializeField] GameObject bulletPrefab;

    SpriteRenderer playerBodySR;
    SpriteRenderer playerCannonSR;

    private void Awake()
    {
        // Get the required components
        GetComponents();
    }

    private void FixedUpdate()
    {
        Vector2 newMovementVector = (Vector2)tankBody.transform.up * movementVector.y * maxSpeed * Time.fixedDeltaTime;

        playerRB.linearVelocity = newMovementVector;

        if (playerRB.linearVelocity != Vector2.zero)
            isMoving = true;
        else
            isMoving = false;

        if (isMoving)
            playerRB.MoveRotation(tankBody.transform.rotation *
                Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));

        //Debug.Log(tankBody.transform.rotation);
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

        //if (bulletPrefab == null)
        //{
        //    Debug.Log("Loading Prefab Bullet...");
        //    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        //}
    }

    public void HandleBodyMovement(Vector2 movementVector)
    {
        this.movementVector = movementVector;
    }

    public void HandleCannonMovement(Vector2 mousePos)
    {
        //Debug.Log($"Mouse moved: Position is {mousePos}");
        var cannonDirection = (Vector3)mousePos - cannonParent.position;

        var angleOfRotation = Mathf.Atan2(cannonDirection.y, cannonDirection.x) * Mathf.Rad2Deg;

        var roatationStep = cannonRotationSpeed * Time.deltaTime;

        toAngle = Quaternion.Euler(0, 0, angleOfRotation - cannonRotationCompAngle);

        var firingPointAngle = Quaternion.Euler(0, 0, angleOfRotation + cannonRotationCompAngle);

        cannonParent.rotation = 
            Quaternion.RotateTowards(tankCannon.transform.rotation,
                                     toAngle,
                                     roatationStep);

        cannonFiringPoint.rotation =
            Quaternion.RotateTowards(tankCannon.transform.rotation,
                                     firingPointAngle,
                                     roatationStep);
    }

    public void HandleShooting()
    {
        try
        {
            Bullet newBullet = Instantiate(bulletPrefab, cannonFiringPoint.position, Quaternion.identity)
                              .GetComponent<Bullet>();

            //     Problem: Bullet is not rotating! Goes either up or down.
            //     Maybe ask Darren if he is avalible? If you can't figure it out by today work on it tommorow.

            newBullet.LaunchBullet(cannonFiringPoint.transform.up);
        }
        catch (UnityException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void HandleMines()
    {
        Debug.Log("Plop!");
    }
}
