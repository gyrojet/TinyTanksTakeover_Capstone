using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player :  BaseTank
{
    /*
     * - Player Controller Class -
     * 
     * This class lets the player control their tank, allowing them to move and Shoot.
     * For now, I will focus on getting moving working, then focus on death and shooting.
     */

    public static Player playerInstance;

    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] GameManager gameManager;

    [Header("Locomotion")]

    public float maxSpeed = 10;
    public float rotationSpeed = 90;

    public float cannonRotationSpeed = 110;
    public float cannonRotationCompAngle = 90;
    public float cannonBulletFiringCompAngle = -90;

    public bool isMoving;

    //private Quaternion toAngle;

    Vector2 movementVector;

    [SerializeField] Rigidbody2D playerRB;

    [Header("Components")]
    [SerializeField] public GameObject tankBody;
    [SerializeField] GameObject tankCannon;

    //[SerializeField] List<ParticleSystem> treadMarkMakers;

    [SerializeField] Transform cannonParent;
    [SerializeField] Transform cannonFiringPoint;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject minePrefab;

    public string bulletTag = "Bullet";

    [SerializeField] private List<Bullet> activeBullets = new List<Bullet>();

    SpriteRenderer playerBodySR;
    SpriteRenderer playerCannonSR;

    private void Awake()
    {
        if (playerInstance == null)
            playerInstance = this;
    }

    private void Start()
    {
        // Get the required components
        GetComponents();

        if (playerInputHandler.canPlayerMove == false)
        {
            playerInputHandler.canPlayerMove = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 newMovementVector = (Vector2)tankBody.transform.up * movementVector.y * maxSpeed * Time.fixedDeltaTime;

        playerRB.linearVelocity = newMovementVector;

        playerRB.MoveRotation(tankBody.transform.rotation *
            Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));

        if (playerRB.linearVelocity != Vector2.zero)
            isMoving = true;
        else
            isMoving = false;

        //HandleTrails();
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

        playerInputHandler = PlayerInputHandler.Instance;
        gameManager = GameManager.gameManagerInstance;
    }

    private void HandleTrails()
    {
        //foreach(ParticleSystem tankTread in treadMarkMakers)
        //{
        //    if(isMoving)
        //        tankTread.Play();
        //    else
        //        tankTread.Stop();
        //}
    }
    public override void HandleBodyMovement(Vector2 movementVector)
    {
        this.movementVector = movementVector;
    }

    public override void HandleCannonMovement(Vector2 mousePos)
    {
        //Debug.Log($"Mouse moved: Position is {mousePos}");
        var cannonDirection = (Vector3)mousePos - cannonParent.position;

        var angleOfRotation = Mathf.Atan2(cannonDirection.y, cannonDirection.x) * Mathf.Rad2Deg;

        var roatationStep = cannonRotationSpeed * Time.deltaTime;

        var desiredAngle = Quaternion.Euler(0, 0, angleOfRotation - cannonRotationCompAngle);

        var firingPointAngle = Quaternion.Euler(0, 0, angleOfRotation + cannonRotationCompAngle);

        cannonParent.rotation = 
            Quaternion.RotateTowards(tankCannon.transform.rotation,
                                     desiredAngle,
                                     roatationStep);

        cannonFiringPoint.rotation =
            Quaternion.RotateTowards(tankCannon.transform.rotation,
                                     firingPointAngle,
                                     roatationStep);
    }

    public override void HandleShooting()
    {
        try
        {
            Bullet newBullet = Instantiate(bulletPrefab, cannonFiringPoint.position, cannonFiringPoint.rotation)
                              .GetComponent<Bullet>();

            newBullet.owner = this.gameObject;

            newBullet.LaunchBullet(cannonFiringPoint.transform.up);

            activeBullets.Add(newBullet);
        }
        catch (UnityException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public override void HandleMines()
    {
        Debug.Log("Plop!");

        Mine newMine = Instantiate(minePrefab, tankBody.transform.position, Quaternion.identity)
                       .GetComponent<Mine>();

        newMine.StartExplosionCount();
    }

    public void DeathRoutine()
    {
        print("KillTank called!");
        KillPlayer();
    }

    private void DisableTankGraphics()
    {
        tankBody.GetComponent<SpriteRenderer>().enabled = false;
        tankCannon.GetComponent<SpriteRenderer>().enabled = false;
    }


    /// <summary>
    /// Testing the death function: This may be removed later.
    /// </summary>
    /// <returns></returns>
    private void KillPlayer()
    {
        print("Die called!");

        playerInputHandler.canPlayerMove = false;

        DisableTankGraphics();

        Instantiate(Resources.Load<GameObject>("Prefabs/Effects/Explosion"),
                    transform.position,
                    Quaternion.identity);

        print("Finished Delay!");

        StartCoroutine(gameManager.ReloadWithDelay());
    }

    public void RemoveBulletFromList(Bullet bulletToRemove)
    {
        try
        {
            activeBullets.Remove(bulletToRemove);
        }
        catch (UnityException ex)
        {
            print(ex.Message);
        }
    }
}
