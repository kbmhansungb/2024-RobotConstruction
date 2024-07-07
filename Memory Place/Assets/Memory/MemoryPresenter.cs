using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryPresenter : MonoBehaviour
{
    public enum EMemoryState
    {
        None,
        Active,
        Disactive
    }

    private EMemoryState memoryState;

    [Header("MemoryPresenter/Project")]
    [SerializeField] private CameraControllComponent cameraControllComponent;
    [SerializeField] private List<Project> projects;
    private int currentProjectIndex = 0;


    [Header("MemoryPresenter/Button")]
    [SerializeField] private Button backButton;

    [Header("MemoryPresenter/UI")]
    [SerializeField] private UnderBar underBar;

    public void Init()
    {
        backButton.onClick.AddListener(OnClickBack);

        underBar.Init();
        underBar.beforeEvent += OnClickBefore;
        underBar.nextEvent += OnClickNext;
    }

    public void SetState(EMemoryState state)
    {
        memoryState = state;

        switch (memoryState)
        {
            case EMemoryState.Active:
            {
                gameObject.SetActive(true);
                SetProject(0, true);
            }
                break;
            case EMemoryState.Disactive:
            {
                gameObject.SetActive(false);
            }
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void SetProject(int index, bool isImmediate = false)
    {
        if (index < 0 || index >= projects.Count)
        {
            Debug.LogError("Invalid index");
            return;
        }

        currentProjectIndex = index;

        Project project = projects[currentProjectIndex];
        underBar.SetProjectName(project.ProjectName, currentProjectIndex > 0, currentProjectIndex < projects.Count - 1);

        // 해당하는 프로젝트의 카메라 위치로 이동합니다.
        cameraControllComponent.SetCameraTransform(project.DesiredCameraPosition, isImmediate);

        // 해당하는 프로젝트만 활성화 합니다.
        for (int i = 0; i < projects.Count; i++)
        {
            projects[i].gameObject.SetActive(i == currentProjectIndex);
        }
    }

    private void OnClickNext(UnderBar underBar)
    {
        SetProject(currentProjectIndex + 1);
    }

    private void OnClickBefore(UnderBar underBar)
    {
        SetProject(currentProjectIndex - 1);
    }

#region Event
    public void OnClickBack()
    {
        Debug.Log("OnClickBack");

        GameManager.Instance.SetState(EGameState.MainMenu);
    }

#endregion
}
