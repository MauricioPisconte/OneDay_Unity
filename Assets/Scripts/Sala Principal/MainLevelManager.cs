using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class MainLevelManager : MonoBehaviour
{
    public static MainLevelManager instance;
    private bool gameStarted = false;
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MoveProviderCustom moveProvider;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private Transform rigthHand;
    [SerializeField] private Transform leftHand;
    
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

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        
    }

    //Corrutina para iniciar el juego
    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            StartCoroutine(GoToTitleScreen());
        }
    }

    private IEnumerator GoToTitleScreen()
    {
        puertaHabitacion.Play();
        
        yield return new WaitForSeconds(puertaHabitacion.clip.length);

        DeleteChildObjectsInHands();
        
        rigthHand.gameObject.SetActive(false);
        leftHand.gameObject.SetActive(false);
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
        rigthHand.gameObject.SetActive(true); 
        leftHand.gameObject.SetActive(true);
    }
    
    public void SetSpawnPosition(int indexPosition)
    {
        playerTransform.position = spawnPosition[indexPosition].position;
    }
    

    public void TriggerCanMove(bool canMove)
    {
        moveProvider.CanMove(canMove);
    }
    
    
    //======= Funciones para reseteo de escena ==========

    public void RestartGame()
    {
        audioSourceBackground.Stop();
        
        postProcesado.SetActive(false);
        pantallaVolumenPS.SetActive(false);
        
        TriggerCanMove(false);
        SetSpawnPosition(0);
        Invoke("RestartScene", 4f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DeleteChildObjectsInHands()
    {
        if (rigthHand.childCount == 3)
        {
            Destroy(rigthHand.GetChild(2));
        }
            
        if (leftHand.childCount == 3)
        {
            Destroy(leftHand.GetChild(2));
        }
    }
}
