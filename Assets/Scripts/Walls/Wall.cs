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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBreakable)
        {
            print("Breakable wall has been hit by a mine explosion!");
            if (collision.gameObject.CompareTag(mineTag))
            {
                wallCollider.enabled = false;

                print("Wall collider disabled!");

                wallSpriteRenderer.enabled = false;

                print("Sprite renderer disabled!");
            }
        }
    }
}
