using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Annotation : MonoBehaviour
{
    [SerializeField] private GameObject label;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Button button;

    public Button Button { get => button; }

    public void Init()
    {
    }

    public void Enable()
    {
        canvas.worldCamera = Camera.main;
    }

    public void SetActiveDescription(bool active)
    {
        label.SetActive(active);
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
