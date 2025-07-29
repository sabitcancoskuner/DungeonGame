using UnityEngine;

public class AreaOfEffectVisualizer : MonoBehaviour
{
    public static AreaOfEffectVisualizer instance;
    [SerializeField] private GameObject areaOfEffectPrefab;

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

    public void ShowAreaOfEffect(Vector3 position, float radius, float duration)
    {
        GameObject areaOfEffect = Instantiate(areaOfEffectPrefab, position, Quaternion.identity);
        areaOfEffect.transform.localScale = new Vector3(radius, radius, 1);
        Destroy(areaOfEffect, duration);
    }

}
