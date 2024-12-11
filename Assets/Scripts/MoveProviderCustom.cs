using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveProviderCustom : ActionBasedContinuousMoveProvider
{
    [SerializeField]
    [Tooltip("Determines whether the player can move")]
    private bool m_CanMove = true;

    [SerializeField]
    [Tooltip("The AudioSource that will adjust volume based on movement")]
    private AudioSource movementAudioSource;

    [SerializeField]
    [Tooltip("The rate at which the volume changes")]
    private float volumeChangeRate = 1f;
    
    public CharacterController characterController;  // Asigna tu CharacterController aquí

    public bool isMoving = false;

    public void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        AdjustAudioVolume();
    }


    public void CanMove(bool verdict)
    {
        m_CanMove = verdict;
    }

    protected override Vector2 ReadInput()
    {
        if (!m_CanMove)
        {
            isMoving = false;
            return Vector2.zero;
        }

        Vector2 input = base.ReadInput();
        isMoving = input != Vector2.zero;

        // Invertir los ejes de entrada
        Vector3 moveDirection = new Vector3(-input.x, 0, -input.y); // Invertimos el movimiento

        // Mover al personaje con el CharacterController
        characterController.Move(moveDirection * Time.deltaTime);

        return input;
    }

    private void AdjustAudioVolume()
    {
        if (movementAudioSource == null)
            return;

        float targetVolume = isMoving ? 1f : 0f;
        movementAudioSource.volume = Mathf.MoveTowards(movementAudioSource.volume, targetVolume, volumeChangeRate * Time.deltaTime);
    }
}