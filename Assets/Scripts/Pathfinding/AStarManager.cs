using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    private Node[] cachedNodes;

    public void CacheNodes(List<Node> nodes)
    {
        cachedNodes = nodes.ToArray();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();

        foreach (Node n in cachedNodes)
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector3.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.RemoveAt(lowestF);

            if (currentNode == end)
            {
                List<Node> path = new List<Node>();

                path.Insert(0, end);

                while (currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path; // Return the found path
            }

            foreach (Node neighbor in currentNode.neighbors)
            {
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, neighbor.transform.position);

                if (heldGScore < neighbor.gScore)
                {
                    neighbor.cameFrom = currentNode;
                    neighbor.gScore = heldGScore;
                    neighbor.hScore = Vector2.Distance(neighbor.transform.position, end.transform.position);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // Return null if no path found
    }

    public static Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        Node closestNode = null;
        float closestDistance = float.MaxValue;

        foreach (Node node in instance.cachedNodes)
        {
            float distance = Vector3.Distance(worldPosition, node.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }
}
