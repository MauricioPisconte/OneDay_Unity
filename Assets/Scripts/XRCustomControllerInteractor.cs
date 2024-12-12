using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCustomControllerInteractor : MonoBehaviour
{
    private XRBaseControllerInteractor _controller;
    private Vector3 scaleLost;

    [SerializeField] private string excludedTag;

    private void Start()
    {
        _controller = GetComponent<XRBaseControllerInteractor>();
        Assert.IsNotNull(_controller, "There is no XRBaseControllerInteractor assigned to this hand " + gameObject.name);

        _controller.selectEntered.AddListener(ParentInteractable);
        _controller.selectExited.AddListener(Unparent);
    }

    private void ParentInteractable(SelectEnterEventArgs arg0)
    {
        // Verificar si el objeto tiene el tag que quieres excluir
        if (arg0.interactable.transform.CompareTag(excludedTag))
        {
            Debug.Log("El objeto tiene el tag excluido, no se asignará como hijo.");
            return; // Si tiene el tag excluido, no lo parentamos
        }

        // Si no tiene el tag excluido, parentamos el objeto
        scaleLost = arg0.interactable.transform.localScale;
        arg0.interactable.transform.parent = transform;
    }

    private void Unparent(SelectExitEventArgs arg0)
    {
        if (arg0.interactable.transform.CompareTag(excludedTag))
        {
            Debug.Log("El objeto tiene el tag excluido, no se asignará como hijo.");
            return; // Si tiene el tag excluido, no lo parentamos
        }
        arg0.interactable.transform.parent = null;
        arg0.interactable.transform.localScale = scaleLost;
    }
}