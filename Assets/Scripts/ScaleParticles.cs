using UnityEngine;

public class ScaleParticleSystem : MonoBehaviour
{
    public Transform parentObject;
    public ParticleSystem particleSystem;
    public float duration = 60f;
    public Vector3 targetScale = new Vector3(100f, 100f, 100f);
    public Vector2 startSizeRangeMinMax = new Vector2(3f, 9f);
    public Vector2 targetSizeRangeMinMax = new Vector2(300f, 900f);

    private Vector3 initialScale;
    private float elapsedTime = 0f;

    void Start()
    {
        initialScale = parentObject.localScale;
        var main = particleSystem.main;
        main.startSizeX = new ParticleSystem.MinMaxCurve(startSizeRangeMinMax.x, startSizeRangeMinMax.y);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / duration);
        parentObject.localScale = Vector3.Lerp(initialScale, targetScale, progress);

        var main = particleSystem.main;
        float newStartSizeMin = Mathf.Lerp(startSizeRangeMinMax.x, targetSizeRangeMinMax.x, progress);
        float newStartSizeMax = Mathf.Lerp(startSizeRangeMinMax.y, targetSizeRangeMinMax.y, progress);
        main.startSizeX = new ParticleSystem.MinMaxCurve(newStartSizeMin, newStartSizeMax);
    }
}