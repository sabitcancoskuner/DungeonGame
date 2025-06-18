using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private TileBase floorTile;

    [Space]
    [SerializeField] private Tilemap wallTileMap;

    [Space]
    [SerializeField] private TileBase leftWallTile;
    [SerializeField] private TileBase rightWallTile;
    [SerializeField] private TileBase topWallTile;
    [SerializeField] private TileBase bottomWallTile;

    [SerializeField] private TileBase testTile;

    [Header("Corners")]
    [SerializeField] private TileBase topLeftCornerTile;
    [SerializeField] private TileBase topRightCornerTile;
    [SerializeField] private TileBase bottomRightCornerTile;
    [SerializeField] private TileBase bottomLeftCornerTile;

    [Header("Connectors")]
    [SerializeField] private TileBase topLeftConnector;
    [SerializeField] private TileBase topRightConnector;
    [SerializeField] private TileBase bottomLeftConnector;
    [SerializeField] private TileBase bottomRightConnector;

    public void PaintFloorTiles(HashSet<Vector2Int> floorPositions)
    {
        floorTilemap.ClearAllTiles();
        foreach (Vector2Int position in floorPositions)
        {
            PaintSingleTile(floorTilemap, floorTile, position);
        }
    }

    public void PaintWallTiles(HashSet<Vector2Int> wallPositions, List<Vector2Int> directionList)
    {
        wallTileMap.ClearAllTiles();
        int directionIndex = 0;
        foreach (Vector2Int position in wallPositions)
        {
            TileBase tileToPaintWith = GetWallTileFromDirection(directionList, directionIndex);

            PaintSingleTile(wallTileMap, tileToPaintWith, position);
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
            directionIndex++;
        }
    }

    private TileBase GetWallTileFromDirection(List<Vector2Int> directionList, int index)
    {
        if (directionList[index] == Vector2Int.up) // top tile
        {
            return topWallTile;
        }

        else if (directionList[index] == Vector2Int.down) // down tile
        {
            return bottomWallTile;
        }

        else if (directionList[index] == Vector2Int.left) // left tile
        {
            return leftWallTile;
        }

        else if (directionList[index] == Vector2Int.right) // right tile
        {
            return rightWallTile;
        }

        else if (directionList[index] == new Vector2(1, 1)) // top right tile
        {
            return topRightCornerTile;
        }

        else if (directionList[index] == new Vector2(-1, 1)) // top left tile
        {
            return topLeftCornerTile;
        }

        else if (directionList[index] == new Vector2(1, -1)) // bottom right tile
        {
            return bottomRightCornerTile;
        }

        return bottomLeftCornerTile;

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
