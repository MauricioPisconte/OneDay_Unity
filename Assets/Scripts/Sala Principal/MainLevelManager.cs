using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MainLevelManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MoveProviderCustom moveProvider;
    [SerializeField] private Transform[] spawnPosition;
    
    [Header("Objetos escena 1")]
    [SerializeField] private GameObject textoIntroduccion;
    [SerializeField] private AudioSource puertaHabitacion;
    
    
    [Header("AudioGeneral")]
    [SerializeField] private AudioSource audioSourceBackground;
    [SerializeField] private AudioClip musicaIntroduccion;
    [SerializeField] private AudioClip audioIntroduccion;
    [SerializeField] private AudioClip musicaDeFondo;
    
    
    [Header("Objetos escena 2")]
    [SerializeField] private GameObject postProcesado;
    [SerializeField] private GameObject pantallaVolumenPS;
    [SerializeField] private ScaleParticleSystem blackHole;
    [SerializeField] private GameObject[] vhsGameObjects;

    //Corrutina para iniciar el juego
    public void StartGame()
    {
        StartCoroutine(GoToTitleScreen());
    }

    private IEnumerator GoToTitleScreen()
    {
        puertaHabitacion.Play();
        
        yield return new WaitForSeconds(puertaHabitacion.clip.length);
        
        TriggerCanMove(false);
        SetSpawnPosition(0);
        audioSourceBackground.clip = musicaIntroduccion;
        audioSourceBackground.Play();
        yield return new WaitForSeconds(audioSourceBackground.clip.length-.5f);
        
        textoIntroduccion.SetActive(false);
        yield return new WaitForSeconds(3f);
        
        audioSourceBackground.clip = audioIntroduccion;
        audioSourceBackground.Play();
        
        yield return new WaitForSeconds(audioSourceBackground.clip.length + 2f);

        GoToMainSpace();
    }

    private void GoToMainSpace()
    {
        audioSourceBackground.clip = musicaDeFondo;
        audioSourceBackground.loop = true;
        audioSourceBackground.Play();
        
        postProcesado.SetActive(true);
        pantallaVolumenPS.SetActive(true);
        StartCoroutine(blackHole.ScaleOverTime());

        SetSpawnPosition(1);
        TriggerCanMove(true);
    }
    
    public void SetSpawnPosition(int indexPosition)
    {
        playerTransform.position = spawnPosition[indexPosition].position;
    }

    public void SetVHSappearing(int vhsIndex)
    {
        vhsGameObjects[vhsIndex].gameObject.SetActive(true);
    }

    public void TriggerCanMove(bool canMove)
    {
        moveProvider.CanMove(canMove);   
    }
}
