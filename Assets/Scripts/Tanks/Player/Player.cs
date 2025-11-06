using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player :  BaseTank
{
    /*
     * - Player Controller Class -
     * 
     * This class lets the player control their tank, allowing them to move and Shoot.
     * For now, I will focus on getting moving working, then focus on death and shooting.
     */

    LivesManager livesManager;
    SfxManager sfxManager;

    public static Player playerInstance;

    public AudioSource engineNoise;

    public PlayerInputHandler playerInputHandler;
    //[SerializeField] GameManager gameManager;
    [SerializeField] LevelTransmitionManager levelTransmitionManager;

    [Header("Locomotion")]

    public float maxSpeed = 10;
    public float rotationSpeed = 90;

    public float cannonRotationSpeed = 110;
    public float cannonRotationCompAngle = 90;
    public float cannonBulletFiringCompAngle = -90;

    public bool isMoving;

    //private Quaternion toAngle;

    public Vector2 movementVector;

    public int maxBullets = 5;
    public int maxMines = 2;

    [SerializeField] Rigidbody2D playerRB;

    [Header("Components")]
    [SerializeField] public GameObject tankBody;
    [SerializeField] GameObject tankCannon;

    //[SerializeField] List<ParticleSystem> treadMarkMakers;

    [SerializeField] Transform cannonParent;
    [SerializeField] Transform cannonFiringPoint;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject minePrefab;

    [SerializeField] GameObject deathMarker;
    [SerializeField] GameObject explosionPrefab;

    // Remove if performance is poor
    float trackOffset = 0.1f;
    [SerializeField] TankTrack tracks;

    public string bulletTag = "Bullet";

    [SerializeField] private List<Bullet> activeBullets = new List<Bullet>();
    [SerializeField] private List<Mine> activeMines = new List<Mine>();

    SpriteRenderer playerBodySR;
    SpriteRenderer playerCannonSR;

    RoundManager roundManager;

    [Header("SFX")]
    [SerializeField] AudioClip shoot;
    [SerializeField] AudioClip setMine;
    [SerializeField] AudioClip die;

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

        if (engineNoise == null)
            engineNoise = GetComponent<AudioSource>();

        if (levelTransmitionManager == null)
            levelTransmitionManager = LevelTransmitionManager.instance;

        if (roundManager == null)
            roundManager = RoundManager.instance;

        if (sfxManager == null)
            sfxManager = SfxManager.instance;

        if (livesManager == null)
            livesManager = LivesManager.instance;
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

        if (isMoving)
        {
            engineNoise.volume = 0.35f;
            PrintTankTrack();
        }
        else
            engineNoise.volume = 0.05f;
    }

    private void PrintTankTrack()
    {
        trackOffset -= Time.deltaTime;

        if (trackOffset <= 0)
        {
            TankTrack trackToPlace = Instantiate(tracks, tankBody.transform.position, tankBody.transform.rotation);

            trackOffset = 0.1f;
        }
        //trackToPlace.gameObject.SetActive(true);
        //trackToPlace.gameObject.transform.position = tankBody.transform.position;
        //trackToPlace.gameObject.transform.rotation = tankBody.transform.rotation;
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
        //gameManager = GameManager.gameManagerInstance;
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
            if (activeBullets.Count < maxBullets)
            {
                Bullet newBullet = Instantiate(bulletPrefab, cannonFiringPoint.position, cannonFiringPoint.rotation)
                                  .GetComponent<Bullet>();

                newBullet.owner = this.gameObject;

                newBullet.LaunchBullet(cannonFiringPoint.transform.up);

                sfxManager.PlaySFX(shoot, newBullet.transform, 1f);

                activeBullets.Add(newBullet);
            }
        }
        catch (UnityException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public override void HandleMines()
    {
        try
        {
            if (activeMines.Count < maxMines)
            {
                Debug.Log("Plop!");

                Mine newMine = Instantiate(minePrefab, tankBody.transform.position, Quaternion.identity)
                               .GetComponent<Mine>();

                sfxManager.PlaySFX(setMine, newMine.transform, 1f);

                newMine.owner = this.gameObject;

                newMine.StartExplosionCount();

                activeMines.Add(newMine);
            }
        }
        catch (UnityException ex)
        {
            print(ex.Message);
        }
    }

    public void DeathRoutine()
    {
        
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

        livesManager.DecreaseLives();

        playerInputHandler.canPlayerMove = false;
        playerRB.linearVelocity = Vector2.zero;

        tankBody.GetComponent<Collider2D>().enabled = false;

        DisableTankGraphics();

        Instantiate(explosionPrefab,
                    transform.position,
                    Quaternion.identity);

        Instantiate(deathMarker, tankBody.transform.position, Quaternion.identity);

        sfxManager.PlaySFX(die, gameObject.transform, 1f);

        print("Finished Delay!");

        roundManager.EndingSequence(true);
        
        //StartCoroutine(gameManager.ReloadWithDelay());
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

    public void RemoveMineFromList(Mine mineToRemove)
    {
        activeMines.Remove(mineToRemove);
    }
}
