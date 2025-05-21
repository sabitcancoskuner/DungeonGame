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
        }

        return path;
    }
}

public static class Direction
{
    public static List<Vector2Int> directionList = new List<Vector2Int>()
    {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.up
    };

    public static Vector2Int GetRandomDirection()
    {
        Vector2Int randomDirection = directionList[Random.Range(0, directionList.Count)];
        return randomDirection;
    }
}
