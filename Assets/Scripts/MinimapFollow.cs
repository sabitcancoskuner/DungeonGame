using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] private float cameraZ = -10f;
    private Transform playerPosition;

    private void Start()
    {
        playerPosition = PlayerManager.Instance.player.transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, cameraZ);
    }
}
