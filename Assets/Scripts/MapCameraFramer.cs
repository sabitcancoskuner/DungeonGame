using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCameraFramer : MonoBehaviour
{
    public static MapCameraFramer Instance;

    public Camera mapCam;
    public Tilemap minimapTilemap;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void FrameTilemapDungeon()
    {
        minimapTilemap.CompressBounds(); 
        
        Bounds bounds = minimapTilemap.localBounds;

        mapCam.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);

        float boundsRatio = bounds.size.x / bounds.size.y;
        float textureRatio = (float)mapCam.targetTexture.width / (float)mapCam.targetTexture.height;

        float padding = 1f; // Adds a 1-tile border around the edge of the UI map

        if (boundsRatio > textureRatio) 
        {
            mapCam.orthographicSize = ((bounds.size.x / textureRatio) / 2f) + padding; 
        } 
        else 
        {
            mapCam.orthographicSize = (bounds.size.y / 2f) + padding;
        }
    }
}
