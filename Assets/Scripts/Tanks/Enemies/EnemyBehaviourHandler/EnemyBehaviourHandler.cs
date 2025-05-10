using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviourHandler : MonoBehaviour
{
    //public EnemyTank enemyl;

    [SerializeField] Player player;
    [SerializeField] Enemy attachedEnemy;

    [Header("Behaviour Attributes")]
    public bool isEnabled = true;

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
    [SerializeField] bool isPlayerWithinRadius;
    [SerializeField] bool isEligableToShoot;
    public bool didHitPlayer;
    private Vector2 playerPos;
    [SerializeField] Vector2 currentPosition;

    private void Start()
    {
        if (player == null)
            player = Player.playerInstance;

        if (attachedEnemy == null)
            attachedEnemy = gameObject.GetComponent<Enemy>();

        if (isEnabled != true)
            isEnabled = true;
    }

    private void Update()
    {
        if (isEnabled)
        {
            ApplyMovement();
            ApplyCannonMovement();
            ApplyShootingAction();
            ApplyMineAction();
        }
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
        if (isPlayerWithinRadius)
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
                attachedEnemy.StopAllCoroutines();
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
        isPlayerWithinRadius = Physics2D.OverlapCircle
            (
                attachedEnemy.tankBody.transform.position,
                visibilityRadius,
                playersLayer
            );

        if (isPlayerWithinRadius)
        {
            //print("Tank is eligable to shoot!");
            playerPos = player.tankBody.transform.position;

            //Debug.DrawLine(gameObject.transform.position, self.gameObject.transform.position - gameObject.transform.position, Color.red, Mathf.Infinity);
            if (CheckForPlayerRayHit())
                isEligableToShoot = true;
        }
        else
            isEligableToShoot = false;
    }

    private bool CheckForPlayerRayHit()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(attachedEnemy.cannonFiringPoint.transform.position, attachedEnemy.cannonFiringPoint.transform.position - attachedEnemy.gameObject.transform.position, 100, playersLayer);

        //Debug.DrawLine(attachedEnemy.cannonFiringPoint.transform.position, attachedEnemy.cannonFiringPoint.transform.position - attachedEnemy.gameObject.transform.position, Color.red, Mathf.Infinity);

        if (rayHit.collider != null)
        {
            if (rayHit.collider.CompareTag("Player")) 
                didHitPlayer = true;
            else
                didHitPlayer = false;
        }

        Debug.Log($"Raycast Status: {didHitPlayer}");

        return didHitPlayer;
    }
}
