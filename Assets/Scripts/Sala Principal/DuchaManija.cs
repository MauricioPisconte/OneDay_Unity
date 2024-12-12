using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DuchaManija : MonoBehaviour
{
    [SerializeField] private DialInteractable canPull;
    [SerializeField] private UnityEvent showObject;

    [SerializeField] private float targetRotationAngle = 90f;
    [SerializeField] [Range(0f, 360f)] private float currentRotationProgress = 0f;

    private Quaternion initialRotation;
    private bool hasRotatedTarget = false;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!hasRotatedTarget)
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
        Quaternion currentRotation = transform.rotation;
        Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * currentRotation;

        float angleX = Mathf.Abs(relativeRotation.eulerAngles.x);

        if (angleX > 180f) angleX -= 360f;

        currentRotationProgress = Mathf.Abs(angleX);

        if (Mathf.Abs(angleX) >= targetRotationAngle && !hasRotatedTarget)
        {
            hasRotatedTarget = true;
            OnRotationComplete();
        }
    }

    private void OnRotationComplete()
    {
        canPull.enabled = false;
        showObject?.Invoke();
    }
}