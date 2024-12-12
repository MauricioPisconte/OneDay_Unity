using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTagLimitedSocketInteractor : XRSocketInteractor
{
    public string interactableTag;
    public int idCorrespondent;
    public bool isSelectable;
    public UnityEvent eventoAudio;
    public AudioSource audioSourceClip;
    
    // Método para verificar si el objeto se puede seleccionar
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag(interactableTag) && idCorrespondent == interactable.GetComponent<CasseteID>().ReturnCasseteID();
    }

    // Método para verificar si el objeto puede ser hover
    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.CompareTag(interactableTag) && idCorrespondent == interactable.GetComponent<CasseteID>().ReturnCasseteID();
    }

    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnSelectEntered(interactable);
        DisableCollider(interactable);
    }

    private void DisableCollider(XRBaseInteractable interactable)
    {
        Collider collider = interactable.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        XRGrabInteractable grabInteractable = interactable.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.interactionLayerMask = LayerMask.GetMask("None");
        }

        StartCoroutine(ReproducirEvento());
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        base.OnSelectExited(interactable);
        EnableCollider(interactable);
    }
    
    private void EnableCollider(XRBaseInteractable interactable)
    {
        Collider collider = interactable.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;  // Reactivar el collider
        }

        
        XRGrabInteractable grabInteractable = interactable.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.interactionLayerMask = LayerMask.GetMask("Default");  // Reactivar la capacidad de ser agarrado
        }
    }
    
    //El script tmb detecta si el cassette es el indicado en base a su ID.


    private IEnumerator ReproducirEvento()
    {
        if (audioSourceClip != null)
        {
            MainLevelManager.instance.TriggerCanMove(false);
            audioSourceClip.Play();
            yield return new WaitForSeconds(audioSourceClip.clip.length + 2f);
            eventoAudio?.Invoke();
            MainLevelManager.instance.TriggerCanMove(true);
        }
        
    }
}
