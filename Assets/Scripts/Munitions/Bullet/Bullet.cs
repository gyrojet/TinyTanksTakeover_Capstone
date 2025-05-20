using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    RoundManager roundManager;

    [Header("Movement Properties")]
    public float moveSpeed = 50;
    public int numOfBounces = 1;

    Vector2 lastVelocity;

    [Header("Identification")]
    public GameObject owner;

    public string wallTag = "Wall";
    public string bulletTag = "Bullet";

    public string playerTag = "Player";
    public string enemyTag = "Enemy";

    [Header("Explosion Effect")]
    public GameObject explosion;

    private ParticleSystem particleSystem;
    private Rigidbody2D bulletRB;

    private void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //roundManager = RoundManager.instance;

        particleSystem.Play();
    }

    private void FixedUpdate()
    {
        lastVelocity = bulletRB.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(bulletTag) == true)
            DestroySelf(false);

        if (collision.gameObject.CompareTag(wallTag))
        {
            if (numOfBounces > 0)
            {
                #region ReflectBullet
                numOfBounces--;

                Vector2 surfaceNormal = collision.contacts[0].normal;

                Vector2 reflectedAngle = Vector2.Reflect(lastVelocity, surfaceNormal);

                //print(reflectedAngle);

                bulletRB.linearVelocity = reflectedAngle;

                bulletRB.transform.up = Vector2.Reflect(transform.up, surfaceNormal);
                #endregion
            }
            else
            {
                DestroySelf(false);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag(playerTag) == true)
            {
                #region Kill Player
                print("Hit Player!");

                Player player = Player.playerInstance;

                player.DeathRoutine();
                #endregion
            }
            else if (collision.gameObject.CompareTag(enemyTag) == true)
            {
                print("Hit Enemy!");

                Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

                enemy.KillTank();
                // Enemy Manager kill script here
            }

            DestroySelf(false);
        }
    }

    
    public void LaunchBullet(Vector2 launchForce)
    {
        bulletRB.linearVelocity = launchForce * moveSpeed;
    }

    public void DestroySelf(bool isEndOfRound)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        particleSystem.Stop();

        if (isEndOfRound == false)
        {
            if (owner.CompareTag(playerTag))
            {
                Player playerOwner = Player.playerInstance;
                playerOwner.RemoveBulletFromList(this);
            }
        }

        /* Plans for enemy classes:
         * - Use an Enum to determine what class should be destroyed
         * - Or try a base class instead!
         */

        Destroy(gameObject);
    }
}
