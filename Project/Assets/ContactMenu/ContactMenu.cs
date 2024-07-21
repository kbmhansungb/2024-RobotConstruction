using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactMenu : MonoBehaviour
{
    public enum ContactMenuState
    {
        None,
        Active,
        Disactive
    }

    [Header("ContactMenu")]
    [SerializeField] private Button backButton;

    public void Init()
    {
        backButton.onClick.AddListener(OnClickBack);
    }

    public void SetState(ContactMenuState state)
    {
        switch (state)
        {
            case ContactMenuState.Active:
            {
                gameObject.SetActive(true);
            }
                break;
            case ContactMenuState.Disactive:
            {
                gameObject.SetActive(false);
            }
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void OnClickBack()
    {
        GameManager.Instance.SetState(EGameState.MainMenu);
    }
}
