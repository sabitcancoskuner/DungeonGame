using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject worldMap;
    [SerializeField] private GameObject minimap;

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            OpenWorldMap(true);
        }
        else
        {
            OpenWorldMap(false);
        }
    }

    private void OpenWorldMap(bool status)
    {
        worldMap.SetActive(status);
        minimap.SetActive(!status);
    }
}
