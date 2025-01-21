using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScaleParticleSystem : MonoBehaviour
{
    public Transform parentObject;
    public ParticleSystem particleSystem;
    public float duration = 60f;
    public Vector3 targetScale = new Vector3(100f, 100f, 100f);
    public Vector2 startSizeRangeMinMax = new Vector2(3f, 9f);
    public Vector2 targetSizeRangeMinMax = new Vector2(300f, 900f);
    public AudioSource audioSourceBack;
    public float finalVolumeAudio;

    private Vector3 initialScale;

    [SerializeField] private UnityEvent Fin;

    void Start()
    {
        initialScale = parentObject.localScale;
        var main = particleSystem.main;
        main.startSizeX = new ParticleSystem.MinMaxCurve(startSizeRangeMinMax.x, startSizeRangeMinMax.y);
        //StartCoroutine(ScaleOverTime());
        
    }

    //public IEnumerator ScaleOverTime()
    //{
    //    float elapsedTime = 0f;

    //    while (elapsedTime < duration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float progress = Mathf.Clamp01(elapsedTime / duration);

    //        parentObject.localScale = Vector3.Lerp(initialScale, targetScale, progress);

    //        var main = particleSystem.main;
    //        float newStartSizeMin = Mathf.Lerp(startSizeRangeMinMax.x, targetSizeRangeMinMax.x, progress);
    //        float newStartSizeMax = Mathf.Lerp(startSizeRangeMinMax.y, targetSizeRangeMinMax.y, progress);
    //        main.startSizeX = new ParticleSystem.MinMaxCurve(newStartSizeMin, newStartSizeMax);

    //        audioSourceBack.volume = Mathf.Lerp(0f, finalVolumeAudio, progress);;

    //        yield return null;
    //    }

    //    parentObject.localScale = targetScale;

    //    var finalMain = particleSystem.main;
    //    finalMain.startSizeX = new ParticleSystem.MinMaxCurve(targetSizeRangeMinMax.x, targetSizeRangeMinMax.y);
    //    Fin?.Invoke();
    //}

    public IEnumerator ScaleOverTime()
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float elapsedTime = Time.time - startTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);

            // Escalado lineal para el objeto
            parentObject.localScale = Vector3.Lerp(initialScale, targetScale, progress);

            // Escalado de partículas
            var main = particleSystem.main;
            float newStartSizeMin = Mathf.Lerp(startSizeRangeMinMax.x, targetSizeRangeMinMax.x, progress);
            float newStartSizeMax = Mathf.Lerp(startSizeRangeMinMax.y, targetSizeRangeMinMax.y, progress);
            main.startSizeX = new ParticleSystem.MinMaxCurve(newStartSizeMin, newStartSizeMax);

            // Ajuste de volumen
            audioSourceBack.volume = Mathf.Lerp(0f, finalVolumeAudio, progress);

            yield return null;
        }

        // Asegúrate de que los valores finales sean exactos
        parentObject.localScale = targetScale;

        var finalMain = particleSystem.main;
        finalMain.startSizeX = new ParticleSystem.MinMaxCurve(targetSizeRangeMinMax.x, targetSizeRangeMinMax.y);

        Fin?.Invoke();
    }

}
