using UnityEngine;

public class Wall : MonoBehaviour
{
    WallSpriteManager spriteManager;

    [Header("Properties")]
    Sprite wallSprite = null;
    public bool isBreakable = false;

    [SerializeField] string bulletTag = "Bullet";

    [SerializeField] Sprite broken;

    SpriteRenderer wallSpriteRenderer;
    Collider2D wallCollider;

    private void Start()
    {
        if (spriteManager == null)
            spriteManager = WallSpriteManager.instance;

        wallSpriteRenderer = GetComponent<SpriteRenderer>();
        wallCollider = GetComponent<Collider2D>();

        try
        {
            if (wallSpriteRenderer != null)
            {
                AssignSprite();
            }
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
    }

    private void AssignSprite()
    {
        if (isBreakable == false)
        {
            wallSprite = spriteManager.GetWallSprite(true);
        }
        else
        {
            wallSprite = spriteManager.GetWallSprite(false);
        }

        wallSpriteRenderer.sprite = wallSprite;
    }

    public void BreakWall()
    {
        wallCollider.enabled = false;

        print("Wall collider disabled!");

        wallSpriteRenderer.sprite = broken;
        wallSpriteRenderer.sortingOrder = -1;

        print("Sprite renderer disabled!");
    }

    // Wall Destruction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreakable)
        {
            if (collision.gameObject.CompareTag(bulletTag))
            {
                print("Mine explosion hit wall!");

                Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                bullet.DestroySelf(false);

                BreakWall();
            }
        }
    }
}
