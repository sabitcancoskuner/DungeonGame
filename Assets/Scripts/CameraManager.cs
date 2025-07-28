using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private CinemachineCamera playerFollowCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator ZoomCamera(float zoomAmount, float duration = 1f)
    {
        float startSize = playerFollowCamera.Lens.OrthographicSize;
        float targetSize = startSize - zoomAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            playerFollowCamera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }
    }

    public IEnumerator UnzoomCamera(float zoomAmount, float duration = 1f)
    {
        yield return new WaitForSeconds(0.15f); // Optional delay before unzooming
        
        float startSize = playerFollowCamera.Lens.OrthographicSize;
        float targetSize = startSize + zoomAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            playerFollowCamera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }
    }
}
