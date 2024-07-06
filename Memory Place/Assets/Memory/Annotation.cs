using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotation : MonoBehaviour
{
    [SerializeField] private GameObject description;
    [SerializeField] private Canvas canvas;

    public void Init()
    {
        description.SetActive(false);
    }

    public void Enable()
    {
        canvas.worldCamera = Camera.main;
    }

    public void SetActiveDescription(bool active)
    {
        description.SetActive(active);
    }

    #region Event

    public void OnTriggerSelected()
    {
        SetActiveDescription(true);
    }

    public void OnTriggerDeselected()
    {
        SetActiveDescription(false);
    }

    #endregion
}
