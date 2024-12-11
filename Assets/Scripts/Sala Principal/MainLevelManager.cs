using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class MainLevelManager : MonoBehaviour
{
    //public static MainLevelManager instance;
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MoveProviderCustom moveProvider;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private Transform rigthHand;
    [SerializeField] private Transform leftHand;
    private LineRenderer lineRendererRight;
    private LineRenderer lineRendererLeft;
    
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

    // public void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    // }

    private void Start()
    {
        lineRendererRight = rigthHand.GetComponent<LineRenderer>();
        lineRendererLeft = leftHand.GetComponent<LineRenderer>();
    }

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
        if (!canMove)
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
        moveProvider.CanMove(canMove);
    }

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
}
