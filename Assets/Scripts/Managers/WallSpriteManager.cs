using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WallSpriteManager : MonoBehaviour
{
    public static WallSpriteManager instance;

    public List<Sprite> unbreakableWallSprites;
    public List<Sprite> breakableWallSprites;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public Sprite GetWallSprite(bool isUnbreakable)
    {
        Sprite wallSprite;

        if (isUnbreakable)
            wallSprite = unbreakableWallSprites[Random.Range(0, unbreakableWallSprites.Count)];
        else
            wallSprite = breakableWallSprites[Random.Range(0, breakableWallSprites.Count)];

        return wallSprite;
    }
}
