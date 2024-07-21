using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProjectSelectionState
{
    None,
    // 선택된 상태
    Selected, 
    // 다른 프로젝트가 선택된 상태
    OtherSelected,
    // 선택되지 않은 상태
    Unselected,
}

public delegate void ProjectDelegate(Project project);

public class Project : MonoBehaviour
{
    [Header("Project")]
    private EProjectSelectionState projectSelectionState;

    [SerializeField] private string projectName;
    [SerializeField] private Annotation annotation;

    public EProjectSelectionState ProjectSelectionState { get => projectSelectionState; }
    public string ProjectName { get => projectName; }
    public Annotation Annotation { get => annotation; }

    public ProjectDelegate OnClickProject;


    [Header("Project/Camera")]
    [SerializeField] private CameraControllComponent desiredCameraPosition;
    
    public CameraControllComponent DesiredCameraPosition { get => desiredCameraPosition; set => desiredCameraPosition = value; }

    public void Init()
    {
        annotation.Init();
        annotation.Button.onClick.AddListener(OnClickAnnotation);
    }

    public void SetProjectSelectionState(EProjectSelectionState projectSelectionState)
    {
        this.projectSelectionState = projectSelectionState;
        
        switch (projectSelectionState)
        {
            case EProjectSelectionState.Selected:
            {
                annotation.SetActiveDescription(false);
            }
                break;
            case EProjectSelectionState.OtherSelected:
            {
                annotation.SetActiveDescription(false);
            }
                break;
            case EProjectSelectionState.Unselected:
            {
                annotation.SetActiveDescription(true);
            }
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    public void OnClickAnnotation()
    {
        Debug.Log($"{this.GetType().Name}: OnClickAnnotation");
        
        OnClickProject?.Invoke(this);
    }
}
