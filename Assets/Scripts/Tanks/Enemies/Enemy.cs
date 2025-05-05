using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseTank
{

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
        CheckRadiusForPlayer();
    }

    private void GetComponents()
    {
        if (tankBody != null)
        {
            enemyRB = tankBody.GetComponent<Rigidbody2D>();
        }

        gameManager = GameManager.gameManagerInstance;
    }

    public override void HandleBodyMovement(Vector2 movementVector)
    {
        base.HandleBodyMovement(movementVector);    
    }

    public override void HandleCannonMovement(Vector2 movementVector)
    {
        base.HandleCannonMovement(movementVector);
    }

    public override void HandleMines()
    {
        base.HandleMines();
    }

    public override void HandleShooting()
    {
        base.HandleShooting();
    }

    private void CheckRadiusForPlayer()
    {
        isPlayerVisible = Physics2D.OverlapCircle
            (
                tankBody.transform.position,
                visibilityRadius,
                playerLayerMask
            );

        print(isPlayerVisible);
    }
}
