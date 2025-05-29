using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private TileBase floorTile;

    [Space]
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private TileBase wallTile;

    public void PaintFloorTiles(HashSet<Vector2Int> floorPositions)
    {
        floorTilemap.ClearAllTiles();
        foreach (Vector2Int position in floorPositions)
        {
            PaintSingleTile(floorTilemap, floorTile, position);
        }
    }

    public void PaintWallTiles(HashSet<Vector2Int> wallPositions)
    {
        wallTileMap.ClearAllTiles();
        foreach (Vector2Int position in wallPositions)
        {
            PaintSingleTile(wallTileMap, wallTile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        tilemap.SetTile((Vector3Int)position, tile);
    }

}
