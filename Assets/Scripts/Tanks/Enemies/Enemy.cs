using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseTank
{
    [SerializeField] EnemyBehaviourHandler enemyBehaviour;
    [SerializeField] GameManager gameManager;

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

    [Header("Components")]
    [SerializeField] GameObject tankBody;
    [SerializeField] GameObject tankCannon;

    //[SerializeField] List<ParticleSystem> treadMarkMakers;

    [SerializeField] Transform cannonParent;
    [SerializeField] Transform cannonFiringPoint;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject minePrefab;

    public LayerMask playerLayerMask;

    public string bulletTag = "Bullet";

    [SerializeField] private List<Bullet> activeBullets = new List<Bullet>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponents();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckRadiusForPlayer();
    }

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

        gameManager = GameManager.gameManagerInstance;
    }

    public override void HandleBodyMovement(Vector2 movementVector)
    {
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
        //print("SHOOT CALLED");
        //try
        //{
        //    Bullet newBullet = Instantiate(bulletPrefab, cannonFiringPoint.position, cannonFiringPoint.rotation)
        //                      .GetComponent<Bullet>();

        //    newBullet.owner = this.gameObject;

        //    newBullet.LaunchBullet(cannonFiringPoint.transform.up);

        //    //activeBullets.Add(newBullet);
        //}
        //catch (UnityException ex)
        //{
        //    Debug.Log(ex.Message);
        //}
    }

    private IEnumerator Shoot()
    {
        print("SHOOT CALLED");
        enemyBehaviour.canShootBullets = false;

        yield return new WaitForSeconds(4);

            Bullet newBullet = Instantiate(bulletPrefab, cannonFiringPoint.position, cannonFiringPoint.rotation)
                              .GetComponent<Bullet>();

            newBullet.owner = this.gameObject;

            newBullet.LaunchBullet(cannonFiringPoint.transform.up);

            yield return new WaitForSeconds(2.75f);

        enemyBehaviour.canShootBullets = true;
        
    }

    //private IEnumerator EnemyShoot(float delay)
    //{
    //    try
    //    {
    //        Bullet newBullet = Instantiate(bulletPrefab, cannonFiringPoint.position, cannonFiringPoint.rotation)
    //                          .GetComponent<Bullet>();

    //        newBullet.owner = this.gameObject;

    //        newBullet.LaunchBullet(cannonFiringPoint.transform.up);

    //        //activeBullets.Add(newBullet);
    //    }
    //    catch (UnityException ex)
    //    {
    //        Debug.Log(ex.Message);
    //    }

    //    yield return new WaitForSeconds(delay);
    //}
}
