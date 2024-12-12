using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceBox : MonoBehaviour
{
    [SerializeField] private Material[] materials; // 0 no, 1 yes
    [SerializeField] private bool isMessageAvailable;

    private MeshRenderer meshOfMessages;
    private AudioSource audioSource;

    [SerializeField] private UnityEvent eventoLlamada;

    protected void Awake()
    {
        meshOfMessages = GetComponent<MeshRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ChangeStateOfMessages(bool veredict)
    {
        meshOfMessages.material = materials[veredict == false ? 0 : 1];
        isMessageAvailable = veredict;
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Hola mundo");

        if (col.CompareTag("Player"))
        {
            if (isMessageAvailable)
            {
                ChangeStateOfMessages(false);
                audioSource.Play();
                Invoke("CallToActionEvent", audioSource.clip.length);
            }
        }
    }

    private void CallToActionEvent()
    {
        eventoLlamada?.Invoke();
    }
}