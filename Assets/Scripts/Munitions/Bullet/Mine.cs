using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    SfxManager sfxManager;

    [Header("Attributes")]
    public float explosionDelay = 3f;
    public float explosionSize = 0.245f;
    public string mineTag;

    [Header("Explosion Effect Prefab")]
    public GameObject explosionEffect;

    private CircleCollider2D explosionRadius;

    [Header("SFX")]
    [SerializeField] AudioClip mineExplode;

    private void Start()
    {
        if (sfxManager == null)
        {
            sfxManager = SfxManager.instance;
        }

        explosionRadius = GetComponent<CircleCollider2D>();

        SetExplosionRadius();

        explosionRadius.enabled = false;
    }

    private void SetExplosionRadius()
    {
        explosionRadius.radius = explosionSize;
    }

    public void StartExplosionCount()
    {
        print("StartExplosionCount reached!");
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        print($"Countdown started: {explosionDelay} seconds.");

        yield return new WaitForSeconds(explosionDelay);

        #region Explode
        gameObject.tag = mineTag;

        ColliderHack();

        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        sfxManager.PlaySFX(mineExplode, gameObject.transform, 1f);

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
        #endregion
    }

    public void FakeExplode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void ColliderHack()
    {
        explosionRadius.enabled = false;
        explosionRadius.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Something has collided with the game object!

        string objectTag = collision.gameObject.tag;

        if (objectTag == "Player")
        {
            print("Mine has hit player!");

            Player player = Player.playerInstance;
            player.DeathRoutine();
        }
        else if (objectTag == "Enemy")
        {
            print("mine has hit enemy!");

            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            enemy.KillTank();
        }
    }
}
