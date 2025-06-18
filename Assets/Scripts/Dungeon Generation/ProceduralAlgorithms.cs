using System.Collections.Generic;
using UnityEngine;

public static class ProceduralAlgorithms
{
    public static HashSet<Vector2Int> RandomWalkAlgorithm(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);

        for (int i = 0; i < walkLength; i++)
        {
            startPosition += Direction.GetRandomDirection();
            path.Add(startPosition);
        }

        return path;
    }

    public static List<Vector2Int> RandomCorridorAlgorithm(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int direction = Direction.GetRandomDirection();
        var currentPosition = startPosition;

        path.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            path.Add(currentPosition);

            if (direction == Vector2Int.up || direction == Vector2Int.down)
            {
                path.Add(currentPosition + Vector2Int.right);
            }

            else if (direction == Vector2Int.left || direction == Vector2Int.right)
            {
                path.Add(currentPosition + Vector2Int.down);
            }
        }

        return path;
    }
}

public static class Direction
{
    public static List<Vector2Int> fourWayDirectionList = new List<Vector2Int>()
    {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.up
    };

    public static List<Vector2Int> eightWayDirectionList = new List<Vector2Int>()
    {
        new Vector2Int(0, 1), // up
        new Vector2Int(0, -1), // down
        new Vector2Int(1, 0), // right
        new Vector2Int(-1, 0), // left
        new Vector2Int(1, 1), // top right
        new Vector2Int(-1, 1), // top left
        new Vector2Int(1, -1), // down right
        new Vector2Int(-1, -1) // down left
    };

    public static Vector2Int GetRandomDirection()
    {
        Vector2Int randomDirection = fourWayDirectionList[Random.Range(0, fourWayDirectionList.Count)];
        return randomDirection;
    }
}
