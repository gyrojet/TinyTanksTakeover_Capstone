using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Attributes")]
    public float explosionDelay = 3f;
    public float explosionSize = 0.245f;
    public string mineTag;

    [Header("Explosion Effect Prefab")]
    public GameObject explosionEffect;

    private CircleCollider2D explosionRadius;
    private Animator mineAnimator;

    private void Start()
    {
        explosionRadius = GetComponent<CircleCollider2D>();

        SetExplosionRadius();
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

        // Not working, try later
        #region Explode
        gameObject.tag = mineTag;

        explosionEffect.SetActive(true);

        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
        #endregion
    }
}
