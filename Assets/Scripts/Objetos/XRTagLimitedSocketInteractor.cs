using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTagLimitedSocketInteractor : XRSocketInteractor
{
    public string interactableTag;
    public bool isSelectable;

    // Método para verificar si el objeto se puede seleccionar
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag(interactableTag);
    }

    // Método para verificar si el objeto puede ser hover
    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.CompareTag(interactableTag);
    }

    // Este método se llama cuando el objeto es soltado o se conecta al socket
    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnSelectEntered(interactable);

        // Desactivar el collider del objeto interactable cuando se conecta al socket
        DisableCollider(interactable);
    }

    // Desactiva el collider del objeto interactable
    private void DisableCollider(XRBaseInteractable interactable)
    {
        Collider collider = interactable.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;  // Desactivar el collider
        }

        // También podrías desactivar la habilidad de interactuar si lo deseas:
        XRGrabInteractable grabInteractable = interactable.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.interactionLayerMask = LayerMask.GetMask("None");  // Evitar que el objeto sea agarrado
        }
    }

    // Si lo deseas, también puedes reactivar el collider cuando el objeto sea soltado (si es necesario)
    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        base.OnSelectExited(interactable);

        // Reactivar el collider si se desea (opcional)
        EnableCollider(interactable);
    }

    // Reactiva el collider del objeto interactable
    private void EnableCollider(XRBaseInteractable interactable)
    {
        Collider collider = interactable.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;  // Reactivar el collider
        }

        // También podrías reactivar la habilidad de interactuar si se desea:
        XRGrabInteractable grabInteractable = interactable.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.interactionLayerMask = LayerMask.GetMask("Default");  // Reactivar la capacidad de ser agarrado
        }
    }
}
