using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    public static TileMapVisualizer Instance;

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private Tilemap mapFloorTileMap;
    [SerializeField] private Tilemap mapWallTileMap;

    [Header("Floor Tiles")]
    [SerializeField] private List<TileBase> baseFloorTiles;
    [SerializeField] private TileBase leftFloorTile;
    [SerializeField] private TileBase rightFloorTile;
    [SerializeField] private TileBase topFloorTile;
    [SerializeField] private TileBase downFloorTile;

    [Header("Floor Corner Tiles")]
    [SerializeField] private TileBase topRightFloorTile;
    [SerializeField] private TileBase topLeftFloorTile;
    [SerializeField] private TileBase downRightFloorTile;
    [SerializeField] private TileBase downLeftFloorTile;
    [SerializeField] private TileBase leftRightFloorTile;

    [Header("Wall Tiles")]
    [SerializeField] private TileBase leftWallTile;
    [SerializeField] private TileBase rightWallTile;
    [SerializeField] private TileBase topWallTile;
    [SerializeField] private TileBase bottomWallTile;

    [Header("Wall Corner Tiles")]
    [SerializeField] private TileBase topLeftCornerTile;
    [SerializeField] private TileBase topRightCornerTile;
    [SerializeField] private TileBase bottomRightCornerTile;
    [SerializeField] private TileBase bottomLeftCornerTile;

    [Header("Wall Connectors")]
    [SerializeField] private TileBase topLeftConnector;
    [SerializeField] private TileBase topRightConnector;
    [SerializeField] private TileBase bottomLeftConnector;
    [SerializeField] private TileBase bottomRightConnector;

    [Header("Minimap Tiles")]
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void PaintFloorTiles(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        floorTilemap.ClearAllTiles();
        mapFloorTileMap.ClearAllTiles();

        int directionIndex = 0;
        foreach (Vector2Int position in floorPositions)
        {
            TileBase floorTileToPaintWith = GetFloorTileFromDirection(directionList, directionIndex);

            PaintSingleTile(floorTilemap, floorTileToPaintWith, position);
            PaintSingleTile(mapFloorTileMap, floorTile, position);
            directionIndex++;
        }
    }

    public void PaintWallTiles(HashSet<Vector2Int> wallPositions, List<Vector2Int> directionList)
    {
        wallTileMap.ClearAllTiles();
        mapWallTileMap.ClearAllTiles();

        int directionIndex = 0;
        foreach (Vector2Int position in wallPositions)
        {
            TileBase wallTileToPaintWith = GetWallTileFromDirection(directionList, directionIndex);

            PaintSingleTile(wallTileMap, wallTileToPaintWith, position);
            PaintSingleTile(mapWallTileMap, wallTile, position);
            directionIndex++;
        }
    }

    public void PaintWallConnectors(List<Vector2Int> wallConnectorPositions, List<Vector2Int> wallConnectorDirections)
    {
        int directionIndex = 0;

        foreach (Vector2Int position in wallConnectorPositions)
        {
            TileBase tileToPaintWith = GetConnectorTileFromDirection(wallConnectorDirections, directionIndex);

            PaintSingleTile(wallTileMap, tileToPaintWith, position);
            PaintSingleTile(mapWallTileMap, wallTile, position);
            directionIndex++;
        }
    }

    private TileBase GetFloorTileFromDirection(List<Vector2Int> directionList, int index)
    {
        if (directionList[index] == Vector2Int.zero) // base floor tile
        {
            int baseFloorTileIndex = 0;
            float randomNumber = Random.Range(0, 1f);

            if (randomNumber < 0.04f)
            {
                baseFloorTileIndex = 1;
            }
            else if (randomNumber < 0.08f)
            {
                baseFloorTileIndex = 2;
            }
            else if (randomNumber < 0.12f)
            {
                baseFloorTileIndex = 3;
            }

            return baseFloorTiles[baseFloorTileIndex];
        }

        else if (directionList[index] == Vector2Int.up) // top floor tile
        {
            return topFloorTile;
        }

        else if (directionList[index] == Vector2Int.down) // down floor tile
        {
            return downFloorTile;
        }

        else if (directionList[index] == Vector2Int.right) // right floor tile
        {
            return rightFloorTile; // right floor tile
        }

        else if (directionList[index] == Vector2Int.left) // left floor tile
        {
            return leftFloorTile;
        }

        else if (directionList[index] == new Vector2Int(1, 1)) // top right floor tile
        {
            return topRightFloorTile;
        }

        else if (directionList[index] == new Vector2Int(-1, 1)) // top left floor tile
        {
            return topLeftFloorTile;
        }

        else if (directionList[index] == new Vector2Int(1, -1)) // down right floor tile
        {
            return downRightFloorTile;
        }

        else if (directionList[index] == new Vector2Int(-1, -1)) // down left floor tile
        {
            return downLeftFloorTile;
        }

        return leftRightFloorTile;
    }

    private TileBase GetWallTileFromDirection(List<Vector2Int> directionList, int index)
    {
        if (directionList[index] == Vector2Int.up) // top wall tile
        {
            return topWallTile;
        }

        else if (directionList[index] == Vector2Int.down) // down wall tile
        {
            return bottomWallTile;
        }

        else if (directionList[index] == Vector2Int.left) // left wall tile
        {
            return leftWallTile;
        }

        else if (directionList[index] == Vector2Int.right) // right wall tile
        {
            return rightWallTile;
        }

        else if (directionList[index] == new Vector2(1, 1)) // top right wall tile
        {
            return topRightCornerTile;
        }

        else if (directionList[index] == new Vector2(-1, 1)) // top left wall tile
        {
            return topLeftCornerTile;
        }

        else if (directionList[index] == new Vector2(1, -1)) // bottom right wall tile
        {
            return bottomRightCornerTile;
        }
        
        return bottomLeftCornerTile; // bottom left wall tile

    }

    private TileBase GetConnectorTileFromDirection(List<Vector2Int> directionList, int index)
    {
        if (directionList[index] == new Vector2(1, 1)) // top right tile
        {
            return topRightConnector;
        }

        else if (directionList[index] == new Vector2(-1, 1)) // top left tile
        {
            return topLeftConnector;
        }

        else if (directionList[index] == new Vector2(1, -1)) // bottom right tile
        {
            return bottomRightConnector;
        }

        return bottomLeftConnector;
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        if (tilemap.HasTile((Vector3Int)position))
        {
            return;
        }
        tilemap.SetTile((Vector3Int)position, tile);
    }

}
