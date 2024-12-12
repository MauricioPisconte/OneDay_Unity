using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TvCanalChanger : MonoBehaviour
{
    [Header("Es posible cambiar entre canal 0 al 2")]
    [SerializeField] private Material[] canales;
    [SerializeField] private AudioClip[] sonidosCanales;
    [SerializeField] private MeshRenderer pantallaMat;
    [SerializeField] private float changingChannelDelay;
    [SerializeField] private UnityEvent[] ExecuteAction;
    
    private int indexMaterial;
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        indexMaterial = canales.Length - 1;
    }
    
    public void ChangeCanal(int canal)
    {
        if (canal < canales.Length)
        {
            pantallaMat.material = canales[canal];
        }
    }

    public void ChangeCanalTemporaly(int canal)
    {
        StartCoroutine(ChanginForWhile(indexMaterial, canal));
    }

    private IEnumerator ChanginForWhile(int initialCanal, int finalCanal)
    {
        yield return new WaitForSeconds(changingChannelDelay); 
        ChangeCanal(finalCanal);
        audioSource.clip = sonidosCanales[finalCanal];
        audioSource.Play();
        yield return new WaitForSeconds(sonidosCanales[finalCanal].length);
        ChangeCanal(initialCanal);
        yield return new WaitForSeconds(changingChannelDelay);
        ExecuteAction[finalCanal]?.Invoke();
    }
}