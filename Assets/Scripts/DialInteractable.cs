using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DialInteractable : XRBaseInteractable
{
    private Transform selectingInteractorTransform;
    private float initialInteractorYRotation;
    private float initialLocalYRotation;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        selectingInteractorTransform = args.interactor.transform;
        initialInteractorYRotation = selectingInteractorTransform.eulerAngles.x;
        initialLocalYRotation = transform.localEulerAngles.x;
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
            float anglesX = selectingInteractorTransform.eulerAngles.x - initialInteractorYRotation;
            anglesX = -anglesX;

            Vector3 currentRotation = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(
                initialLocalYRotation + anglesX,
                currentRotation.y,
                currentRotation.z
            );
        }
    }
}