using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Node
{
    [Header("Traversability")]
    public bool isTraversable;
    public Vector2 worldPosition;

    public Node(bool _isTraversable,  Vector2 _worldPosition)
    {
        isTraversable = _isTraversable;
        worldPosition = _worldPosition;
    }
}
