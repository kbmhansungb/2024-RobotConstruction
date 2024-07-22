using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Environment : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Volume volume;

    public void Init()
    {
        
    }
}
