using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvCanalChanger : MonoBehaviour
{
    [Header("Es posible cambiar entre canal 0 al 2")]
    [SerializeField] private Material[] canales;
    [SerializeField] private AudioClip[] sonidosCanales;
    [SerializeField] private MeshRenderer pantallaMat;
    [SerializeField] private float changingChannelDelay;

    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void ChangeCanal(int canal)
    {
        if (canal < canales.Length)
        {
            pantallaMat.materials[0] = canales[canal];
        }
    }

    public void ChangeCanalTemporaly(int canal)
    {
        StartCoroutine(ChanginForWhile(Array.IndexOf(canales, pantallaMat.materials[0]), canal));
    }

    private IEnumerator ChanginForWhile(int initialCanal, int finalCanal)
    {
        yield return new WaitForSeconds(changingChannelDelay);
        ChangeCanal(finalCanal);
        audioSource.clip = sonidosCanales[finalCanal];
        yield return new WaitForSeconds(sonidosCanales[finalCanal].length + 0.5f);
        ChangeCanal(initialCanal);
    }
}