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
    [SerializeField] private RawImage RawImage;
    private RenderTexture RenderTexture;
    private int currentProjectIndex = 0;

    public Project CurrentProject { get => currentProjectIndex != -1 ? projects[currentProjectIndex] : null; }


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

        // 프로젝트를 초기화합니다.
        foreach (var project in projects)
        {
            project.Init();
            project.OnClickProject += OnClickSubProject;
        }
    }

    private void Update()
    {
        // RawImage의 크기를 구해 RenderTexture를 생성합니다.
        if (RenderTexture == null)
        {
            RenderTexture = new RenderTexture((int)RawImage.rectTransform.rect.width, (int)RawImage.rectTransform.rect.height, 0);
            RawImage.texture = RenderTexture;
            cameraControllComponent.MainCamera.targetTexture = RenderTexture;
            cameraControllComponent.WorldUICamera.targetTexture = RenderTexture;
        }

        // 화면 크기가 변경되었을 때 RenderTexture의 크기를 변경합니다.
        if (RenderTexture.width != (int)RawImage.rectTransform.rect.width || RenderTexture.height != (int)RawImage.rectTransform.rect.height)
        {
            Destroy(RenderTexture);

            RenderTexture = new RenderTexture((int)RawImage.rectTransform.rect.width, (int)RawImage.rectTransform.rect.height, 0);
            RawImage.texture = RenderTexture;
            cameraControllComponent.MainCamera.targetTexture = RenderTexture;
            cameraControllComponent.WorldUICamera.targetTexture = RenderTexture;
        }
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

    public void SetProject(Project project, bool isImmediate = false)
    {
        int index = projects.IndexOf(project);
        SetProject(index, isImmediate);
    }

    public void SetProject(int index, bool isImmediate = false)
    {
        currentProjectIndex = index;

        Project project = CurrentProject;
        if (project != null)
        {
            underBar.SetProjectName(project.ProjectName, currentProjectIndex > 0, currentProjectIndex < projects.Count - 1);

            // 해당하는 프로젝트의 카메라 위치로 이동합니다.
            cameraControllComponent.SetCameraTransform(project.DesiredCameraPosition, isImmediate);
        }

        // 해당하는 프로젝트만 활성화 합니다.
        for (int i = 0; i < projects.Count; i++)
        {
            // var projectState = EProjectSelectionState.None;
            // if (project == null)
            // {
            //     projectState = EProjectSelectionState.Unselected;
            // }
            // else if (i == currentProjectIndex)
            // {
            //     projectState = EProjectSelectionState.Selected;
            // }
            // else
            // {
            //     projectState = EProjectSelectionState.OtherSelected;
            // }
            // projects[i].SetProjectSelectionState(projectState);

            // 2024 07 17
            // 라벨이 항상 표시되도록 합니다.
            projects[i].SetProjectSelectionState(EProjectSelectionState.Unselected);
        }
    }

#region Event
    private void OnClickNext(UnderBar underBar)
    {
        SetProject(currentProjectIndex + 1);
    }

    private void OnClickBefore(UnderBar underBar)
    {
        SetProject(currentProjectIndex - 1);
    }

    /// <summary>
    /// 프로젝트를 클릭했을 때 호출됩니다.
    /// </summary>
    /// <param name="project"></param>
    private void OnClickSubProject(Project project)
    {
        // 2024 07 17
        // 라벨이 항상 표시되도록 합니다.
        // if (project == CurrentProject)
        // {
        //     SetProject(null);
        // }
        // else
        {
            SetProject(project);
        }
    }

    public void OnClickBack()
    {
        Debug.Log("OnClickBack");

        GameManager.Instance.SetState(EGameState.MainMenu);
    }

#endregion
}
