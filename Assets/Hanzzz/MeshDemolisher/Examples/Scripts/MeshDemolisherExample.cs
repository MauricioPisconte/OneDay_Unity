using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Hanzzz.MeshDemolisher
{
    public class MeshDemolisherExample : MonoBehaviour
    {
        [SerializeField] private GameObject targetGameObject;
        [SerializeField] private Transform breakPointsParent;
        [SerializeField] private Material interiorMaterial;

        [SerializeField] [Range(0f,1f)] private float resultScale;
        [SerializeField] private Transform resultParent;
        [SerializeField] private AudioSource audioSource;
        
        private static MeshDemolisher meshDemolisher = new MeshDemolisher();
        

        public void Demolish()
        {
            audioSource.Play();
            
            // Limpiar los resultados anteriores
            Enumerable.Range(0, resultParent.childCount)
                      .Select(i => resultParent.GetChild(i))
                      .ToList()
                      .ForEach(x => DestroyImmediate(x.gameObject));

            // Obtener los puntos de ruptura
            List<Transform> breakPoints = Enumerable.Range(0, breakPointsParent.childCount)
                                                     .Select(x => breakPointsParent.GetChild(x))
                                                     .ToList();

            // Destruir el objeto objetivo
            List<GameObject> res = meshDemolisher.Demolish(targetGameObject, breakPoints, interiorMaterial);

            // Asignar los fragmentos al resultado
            res.ForEach(x => x.transform.SetParent(resultParent, true));

            // Escalar los fragmentos
            Enumerable.Range(0, resultParent.childCount)
                      .Select(i => resultParent.GetChild(i))
                      .ToList()
                      .ForEach(x => x.localScale = resultScale * Vector3.one);

            // Desactivar el objeto original
            targetGameObject.SetActive(false);

            // Agregar BoxCollider y Rigidbody a cada fragmento
            foreach (Transform fragment in resultParent)
            {
                if (fragment.gameObject.GetComponent<BoxCollider>() == null)
                    fragment.gameObject.AddComponent<BoxCollider>();

                if (fragment.gameObject.GetComponent<Rigidbody>() == null)
                    fragment.gameObject.AddComponent<Rigidbody>();
            }

            // Programar la destrucción de los fragmentos después de 3 segundos
            StartCoroutine(DestroyFragmentsAfterDelay(3f));
        }

        private System.Collections.IEnumerator DestroyFragmentsAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Destruir los fragmentos
            foreach (Transform fragment in resultParent)
            {
                Destroy(fragment.gameObject);
            }
        }

        public void OnValidate()
        {
            Enumerable.Range(0, resultParent.childCount)
                      .Select(i => resultParent.GetChild(i))
                      .ToList()
                      .ForEach(x => x.localScale = resultScale * Vector3.one);
        }
    }
}