using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseTank
{
    [SerializeField] EnemyBehaviourHandler enemyBehaviour;
    //[SerializeField] GameManager gameManager;

    SfxManager sfxManager;
    RoundManager roundManager;

    [Header("Locomotion")]

    public float maxSpeed = 10;
    public float rotationSpeed = 90;

    public float cannonRotationSpeed = 110;
    public float cannonRotationCompAngle = 90;
    public float cannonBulletFiringCompAngle = -90;

    public float visibilityRadius = 10;

    public bool isMoving;
    public bool isPlayerVisible;

    [Header("Combat")]

    [SerializeField] Rigidbody2D enemyRB;
    [SerializeField] float shootingDelay = 4f;

    public IEnumerator shootBullet;

    [Header("Components")]
    
    [SerializeField] public GameObject tankBody;
    [SerializeField] GameObject tankCannon;

    //[SerializeField] List<ParticleSystem> treadMarkMakers;
    [SerializeField] public Transform cannonParent;
    [SerializeField] public Transform cannonFiringPoint;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject minePrefab;

    [SerializeField] GameObject deathMarkerPrefab;

    [Header("SFX")]
    [SerializeField] AudioClip shoot;
    [SerializeField] AudioClip die;

    public LayerMask playerLayerMask;

    public string bulletTag = "Bullet";

    [SerializeField] private List<Bullet> activeBullets = new List<Bullet>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponents();

        shootBullet = Shoot();
    }

    // Update is called once per frame
    

    private void GetComponents()
    {
        if (tankBody != null)
        {
            enemyRB = tankBody.GetComponent<Rigidbody2D>();
        }

        if (enemyBehaviour == null)
        {
            enemyBehaviour = gameObject.GetComponent<EnemyBehaviourHandler>();
        }

        if (roundManager == null)
        {
            roundManager = RoundManager.instance;
        }

        if (sfxManager == null)
        {
            sfxManager = SfxManager.instance;
        }

        //gameManager = GameManager.gameManagerInstance;
    }

    public override void HandleBodyMovement(Vector2 movementVector)
    {
        // Replace this later...
        base.HandleBodyMovement(movementVector);
    }

    public override void HandleCannonMovement(Vector2 movementVector)
    {
        var cannonDirection = (Vector3)movementVector - cannonParent.position;

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

    public override void HandleMines()
    {
        base.HandleMines();
    }

    public override void HandleShooting()
    {
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        print("SHOOT CALLED");
        enemyBehaviour.canShootBullets = false;

        yield return new WaitForSeconds(shootingDelay);

        Bullet newBullet = Instantiate(bulletPrefab, cannonFiringPoint.position, cannonFiringPoint.rotation)
                          .GetComponent<Bullet>();

        newBullet.owner = this.gameObject;

        newBullet.LaunchBullet(cannonFiringPoint.transform.up);

        sfxManager.PlaySFX(shoot, cannonFiringPoint.transform, 1f);

        yield return new WaitForSeconds(1f);

        enemyBehaviour.canShootBullets = true;
        
    }

    public void KillTank()
    {
        roundManager.UpdateCount(gameObject);

        sfxManager.PlaySFX(die, tankBody.transform, 1f);
        
        GameObject deathMarker = Instantiate(deathMarkerPrefab, tankBody.transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }

}
