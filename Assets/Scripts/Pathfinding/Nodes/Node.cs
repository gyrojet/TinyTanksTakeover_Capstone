using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Node : MonoBehaviour
{
    [Header("Traversability")]
    public bool isTraversable;

    [Header("Attributes")]
    public int G;
    public int H;
    public int F;
}
