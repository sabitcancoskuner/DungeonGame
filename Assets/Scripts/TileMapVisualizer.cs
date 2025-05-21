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

        PaintEmptyTilesInside(floorPositions);
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

    private void PaintEmptyTilesInside(HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int position in floorPositions)
        {
            Vector3Int tileCellPos = (Vector3Int)position;

            if (floorTilemap.HasTile(tileCellPos) == false)
            {
                Vector3Int up = tileCellPos + Vector3Int.up;
                Vector3Int down = tileCellPos + Vector3Int.down;
                Vector3Int left = tileCellPos + Vector3Int.left;
                Vector3Int right = tileCellPos + Vector3Int.right;

                if (floorTilemap.GetTile(up) == floorTile &&
                    floorTilemap.GetTile(down) == floorTile &&
                    floorTilemap.GetTile(left) == floorTile &&
                    floorTilemap.GetTile(right) == floorTile)
                {
                    PaintSingleTile(floorTilemap, floorTile, position);    
                }
            }
        }
    }
}
