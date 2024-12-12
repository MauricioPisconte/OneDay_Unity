using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DialInteractable : XRBaseInteractable
{
    private Transform selectingInteractorTransform;
    private float initialInteractorZRotation; // Rotación inicial del interactor en Z
    private float initialLocalXRotation; // Rotación inicial del objeto en X

    // Ajustes para mejorar la interacción
    [SerializeField] private float rotationSensitivity = 1f; // Sensibilidad de la rotación
    [SerializeField] private float rotationSmoothness = 5f; // Suavizado de la rotación

    private void Start()
    {
        // Inicializamos los valores
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        selectingInteractorTransform = args.interactor.transform;
        initialInteractorZRotation = selectingInteractorTransform.eulerAngles.z; // Guardar la rotación inicial en Z
        initialLocalXRotation = transform.localEulerAngles.x; // Guardar la rotación local en X
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        selectingInteractorTransform = null;
    }

    private void Update()
    {
        if (selectingInteractorTransform != null)
        {
            // Calculamos la diferencia de rotación entre el controlador (interactor) y el objeto en el eje Z
            float deltaRotationZ = selectingInteractorTransform.eulerAngles.z - initialInteractorZRotation;

            // Ajustamos la rotación para mejorar la experiencia, multiplicamos por la sensibilidad
            deltaRotationZ *= rotationSensitivity;

            // Aplicamos suavizado a la rotación para hacerla más fluida
            float newXRotation = Mathf.LerpAngle(transform.localEulerAngles.x, initialLocalXRotation + deltaRotationZ, Time.deltaTime * rotationSmoothness);

            // Aplicamos la nueva rotación solo en el eje X, pero la rotación es controlada por el Z del interactor
            transform.localEulerAngles = new Vector3(
                newXRotation, // Modificamos solo el eje X
                transform.localEulerAngles.y, // Mantenemos la rotación Y
                transform.localEulerAngles.z  // Mantenemos la rotación Z
            );
        }
    }
}
