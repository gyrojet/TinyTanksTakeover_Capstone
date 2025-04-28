using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement Properties")]
    public float moveSpeed = 50;
    public int numOfBounces = 1;

    Vector2 lastVelocity;

    [Header("Identification")]
    public string killableTag = "Killable";

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D bulletRB;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletRB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        lastVelocity = bulletRB.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(killableTag))
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
                // Add explosion later!
                DestroySelf();
            }
        }
        else
        {
            // Add explosion later...
            DestroySelf();
        }
    }

    
    public void LaunchBullet(Vector2 launchForce)
    {
        bulletRB.AddForce(launchForce, ForceMode2D.Force);
    }

    private void DestroySelf()
    {
        Debug.Log("Destroyed!");
        Destroy(gameObject);
    }
}
