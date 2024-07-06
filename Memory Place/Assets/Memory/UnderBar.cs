using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void UnderBarEvent(UnderBar underBar);

public class UnderBar : MonoBehaviour
{
    [Header("UnderBar")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button beforeButton;
    [SerializeField] private TMP_Text projectText; 

    public UnderBarEvent nextEvent;
    public UnderBarEvent beforeEvent;

    public void Init()
    {
        nextButton.onClick.AddListener(OnClickNext);
        beforeButton.onClick.AddListener(OnClickBefore);
    }

    public void SetProjectName(string projectName, bool haveBefore = false, bool haveNext = false)
    {
        projectText.text = projectName;

        nextButton.interactable = haveNext;
        beforeButton.interactable = haveBefore;
    }

#region Event
    private void OnClickNext()
    {
        nextEvent?.Invoke(this);
    }

    private void OnClickBefore()
    {
        beforeEvent?.Invoke(this);
    }
#endregion
}
