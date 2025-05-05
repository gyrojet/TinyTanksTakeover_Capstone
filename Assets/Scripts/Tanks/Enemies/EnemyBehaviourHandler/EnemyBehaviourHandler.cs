using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviourHandler : MonoBehaviour
{
    //public EnemyTank enemyl;

    [SerializeField] Player player;

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

    private bool isPlayerVisible;
    private bool isEligableToShoot;

    private Vector2 playerPos;

    private void Start()
    {
        if (player == null)
            player = Player.playerInstance;
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

    private IEnumerator ApplyShootingAction()
    {
        if (canShootBullets)
        {
            if (isEligableToShoot)
            {
                yield return new WaitForSeconds(attackDelay);
                OnShoot?.Invoke();
            }
        }
    }

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
