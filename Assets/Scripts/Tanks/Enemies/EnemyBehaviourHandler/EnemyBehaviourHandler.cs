using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviourHandler : MonoBehaviour
{
    //public EnemyTank enemyl;

    [SerializeField] Player player;
    [SerializeField] Enemy self;

    [Header("Behaviour Attributes")]
    public bool canMove;
    public bool canShootBullets;
    public bool canUseMines;

    public float attackDelay = 5;

    [Header("Enemy Sight")]
    public float visibilityRadius = 7;
    public LayerMask playersLayer;

    public UnityEvent OnShoot = new UnityEvent();
    public UnityEvent OnUseMines = new UnityEvent();

    public UnityEvent<Vector2> OnBodyMove = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnCannonMove = new UnityEvent<Vector2>();

    [Header("Debugging Propetries")]
    [SerializeField] bool isPlayerVisible;
    [SerializeField] bool isEligableToShoot;

    private Vector2 playerPos;

    private void Start()
    {
        if (player == null)
            player = Player.playerInstance;

        if (self == null)
            self = gameObject.GetComponent<Enemy>();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyCannonMovement();
        ApplyShootingAction();
        ApplyMineAction();
    }

    private void FixedUpdate()
    {
        CheckRadiusForPlayer();
    }

    private void ApplyMovement()
    {
        if (canMove)
        {
            OnBodyMove?.Invoke(new Vector2(0, 0));
        }
    }

    private void ApplyCannonMovement()
    { 
        if (isPlayerVisible)
        {
            OnCannonMove?.Invoke(playerPos);
        }
        else
            OnCannonMove?.Invoke(Random.insideUnitCircle * 1000);
    }

    private void ApplyShootingAction()
    {
        /*
         * Tank can shoot when using Invoke, but it's so quick that it basically destroys the game.
         * Work on this tommorow, maybe you can work something out!
         * 
         * Ideas: Implement a proper delay into the code...
         */

        if (canShootBullets)
        {
            if (isEligableToShoot)
            {
                canShootBullets = false;
                OnShoot?.Invoke();
            }
            else
                self.StopAllCoroutines();
        }
    }

    //private IEnumerator DelayShooting()
    //{
    //    print("Calling DelayShooting");
    //}

    private void ApplyMineAction()
    {
        if (canUseMines)
        {
            OnUseMines?.Invoke();
        }
    }

    private void CheckRadiusForPlayer()
    {
        isPlayerVisible = Physics2D.OverlapCircle
            (
                gameObject.transform.position,
                visibilityRadius,
                playersLayer
            );

        if (isPlayerVisible)
        {
            //print("Tank is eligable to shoot!");
            playerPos = player.tankBody.transform.position;
            isEligableToShoot = true;
        }
        else
            isEligableToShoot = false;
    }
}
