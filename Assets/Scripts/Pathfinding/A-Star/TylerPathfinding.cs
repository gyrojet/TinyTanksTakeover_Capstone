using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TylerPathfinding : MonoBehaviour
{
    // LOOK ST THIS LATER< IF THE OTHER PROJECT FAILS
    AStarGrid nodeGrid;
    List<Node> openList;


    private void Awake()
    {
        nodeGrid = GetComponent<AStarGrid>();
    }

    void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startingNode = nodeGrid.NodeFromWorldPoint(startPos);
        Node targetNode = nodeGrid.NodeFromWorldPoint(targetPos);
    }
}
