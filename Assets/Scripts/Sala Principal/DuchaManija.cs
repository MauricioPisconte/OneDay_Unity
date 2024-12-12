using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DuchaManija : MonoBehaviour
{
    [SerializeField] private DialInteractable canPull;
    [SerializeField] private UnityEvent showObject;
    private bool appeader = false;

    private Quaternion initialRotation;
    private bool hasRotated180 = false;

    void Start()
    {
        // Guardar la rotación inicial al iniciar
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!hasRotated180)
        {
            CheckRotation();
        }
    }

    public void ActivateShower()
    {
        canPull.enabled = true;
    }

    private void CheckRotation()
    {
        // Calcular la rotación relativa desde la rotación inicial
        Quaternion currentRotation = transform.rotation;
        Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * currentRotation;

        // Obtener el ángulo en el eje X
        float angleX = Mathf.Abs(relativeRotation.eulerAngles.x);

        // Asegurarse de que el ángulo esté entre 0 y 360 grados
        if (angleX > 180f) angleX -= 360f;

        // Comprobar si el giro es en sentido de las manecillas del reloj y alcanza los 180 grados
        if (angleX >= 180f && Mathf.Sign(relativeRotation.eulerAngles.x) == 1)
        {
            hasRotated180 = true;
            OnRotationComplete();
        }
    }

    private void OnRotationComplete()
    {
        // Evento o lógica cuando el objeto ha girado 180 grados en sentido horario
        Debug.Log("Rotación de 180 grados completada en el eje X, sentido horario.");
        showObject?.Invoke();
    }
}