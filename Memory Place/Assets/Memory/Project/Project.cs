using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project : MonoBehaviour
{
    [Header("Project")]
    [SerializeField] private string projectName;

    public string ProjectName { get => projectName; }


    [Header("Project/Camera")]
    [SerializeField] private CameraControllComponent desiredCameraPosition;
    
    public CameraControllComponent DesiredCameraPosition { get => desiredCameraPosition; set => desiredCameraPosition = value; }

    public void Init()
    {
        
    }
}
