using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] private WalkerSO walker;

    [SerializeField] private TileMapVisualizer tileMapVisualizer;

    [Space]
    [SerializeField] private int postProcessIterations = 5;
    [SerializeField] private int gapFillIterations = 3;
    [SerializeField] private int gapFillThreshold = 5;

    [SerializeField] private int wallFlipIterations = 2;
    [SerializeField] private int wallFlipFloorThreshold = 5;
    [SerializeField] private int wallFlipThreshold = 3;

    private HashSet<Vector2Int> floorTilesToPaint = new HashSet<Vector2Int>();
    private List<Vector2Int> floorTileDirections = new List<Vector2Int>();

    private HashSet<Vector2Int> wallTilesToPaint = new HashSet<Vector2Int>();
    private List<Vector2Int> wallTileDirections = new List<Vector2Int>();

    private List<Vector2Int> wallConnectorsToPaint = new List<Vector2Int>();
    private List<Vector2Int> wallConnectorsDirections = new List<Vector2Int>();

    private void Start() {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {

        floorTilesToPaint.Clear();
        floorTileDirections.Clear();

        wallTilesToPaint.Clear();
        wallTileDirections.Clear();

        wallConnectorsToPaint.Clear();
        wallConnectorsDirections.Clear();

        RunWalker(startPosition, this.floorTilesToPaint); // for creating first floor in the center

        for (int i = 0; i < walker.numOfWalkers; i++)
        {
            GenerateDungeonCorridors(walker.corridorLength);
            GenerateDungeonWall(floorTilesToPaint);
        }

        for (int i = 0; i < postProcessIterations; i++)
        {
            FloorPostProcessing(floorTilesToPaint, wallTilesToPaint);
            WallPostProcessing(floorTilesToPaint, wallTilesToPaint);
        }

        FindAllConnectorPositions(floorTilesToPaint, wallTilesToPaint); // for wall connectors

        GetAllFloorDirections(floorTilesToPaint, wallTilesToPaint, floorTileDirections);
        GetAllWallDirections(floorTilesToPaint, wallTilesToPaint, wallTileDirections);

        tileMapVisualizer.PaintFloorTiles(floorTilesToPaint, floorTileDirections);
        tileMapVisualizer.PaintWallTiles(wallTilesToPaint, wallTileDirections);
        tileMapVisualizer.PaintWallConnectors(wallConnectorsToPaint, wallConnectorsDirections);
    }

    private void GenerateDungeonCorridors(int corridorLength)
    {
        Vector2Int currentPosition = startPosition;

        for (int i = 0; i < walker.corridorCount; i++)
        {
            var path = ProceduralAlgorithms.RandomCorridorAlgorithm(currentPosition, corridorLength);
            currentPosition = path[path.Count - 1];
            this.floorTilesToPaint.UnionWith(path);
            RunWalker(currentPosition, this.floorTilesToPaint);
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

        HashSet<Vector2Int> allTilePositions = new HashSet<Vector2Int>();
        allTilePositions.UnionWith(floorTilePositions);
        allTilePositions.UnionWith(wallTilePositions);

        for (int i = 0; i < gapFillIterations; i++)
        {
            foreach (Vector2Int position in allTilePositions)
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
        CellularAutomataForWall(floorTilePositions, wallTilePositions); // to fill empty spaces next to the newly generated floor tiles
        GenerateDungeonWall(floorTilesToPaint);
    }

    private void CellularAutomataForWall(HashSet<Vector2Int> floorTilePositions, HashSet<Vector2Int> wallTilePositions)
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
    }

    private void FindAllConnectorPositions(HashSet<Vector2Int> floorTilePositions, HashSet<Vector2Int> wallTilePositions)
    {
        foreach (Vector2Int position in wallTilePositions)
        {
            if (floorTilePositions.Contains(position + Vector2Int.up)) // bottom tile
            {
                if (floorTilePositions.Contains(position + Vector2Int.right)) // bottom left tile
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.down);
                    wallConnectorsDirections.Add(new Vector2Int(-1, -1));

                    wallConnectorsToPaint.Add(position + Vector2Int.left);
                    wallConnectorsDirections.Add(new Vector2Int(-1, -1));
                }

                else if (floorTilePositions.Contains(position + Vector2Int.left)) // bottom right tile
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.down);
                    wallConnectorsDirections.Add(new Vector2Int(1, -1));

                    wallConnectorsToPaint.Add(position + Vector2Int.right);
                    wallConnectorsDirections.Add(new Vector2Int(1, -1));
                }

                else if (wallTilePositions.Contains(position + Vector2Int.right) == false && wallTilePositions.Contains(position + Vector2Int.left) == false)
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.right);
                    wallConnectorsDirections.Add(new Vector2Int(1, -1));

                    wallConnectorsToPaint.Add(position + Vector2Int.left);
                    wallConnectorsDirections.Add(new Vector2Int(-1, -1));
                }

                else if (wallTilePositions.Contains(position + Vector2Int.right) == false) // bottom right tile
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.right);
                    wallConnectorsDirections.Add(new Vector2Int(1, -1));
                }

                else if (wallTilePositions.Contains(position + Vector2Int.left) == false) // bottom left file
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.left);
                    wallConnectorsDirections.Add(new Vector2Int(-1, -1));
                }
            }

            else if (floorTilePositions.Contains(position + Vector2Int.down)) // top tile
            {
                if (floorTilePositions.Contains(position + Vector2Int.right)) // top left tile
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.up);
                    wallConnectorsDirections.Add(new Vector2Int(-1, 1));

                    wallConnectorsToPaint.Add(position + Vector2Int.left);
                    wallConnectorsDirections.Add(new Vector2Int(-1, 1));
                }

                else if (floorTilePositions.Contains(position + Vector2Int.left)) // top right tile
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.up);
                    wallConnectorsDirections.Add(new Vector2Int(1, 1));

                    wallConnectorsToPaint.Add(position + Vector2Int.right);
                    wallConnectorsDirections.Add(new Vector2Int(1, 1));
                }

                else if (wallTilePositions.Contains(position + Vector2Int.right) == false && wallTilePositions.Contains(position + Vector2Int.left) == false)
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.right);
                    wallConnectorsDirections.Add(new Vector2Int(1, 1));

                    wallConnectorsToPaint.Add(position + Vector2Int.left);
                    wallConnectorsDirections.Add(new Vector2Int(-1, 1));
                }

                else if (wallTilePositions.Contains(position + Vector2Int.right) == false) // top right tile
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.right);
                    wallConnectorsDirections.Add(new Vector2Int(1, 1));
                }

                else if (wallTilePositions.Contains(position + Vector2Int.left) == false) // top left file
                {
                    wallConnectorsToPaint.Add(position + Vector2Int.left);
                    wallConnectorsDirections.Add(new Vector2Int(-1, 1));
                }
            }
        }
    }

    private void GetAllWallDirections(HashSet<Vector2Int> floorTilePositions, HashSet<Vector2Int> wallTilePositions, List<Vector2Int> wallTilesDirectionList)
    {
        foreach (Vector2Int wallTile in wallTilePositions)
        {
            if (floorTilePositions.Contains(wallTile + Vector2Int.up)) // then it is a bottom tile
            {
                if (floorTilesToPaint.Contains(wallTile + Vector2Int.left))
                {
                    wallTilesDirectionList.Add(new Vector2Int(1, -1)); // bottom right tile
                }

                else if (floorTilesToPaint.Contains(wallTile + Vector2Int.right))
                {
                    wallTilesDirectionList.Add(new Vector2Int(-1, -1)); // bottom left tile
                }

                else
                {
                    wallTilesDirectionList.Add(Vector2Int.down);
                }
            }

            else if (floorTilePositions.Contains(wallTile + Vector2Int.down)) // then it is a top tile
            {
                if (floorTilesToPaint.Contains(wallTile + Vector2Int.left))
                {
                    wallTilesDirectionList.Add(new Vector2Int(1, 1)); // top right tile
                }

                else if (floorTilesToPaint.Contains(wallTile + Vector2Int.right))
                {
                    wallTilesDirectionList.Add(new Vector2Int(-1, 1)); // top left tile
                }

                else
                {
                    wallTilesDirectionList.Add(Vector2Int.up);
                }
            }

            else if (floorTilePositions.Contains(wallTile + Vector2Int.left)) // then it is a right tile
            {
                if (floorTilePositions.Contains(wallTile + Vector2Int.up))
                {
                    wallTilesDirectionList.Add(new Vector2Int(1, -1)); // bottom right tile
                }

                else if (floorTilePositions.Contains(wallTile + Vector2Int.down))
                {
                    wallTilesDirectionList.Add(new Vector2Int(1, 1)); // top right tile
                }

                else
                {
                    wallTilesDirectionList.Add(Vector2Int.right);
                }
            }

            else if (floorTilePositions.Contains(wallTile + Vector2Int.right)) // then it is a left tile
            {
                if (floorTilePositions.Contains(wallTile + Vector2Int.up))
                {
                    wallTilesDirectionList.Add(new Vector2Int(-1, -1)); // bottom left tile
                }

                else if (floorTilePositions.Contains(wallTile + Vector2Int.down))
                {
                    wallTilesDirectionList.Add(new Vector2Int(-1, 1)); // top left tile
                }

                else
                {
                    wallTilesDirectionList.Add(Vector2Int.left);
                }
            }

        }
    }

    private void GetAllFloorDirections(HashSet<Vector2Int> floorTilePositions, HashSet<Vector2Int> wallTilePositions, List<Vector2Int> floorTileDirectionList)
    {
        foreach (Vector2Int tilePos in floorTilePositions)
        {
            if (wallTilePositions.Contains(tilePos + Vector2Int.up)) // top tile
            {
                if (wallTilePositions.Contains(tilePos + Vector2Int.right)) // top right tile
                {
                    floorTileDirectionList.Add(new Vector2Int(1, 1));
                }

                else if (wallTilePositions.Contains(tilePos + Vector2Int.left)) // top left tile
                {
                    floorTileDirectionList.Add(new Vector2Int(-1, 1));
                }

                else
                {
                    floorTileDirectionList.Add(Vector2Int.up);
                }
            }

            else if (wallTilePositions.Contains(tilePos + Vector2Int.down)) // down tile
            {
                if (wallTilePositions.Contains(tilePos + Vector2Int.right)) // down right tile
                {
                    floorTileDirectionList.Add(new Vector2Int(1, -1));
                }

                else if (wallTilePositions.Contains(tilePos + Vector2Int.left)) // down left tile
                {
                    floorTileDirectionList.Add(new Vector2Int(-1, -1));
                }

                else
                {
                    floorTileDirectionList.Add(Vector2Int.down);
                }
            }

            else if (wallTilePositions.Contains(tilePos + Vector2Int.left)) // left tile
            {
                if (wallTilePositions.Contains(tilePos + Vector2Int.right)) // left right tile
                {
                    floorTileDirectionList.Add(new Vector2Int(2, 2)); 
                }
                else
                {
                    floorTileDirectionList.Add(Vector2Int.left);
                }
            }

            else if (wallTilePositions.Contains(tilePos + Vector2Int.right)) // right tile
            {
                if (wallTilePositions.Contains(tilePos + Vector2Int.left)) // left right tile
                {
                    floorTileDirectionList.Add(new Vector2Int(2, 2)); 
                }
                else
                {
                    floorTileDirectionList.Add(Vector2Int.right);
                }
            }

            else
            {
                floorTileDirectionList.Add(Vector2Int.zero);
            }
        }
    }

}
