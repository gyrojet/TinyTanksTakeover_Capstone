using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement Properties")]
    public float moveSpeed = 50;
    public int numOfBounces = 1;

    Vector2 lastVelocity;

    [Header("Identification")]
    public string wallTag = "Wall";
    public string bulletTag = "Bullet";

    public string playerTag = "Player";
    public string enemyTag = "Enemy";

    [Header("Explosion Effect")]
    public GameObject explosion;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D bulletRB;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        lastVelocity = bulletRB.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(bulletTag) == true)
            DestroySelf();

        if (collision.gameObject.CompareTag(wallTag))
        {
            if (numOfBounces > 0)
            {
                #region ReflectBullet
                numOfBounces--;

                Vector2 surfaceNormal = collision.contacts[0].normal;

                bulletRB.linearVelocity = Vector2.Reflect(lastVelocity, surfaceNormal);
                #endregion
            }
            else
            {
                DestroySelf();
            }
        }
        else
        {
            if (collision.gameObject.CompareTag(playerTag) == true)
            {
                #region Kill Player
                print("Hit Player!");

                Tank player = Tank.playerInstance;

                player.KillTank();
                #endregion
            }
            else if (collision.gameObject.CompareTag(enemyTag) == true)
            {
                Debug.Log("Hit Foe!");
                // Add enemy death when finished with it
            }

                DestroySelf();
        }
    }

    
    public void LaunchBullet(Vector2 launchForce)
    {
        bulletRB.linearVelocity = launchForce * moveSpeed;
    }

    private void DestroySelf()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
