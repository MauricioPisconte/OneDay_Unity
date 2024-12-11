using System.Collections;
using System.Collections.Generic;
using Hanzzz.MeshDemolisher;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [SerializeField] private string targetTag = "TargetTag"; // Tag específico a detectar
    [SerializeField] private float requiredSpeed = 5f; // Velocidad mínima requerida para escribir "Hola Mundo"

    private Rigidbody rb;

    void Start()
    {
        // Obtener el Rigidbody del objeto
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Breaker necesita un Rigidbody para detectar la velocidad.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto colisionado tiene el tag específico
        if (collision.gameObject.CompareTag(targetTag))
        {
            float speed = rb.velocity.magnitude;
            Debug.Log(speed);
            if (speed >= requiredSpeed)
            {
                collision.gameObject.transform.parent.GetComponent<MeshDemolisherExample>().Demolish();
            }
        }
    }
}