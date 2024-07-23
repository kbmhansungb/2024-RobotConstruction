using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ContentData
{
    public string Name;
    public RectTransform DescriptionUI;
    public DescriptionPoint DescriptionPoint;

    [Space]
    public List<Outline> Outlines;
    public List<GameObject> ActiveObjects;
}

public class MainPresenter : MonoBehaviour
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
    [SerializeField] private List<ContentData> contents;

    private int currentProjectIndex = 0;

    [Header("MemoryPresenter/UI")]
    [SerializeField] private UnderBar underBar;

    public void Init()
    {
        underBar.Init();
        underBar.beforeEvent += OnClickBefore;
        underBar.nextEvent += OnClickNext;

        // 프로젝트를 초기화합니다.
        foreach (var content in contents)
        {
            content.DescriptionPoint.Init();
            content.DescriptionPoint.OnClickProject += OnClickSubProject;
        }

        SetProject(0, true);
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

    public void SetProject(DescriptionPoint descriptionPoint, bool isImmediate = false)
    {
        int index = contents.FindIndex(elum => { return elum.DescriptionPoint == descriptionPoint; });
        SetProject(index, isImmediate);
    }

    public void SetProject(int index, bool isImmediate = false)
    {
        if (index < 0 || index >= contents.Count)
        {
            Debug.LogError($"{this.GetType().Name}: Invalid index");

            return;
        }

        // 새로운 프로젝트의 상태를 변경합니다.
        currentProjectIndex = index;
        var currentContent = contents[currentProjectIndex];

        // 프로젝트 이름을 설정합니다.
        underBar.SetProjectName(currentContent.Name, currentProjectIndex > 0, currentProjectIndex < contents.Count - 1);

        // 해당하는 프로젝트의 카메라 위치로 이동합니다.
        cameraControllComponent.SetCameraTransform(currentContent.DescriptionPoint.DesiredCameraPosition, isImmediate);

        for (int i = 0; i < contents.Count; i++)
        {
            var content = contents[i];

            // 알맞은 설명을 활성화합니다.
            content.DescriptionUI.gameObject.SetActive(i == index);

            // 모든 아웃라인과 오브젝트를 비활성화 합니다.
            content.Outlines.ForEach(outline => outline.enabled = false);
            content.ActiveObjects.ForEach(obj => obj.SetActive(false));
        }

        // 해당하는 프로젝트의 아웃라인과 오브젝트를 활성화합니다.
        currentContent.Outlines.ForEach(outline => outline.enabled = true);
        currentContent.ActiveObjects.ForEach(obj => obj.SetActive(true));
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
    private void OnClickSubProject(DescriptionPoint project)
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

#endregion
}
