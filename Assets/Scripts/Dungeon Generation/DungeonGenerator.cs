using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private Vector2Int startPosition;
    [SerializeField] private WalkerSO walker;

    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject nodeParentObject;
    private List<Node> nodes = new List<Node>();

    [Space]
    [SerializeField] private int postProcessIterations = 5;
    [SerializeField] private int gapFillIterations = 3;
    [SerializeField] private int gapFillThreshold = 5;

    [SerializeField] private int wallFlipIterations = 2;
    [SerializeField] private int wallFlipFloorThreshold = 5;
    [SerializeField] private int wallFlipThreshold = 3;

    private HashSet<Vector2Int> floorTilesToPaint;
    private List<Vector2Int> floorTileDirections;

    private HashSet<Vector2Int> wallTilesToPaint;
    private List<Vector2Int> wallTileDirections;

    private List<Vector2Int> wallConnectorsToPaint;
    private List<Vector2Int> wallConnectorsDirections;
    
    private void Start()
    {
        floorTilesToPaint = new HashSet<Vector2Int>();
        floorTileDirections = new List<Vector2Int>();

        wallTilesToPaint = new HashSet<Vector2Int>();
        wallTileDirections = new List<Vector2Int>();
        
        wallConnectorsToPaint = new List<Vector2Int>();
        wallConnectorsDirections = new List<Vector2Int>();

        GenerateDungeon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GenerateDungeon();
        }
    }

    public void GenerateDungeon()
    {
        startPosition = PlayerManager.Instance.GetPlayerGridLocation();
        
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                Destroy(node.gameObject);
            }
        }
        nodes.Clear();

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
        }

        GenerateDungeonWall(floorTilesToPaint);

        for (int i = 0; i < postProcessIterations; i++)
        {
            FloorPostProcessing(floorTilesToPaint, wallTilesToPaint);
            WallPostProcessing(floorTilesToPaint, wallTilesToPaint);
        }

        EnsureConnectivity(floorTilesToPaint);

        GenerateDungeonWall(floorTilesToPaint);

        FindAllConnectorPositions(floorTilesToPaint, wallTilesToPaint); // for wall connectors

        GetAllFloorDirections(floorTilesToPaint, wallTilesToPaint, floorTileDirections);
        GetAllWallDirections(floorTilesToPaint, wallTilesToPaint, wallTileDirections);

        CreateNodes(floorTilesToPaint);
        AStarManager.instance.CacheNodes(nodes);

        TileMapVisualizer.Instance.PaintFloorTiles(floorTilesToPaint, floorTileDirections);
        TileMapVisualizer.Instance.PaintWallTiles(wallTilesToPaint, wallTileDirections);
        TileMapVisualizer.Instance.PaintWallConnectors(wallConnectorsToPaint, wallConnectorsDirections);

        // MapCameraFramer.Instance.FrameTilemapDungeon();
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

        for (int i = 0; i < gapFillIterations; i++)
        {
            HashSet<Vector2Int> allTilePositions = new HashSet<Vector2Int>();
            allTilePositions.UnionWith(floorTilePositions);
            allTilePositions.UnionWith(wallTilePositions);

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

            tilesToFill.Clear();
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

                else if (wallNeighbourCount >= wallFlipThreshold)
                {
                    floorTilePositions.Add(position);
                    wallTilesToRemove.Add(position);
                }

                floorNeighbourCount = 0;
                wallNeighbourCount = 0;
            }

            foreach (Vector2Int tile in wallTilesToRemove)
            {
                wallTilePositions.Remove(tile);
            }

            wallTilesToRemove.Clear();
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
                if (floorTilePositions.Contains(wallTile + Vector2Int.left))
                {
                    wallTilesDirectionList.Add(new Vector2Int(1, -1)); // bottom right tile
                }

                else if (floorTilePositions.Contains(wallTile + Vector2Int.right))
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
                if (floorTilePositions.Contains(wallTile + Vector2Int.left))
                {
                    wallTilesDirectionList.Add(new Vector2Int(1, 1)); // top right tile
                }

                else if (floorTilePositions.Contains(wallTile + Vector2Int.right))
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
            else
            {
                wallTilesDirectionList.Add(Vector2Int.zero);
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

    private void EnsureConnectivity(HashSet<Vector2Int> floorTilePositions)
    {
        while (true)
        {
            // Pick any floor tile as the start and flood fill to find all reachable tiles
            Vector2Int start = floorTilePositions.First();
            HashSet<Vector2Int> reachable = FloodFill(start, floorTilePositions);

            if (reachable.Count == floorTilePositions.Count)
                break; // fully connected

            // Find the closest pair between reachable and unreachable
            Vector2Int bestReachable = Vector2Int.zero;
            Vector2Int bestUnreachable = Vector2Int.zero;
            int bestDistance = int.MaxValue;

            foreach (Vector2Int unreachableTile in floorTilePositions)
            {
                if (reachable.Contains(unreachableTile)) continue;

                foreach (Vector2Int reachableTile in reachable)
                {
                    int dist = Mathf.Abs(unreachableTile.x - reachableTile.x) + Mathf.Abs(unreachableTile.y - reachableTile.y);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestReachable = reachableTile;
                        bestUnreachable = unreachableTile;
                    }
                }
            }

            // Carve a straight corridor between the two closest tiles
            CarveCorridorBetween(bestReachable, bestUnreachable, floorTilePositions);
            GenerateDungeonWall(floorTilePositions);
        }
    }

    private HashSet<Vector2Int> FloodFill(Vector2Int start, HashSet<Vector2Int> floorTilePositions)
    {
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            foreach (Vector2Int direction in Direction.fourWayDirectionList)
            {
                Vector2Int neighbour = current + direction;
                if (floorTilePositions.Contains(neighbour) && !visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }

        return visited;
    }

    private void CarveCorridorBetween(Vector2Int from, Vector2Int to, HashSet<Vector2Int> floorTilePositions)
    {
        Vector2Int current = from;

        // Walk horizontally first, then vertically
        while (current.x != to.x)
        {
            current.x += (to.x > current.x) ? 1 : -1;
            floorTilePositions.Add(current);
        }

        while (current.y != to.y)
        {
            current.y += (to.y > current.y) ? 1 : -1;
            floorTilePositions.Add(current);
        }
    }

    private void CreateNodes(HashSet<Vector2Int> floorTilePositions)
    {
        foreach (Vector2Int position in floorTilePositions)
        {
            GameObject nodeObject = Instantiate(nodePrefab, new Vector2((position.x / 2f) + 0.25f, (position.y / 2f) + 0.25f), Quaternion.identity, nodeParentObject.transform);
            nodes.Add(nodeObject.GetComponent<Node>());
        }

        CreateConnections();
    }

    private void CreateConnections()
    {
        // Create a spatial hash for faster neighbor finding
        Dictionary<Vector2Int, Node> nodeGrid = new Dictionary<Vector2Int, Node>();
        
        // Build a grid lookup - scale by 2 since nodes are at 0.5 intervals (position.x/2f + 0.25f)
        foreach (Node node in nodes)
        {
            Vector2Int gridPos = new Vector2Int(
                Mathf.RoundToInt((node.transform.position.x - 0.25f) * 2), 
                Mathf.RoundToInt((node.transform.position.y - 0.25f) * 2)
            );
            
            // Handle potential duplicate keys
            if (!nodeGrid.ContainsKey(gridPos))
            {
                nodeGrid[gridPos] = node;
            }
        }
        
        // Only check adjacent grid positions for neighbors (8-directional)
        Vector2Int[] adjacentOffsets = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1)
        };
        
        foreach (var kvp in nodeGrid)
        {
            Vector2Int gridPos = kvp.Key;
            Node node = kvp.Value;
            
            foreach (Vector2Int offset in adjacentOffsets)
            {
                Vector2Int neighborPos = gridPos + offset;
                if (nodeGrid.TryGetValue(neighborPos, out Node neighbor))
                {
                    float distance = Vector2.Distance(node.transform.position, neighbor.transform.position);
                    // Increase threshold slightly to account for floating point precision
                    if (distance <= 1.1f)
                    {
                        AddNeighbor(node, neighbor);
                        AddNeighbor(neighbor, node);
                    }
                }
            }
        }

    }

    private void AddNeighbor(Node node, Node neighbor)
    {
        if (!node.neighbors.Contains(neighbor))
        {
            node.neighbors.Add(neighbor);
        }
    }
}
