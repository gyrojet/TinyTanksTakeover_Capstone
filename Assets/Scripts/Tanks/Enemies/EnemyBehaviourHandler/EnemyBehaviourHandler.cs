using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection.Emit;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Pathfinding
{
    public class EnemyBehaviourHandler : MonoBehaviour
    {
        //public EnemyTank enemyl;

        [SerializeField] Player player;
        [SerializeField] Enemy attachedEnemy;

        public Seeker seeker;
        public AIPath pathfinder;
        public AIDestinationSetter destinationSetter;

        [Header("Behaviour Attributes")]
        public bool isEnabled = true;

        public bool canMove;
        public bool canShootBullets;
        public bool canUseMines;

        public float attackDelay = 5;
        public float attackRecognitionRange = 50;

        public MovementType movementType;

        [Header("Enemy Sight")]
        public float visibilityRadius = 7;
        public float pathfindingTargetRadius = 4.5f;

        public float pathfindingTargetOffset = 1.65f;

        public float randomMoveRange = 2f;

        public LayerMask playersLayer;
        public LayerMask nodeLayer;

        [Header("LayerMask for Raycasting")]

        [SerializeField] string[] layersToGetWithRaycast;

        public UnityEvent OnShoot = new UnityEvent();
        public UnityEvent OnUseMines = new UnityEvent();

        public UnityEvent<Vector2> OnBodyMove = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnCannonMove = new UnityEvent<Vector2>();

        List<GameObject> possibleTargetNodes;

        bool isChosenTargetValid = false;

        [Header("Debugging Propetries")]
        [SerializeField] bool isPlayerWithinRadius;
        [SerializeField] bool isEligableToShoot;
        public bool didFindPlayer;
        private Vector2 playerPos;
        [SerializeField] Vector2 currentPosition;

        private void Start()
        {
            if (player == null)
                player = Player.playerInstance;

            if (attachedEnemy == null)
                attachedEnemy = gameObject.GetComponent<Enemy>();

            if (pathfinder == null)
                pathfinder = gameObject.GetComponent<AIPath>();

            if (destinationSetter == null)
                destinationSetter = gameObject.GetComponent<AIDestinationSetter>();

            if (seeker == null)
                seeker = gameObject.GetComponent<Seeker>();

            if (isEnabled != true)
                isEnabled = true;

            // Gets random path
            // Check radius for player: If true, set them as the new position. If false, choose a random position and head to it.
            //seeker.GetNewPath(attachedEnemy.tankBody.transform.position, )

            //nearbyNodes.Clear();

            if (canMove)
                UpdatePathfindingTarget();
        }

        public bool IsPlayerWithinRadius(float radiusToCheck)
        {
            return Physics2D.OverlapCircle
                (
                    attachedEnemy.tankBody.transform.position,
                    radiusToCheck,
                    playersLayer
                );
        }

        public void UpdatePathfindingTarget()
        {
            switch (movementType)
            {
                case MovementType.Random:
                    #region Random Movement
                    Collider2D[] nodes = Physics2D.OverlapCircleAll(
                        attachedEnemy.tankBody.transform.position,
                        pathfindingTargetRadius,
                        nodeLayer
                        );

                    destinationSetter.target = nodes[Random.Range(0, nodes.Length)].gameObject.transform;
                    break;
                    #endregion

                case MovementType.FollowPlayer:
                    #region Follow Player
                    if (IsPlayerWithinRadius(visibilityRadius))
                    {
                        print("Finding path to player...");

                        GameObject targetObj = Instantiate(new GameObject("TankTargetPoint", typeof(Transform)));

                        float targetX = player.tankBody.transform.position.x + (Random.Range(-pathfindingTargetOffset, pathfindingTargetOffset));
                        float targetY = player.tankBody.transform.position.y + (Random.Range(-pathfindingTargetOffset, pathfindingTargetOffset));

                        targetObj.transform.position = new Vector2(targetX, targetY);

                        destinationSetter.target = targetObj.transform;
                    }
                    else
                    {
                        print("Player Not Found!");

                        //  REMOVE LATER
                        print("Finding path to player...");

                        GameObject targetObj = Instantiate(new GameObject("TankTargetPoint", typeof(Transform)));

                        float targetX = player.tankBody.transform.position.x + (Random.Range(-pathfindingTargetOffset, pathfindingTargetOffset));
                        float targetY = player.tankBody.transform.position.y + (Random.Range(-pathfindingTargetOffset, pathfindingTargetOffset));

                        targetObj.transform.position = new Vector2(targetX, targetY);

                        destinationSetter.target = targetObj.transform;
                    }
                    break;
                #endregion

                case MovementType.RandomLongest:
                    #region Random, Farthest Distance

                    List<Collider2D> nodesSecond = Physics2D.OverlapCircleAll(
                        attachedEnemy.tankBody.transform.position,
                        pathfindingTargetRadius,
                        nodeLayer
                        ).ToList();

                    
                    break;

                #endregion
                default:
                    print("SOMETHING'S WRONG!");
                    break;
            }
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
                ///
                /// REMOVE LATER
                ///
                if (pathfinder.reachedDestination)
                    UpdatePathfindingTarget();
            }
        }

        private void ApplyCannonMovement()
        {
            if (isPlayerWithinRadius)
            {
                OnCannonMove?.Invoke(playerPos);
            }
        }

        private void ApplyShootingAction()
        {
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
            isPlayerWithinRadius = IsPlayerWithinRadius(visibilityRadius);

            if (isPlayerWithinRadius)
            {
                //print("Tank is eligable to shoot!");
                playerPos = player.tankBody.transform.position;

                //Debug.DrawLine(gameObject.transform.position, self.gameObject.transform.position - gameObject.transform.position, Color.red, Mathf.Infinity);

                // If the player is within the tank's set range
                isEligableToShoot = CheckForPlayerRayHit();
            }
        }

        private bool CheckForPlayerRayHit()
        {
            // If player is within radius AND within distance of the raycast:

            Vector2 origin = attachedEnemy.cannonFiringPoint.transform.position;
            Vector2 direction = (attachedEnemy.cannonFiringPoint.transform.position - attachedEnemy.gameObject.transform.position) * attackRecognitionRange;

            float distance = attackRecognitionRange;
            

            ///
            /// COME BACK HERE LATER!
            ///
            RaycastHit2D rayHit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask(layersToGetWithRaycast));

            //RaycastHit2D rayHit = Physics2D.Raycast(attachedEnemy.cannonFiringPoint.transform.position, (attachedEnemy.cannonFiringPoint.transform.position - attachedEnemy.gameObject.transform.position) * attackRecognitionRange, attackRecognitionRange, playersLayer);

            Debug.DrawRay(origin, direction, Color.red, 0.001f);

            if (rayHit.collider != null)
            {
                //If ray hits player:
                if (rayHit.collider.CompareTag("Player"))
                    didFindPlayer = true;
                else
                    didFindPlayer = false;
            }

            //Debug.Log($"Raycast Status: {didFindPlayer}");

            // Return result of raycast
            return didFindPlayer;
        }
    }
}
