using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveProviderCustom : ActionBasedContinuousMoveProvider
{
    [SerializeField]
    [Tooltip("Determines whether the player can move")]
    private bool m_CanMove = true;

    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private float volumeIncrease = 0.1f;
    [SerializeField] private float volumeDecrease = 0.2f;
    [SerializeField] private float volumeChangeInterval = 0.5f;

    [SerializeField] private bool isMoving = false;
    private float lastVolumeChangeTime = 0f;

    protected void Update()
    {
        base.Update();
        AdjustVolume();
    }

    public void CanMove(bool veredict)
    {
        m_CanMove = veredict;
    }

    protected override Vector2 ReadInput()
    {
        if (!m_CanMove)
        {
            isMoving = false;
            return Vector2.zero;
        }

        isMoving = base.ReadInput() != Vector2.zero;

        return base.ReadInput();
    }

    private void AdjustVolume()
    {
        if (footstepAudio == null) return;

        if (Time.time - lastVolumeChangeTime >= volumeChangeInterval)
        {
            lastVolumeChangeTime = Time.time;

            if (isMoving)
            {
                footstepAudio.volume = Mathf.Clamp(footstepAudio.volume + volumeIncrease, 0, 1);
            }
            else
            {
                footstepAudio.volume = Mathf.Clamp(footstepAudio.volume - volumeDecrease, 0, 1);
            }
        }
    }
}