using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BolaDelOcho : MonoBehaviour
{
    [SerializeField] private float shakeThreshold = 0.5f;
    [SerializeField] private float shakeInterval = 0.1f;
    [SerializeField] private float shakeCooldown = 10f;

    [Header("Texto de la bola")]
    [SerializeField] private TextMeshProUGUI textBall;
    [SerializeField] private Transform triangleSpace;
    [SerializeField] private float initialTrianglePosition = 0.24f;
    [SerializeField] private float finalTrianglePosition = 0.265f;

    [System.Serializable]
    public struct Eventos
    {
        public string[] dialogo;
        public UnityEvent evento;
        public float nextEventTimeHold;
    }

    [SerializeField] private Eventos[] eventos;

    private XRGrabInteractable grabInteractable;
    private bool isBeingHeld = false;
    private bool canShake = true;
    private bool canTriggerNextEvent = true; // Nueva variable para el cooldown general

    private float previousYPosition;
    private float shakeMagnitude;
    private float timeSinceLastShakeCheck;

    private int currentEventIndex = 0; // Índice del evento actual
    private int currentDialogIndex = 0; // Índice del diálogo actual dentro del evento

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        //previousYPosition = transform.parent.position.y;
    }

    #region Eventos de la bola

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isBeingHeld = true;
        previousYPosition = transform.parent.position.y;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isBeingHeld = false;
        shakeMagnitude = 0;
    }

    #endregion

    private void Update()
    {
        if (isBeingHeld && canShake && canTriggerNextEvent)
        {
            DetectShaking();
        }
    }

    public void DetectShaking()
    {
        timeSinceLastShakeCheck += Time.deltaTime;

        if (timeSinceLastShakeCheck >= shakeInterval)
        {
            float currentYPosition = transform.parent.position.y;
            float deltaY = Mathf.Abs(currentYPosition - previousYPosition);

            shakeMagnitude += deltaY;

            if (shakeMagnitude >= shakeThreshold)
            {
                StartCoroutine(HandleEventOrDialog());
                StartCoroutine(ShakeCooldown());
                shakeMagnitude = 0;
            }

            previousYPosition = currentYPosition;
            timeSinceLastShakeCheck = 0f;
        }
    }

    private IEnumerator ShakeCooldown()
    {
        canShake = false;
        yield return new WaitForSeconds(shakeCooldown);
        canShake = true;
    }

    public IEnumerator HandleEventOrDialog()
    {
        if (currentEventIndex >= eventos.Length) yield break;

        var currentEvent = eventos[currentEventIndex];

        if (currentDialogIndex < currentEvent.dialogo.Length)
        {
            string dialog = currentEvent.dialogo[currentDialogIndex];
            currentDialogIndex++;

            yield return PlayDialogAnimation(dialog);
        }

        if (currentDialogIndex >= currentEvent.dialogo.Length)
        {
            yield return new WaitForSeconds(4f);
            currentEvent.evento?.Invoke();

            canTriggerNextEvent = false;

            currentEventIndex++;
            currentDialogIndex = 0;

            yield return new WaitForSeconds(currentEvent.nextEventTimeHold);

            canTriggerNextEvent = true;
        }
    }

    private IEnumerator PlayDialogAnimation(string dialog)
    {
        float timeMovement = shakeCooldown / 10;

        yield return new WaitForSeconds(timeMovement);

        textBall.text = dialog;

        float elapsedTime = 0f;

        while (elapsedTime < timeMovement)
        {
            float newYPosition = Mathf.Lerp(initialTrianglePosition, finalTrianglePosition, elapsedTime / timeMovement);
            triangleSpace.localPosition = new Vector3(0f, newYPosition, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        triangleSpace.localPosition = new Vector3(0f, finalTrianglePosition, 0f);
        yield return new WaitForSeconds(shakeCooldown * 8 / 10);

        elapsedTime = 0f;

        while (elapsedTime < timeMovement)
        {
            float newYPosition = Mathf.Lerp(finalTrianglePosition, initialTrianglePosition, elapsedTime / timeMovement);
            triangleSpace.localPosition = new Vector3(0f, newYPosition, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        triangleSpace.localPosition = new Vector3(0f, initialTrianglePosition, 0f);
    }
}