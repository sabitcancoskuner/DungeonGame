using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Transform characterToFollow;

    private void LateUpdate()
    {
        transform.position = new Vector3(characterToFollow.position.x, characterToFollow.position.y, characterToFollow.position.z);
    }
}
