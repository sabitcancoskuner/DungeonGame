using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> neighbors;

    public float gScore;
    public float hScore;

    public float FScore()
    {
        return gScore + hScore;
    }
}
