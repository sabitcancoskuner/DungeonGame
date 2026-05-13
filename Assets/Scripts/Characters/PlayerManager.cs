using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Player player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public Vector2Int GetPlayerGridLocation()
    {
        Vector2Int location = Vector2Int.zero;
        location.x = Mathf.RoundToInt((player.transform.position.x - 0.25f) * 2);
        location.y = Mathf.RoundToInt((player.transform.position.y - 0.25f) * 2);

        return location;
    }
}
