using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    private Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] private WalkerSO walker;

    [SerializeField] private TileMapVisualizer tileMapVisualizer;

    public void GenerateDungeon()
    {
        Vector2Int currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < walker.numOfWalkers; i++)
        {
            RunWalker(currentPosition, floorPositions);
            PaintDungeonFloor(floorPositions);
            PaintDungeonWall(floorPositions, Direction.directionList);
        }
    }

    private void RunWalker(Vector2Int currentPosition, HashSet<Vector2Int> floorPositions)
    {
        for (int i = 0; i < walker.iteration; i++)
        {
            var path = ProceduralAlgorithms.RandomWalkAlgorithm(currentPosition, walker.walkLength);
            floorPositions.UnionWith(path);

            if (walker.startFromRandomTile)
            {
                currentPosition = path.ElementAt(Random.Range(0, path.Count()));
            }
        }
    }

    private void PaintDungeonFloor(HashSet<Vector2Int> floorPositions)
    {
        tileMapVisualizer.PaintFloorTiles(floorPositions);
    }

    private void PaintDungeonWall(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directions)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }

        tileMapVisualizer.PaintWallTiles(wallPositions);
    }
}
