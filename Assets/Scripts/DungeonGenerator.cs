using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] private WalkerSO walker;

    [SerializeField] private TileMapVisualizer tileMapVisualizer;

    [Space]
    [SerializeField] private int gapFillIterations = 3;
    [SerializeField] private int gapFillThreshold = 5;

    [SerializeField] private int wallFlipIterations = 2;
    [SerializeField] private int wallFlipFloorThreshold = 5;
    [SerializeField] private int wallFlipThreshold = 3;

    private HashSet<Vector2Int> floorTilesToPaint = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> wallTilesToPaint = new HashSet<Vector2Int>();


    public void GenerateDungeon()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        floorTilesToPaint.Clear();
        wallTilesToPaint.Clear();

        RunWalker(startPosition, floorPositions); // for creating first floor in the center

        for (int i = 0; i < walker.numOfWalkers; i++)
        {
            GenerateDungeonCorridors(walker.corridorLength, floorPositions);
            GenerateDungeonWall(floorTilesToPaint);

            FloorPostProcessing(floorTilesToPaint, wallTilesToPaint);
            WallPostProcessing(floorTilesToPaint, wallTilesToPaint);
        }

        tileMapVisualizer.PaintFloorTiles(floorTilesToPaint);
        tileMapVisualizer.PaintWallTiles(wallTilesToPaint);
    }

    private void GenerateDungeonCorridors(int corridorLength, HashSet<Vector2Int> floorPositions)
    {
        Vector2Int currentPosition = startPosition;

        for (int i = 0; i < walker.corridorCount; i++)
        {
            var path = ProceduralAlgorithms.RandomCorridorAlgorithm(currentPosition, corridorLength);
            currentPosition = path[path.Count - 1];
            floorPositions.UnionWith(path);
            RunWalker(currentPosition, floorPositions);
            this.floorTilesToPaint.UnionWith(floorPositions);
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

    private void GenerateDungeonWall(HashSet<Vector2Int> floorPositions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in Direction.fourWayDirectionList)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }

        this.wallTilesToPaint.UnionWith(wallPositions);
    }

    private void FloorPostProcessing(HashSet<Vector2Int> floorTilePositions, HashSet<Vector2Int> wallTilePositions)
    {
        List<Vector2Int> tilesToFill = new List<Vector2Int>();
        int neighbourCount = 0;

        for (int i = 0; i < gapFillIterations; i++)
        {
            foreach (Vector2Int position in wallTilePositions)
            {
                foreach (Vector2Int direction in Direction.eightWayDirectionList)
                {
                    Vector2Int neighbourTilePosition = position + direction;

                    if (floorTilePositions.Contains(neighbourTilePosition))
                    {
                        neighbourCount++;
                    }

                }

                if (neighbourCount >= gapFillThreshold)
                {
                    floorTilePositions.Add(position);
                    tilesToFill.Add(position);
                }

                neighbourCount = 0;
            }

            foreach (Vector2Int tile in tilesToFill)
            {
                wallTilePositions.Remove(tile);
            }
        }
    }

    private void WallPostProcessing(HashSet<Vector2Int> floorTilePositions, HashSet<Vector2Int> wallTilePositions)
    {
        List<Vector2Int> wallTilesToRemove = new List<Vector2Int>();
        int floorNeighbourCount = 0;
        int wallNeighbourCount = 0;

        for (int i = 0; i < wallFlipIterations; i++)
        {
            foreach (Vector2Int position in wallTilePositions)
            {
                foreach (Vector2Int direction in Direction.eightWayDirectionList)
                {
                    Vector2Int neighbourTilePosition = position + direction;

                    if (floorTilePositions.Contains(neighbourTilePosition))
                    {
                        floorNeighbourCount++;
                    }

                    if (wallTilePositions.Contains(neighbourTilePosition))
                    {
                        wallNeighbourCount++;
                    }
                }

                if (floorNeighbourCount >= wallFlipFloorThreshold)
                {
                    floorTilePositions.Add(position);
                    wallTilesToRemove.Add(position);
                }

                if (wallNeighbourCount >= wallFlipThreshold)
                {
                    floorTilePositions.Add(position);
                    wallTilesToRemove.Add(position);
                }

                floorNeighbourCount = 0;
                wallNeighbourCount = 0;
            }

            foreach (Vector2Int tile in wallTilesToRemove)
            {
                wallTilesToPaint.Remove(tile);
            }
        }

        GenerateDungeonWall(floorTilesToPaint);
    }
}
