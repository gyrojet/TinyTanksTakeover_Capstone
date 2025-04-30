using UnityEngine;

public class Wall : MonoBehaviour
{
    WallSpriteManager spriteManager;

    [Header("Properties")]
    Sprite wallSprite = null;
    public bool isBreakable = false;

    [SerializeField] string mineTag = "MineExplosion";

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreakable)
        {
            if (collision.gameObject.CompareTag(mineTag) == true)
            {
                Debug.Log("This would appear when a breakable wall is hit by a mine.");
            }
            else
                Debug.Log("Wall is breakable, but was NOT hit by a mine blast!");
        }
        else
            Debug.Log("Wall is not breakable!");
    }
}
