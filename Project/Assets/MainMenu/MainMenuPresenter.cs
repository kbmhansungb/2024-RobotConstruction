using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    /// <summary>
    /// <para>메인 메뉴의 상태를 나타냅니다.</para>
    /// <para>Visible과 Invisible은 메뉴가 보여지거나 사라지는 중이며 상호작용이 불가능합니다.</para>
    /// </summary>
    public enum EMainMenuState
    {
        Visible,
        Interactive,
        Invisible,
        Disactive,
        Contact,
    }
    EMainMenuState mainMenuState = EMainMenuState.Disactive;
    Coroutine stateCoroutine = null;

    [Header("Main Menu Buttons")]
    [SerializeField] private List<MainMenuItem> mainMenuItems;
    [SerializeField] private MainMenuItem careerButton;
    [SerializeField] private MainMenuItem contactButton;
    [SerializeField] private MainMenuItem moreButton;

    private MainMenuItem currentItem;

    [Space(10)]
    [SerializeField] private Vector2 buttonRange = new Vector2(0.0f, 0.6f);

    /// <summary>
    /// 메인 메뉴를 초기화합니다.
    /// </summary>
    public void Init()
    {
        mainMenuItems.ForEach(item => {
            item.Init();

            item.OnEnter += OnEnterItem;
            item.OnExit += OnExitItem;
        });

        careerButton.OnClick += OnClickMemory;
        contactButton.OnClick += OnClickContact;
        moreButton.OnClick += OnClickExit;
    }

    /// <summary>
    /// 메인 메뉴의 상태를 변경합니다.
    /// </summary>
    /// <param name="newMainMenuState"></param>
    /// <param name="callback"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetState(EMainMenuState newMainMenuState, Action callback = null)
    {
        if (mainMenuState == newMainMenuState)
            return;

        if (stateCoroutine != null)
        {
            GameManager.Instance.StopCoroutine(stateCoroutine);
            stateCoroutine = null;
        }

        switch (newMainMenuState)
        {
            case EMainMenuState.Visible:
                {
                    this.gameObject.SetActive(true);

                    this.stateCoroutine = GameManager.Instance.StartCoroutine(CahngeStateInteractiveCoroutine(0.5f, callback));
                }
                break;
            case EMainMenuState.Interactive:
                {
                    this.gameObject.SetActive(true);

                    callback?.Invoke();
                }
                break;
            case EMainMenuState.Invisible:
                {
                    this.gameObject.SetActive(true);

                    this.stateCoroutine = GameManager.Instance.StartCoroutine(ChangeStateDisactiveCoroutine(0.5f, callback));
                }
                break;
            case EMainMenuState.Disactive:
                {
                    this.gameObject.SetActive(false);

                    callback?.Invoke();
                }
                break;
            default:
                throw new NotImplementedException();
        }

        // 스위칭이 완료되면 상태를 갱신합니다.
        mainMenuState = newMainMenuState;
    }

    private IEnumerator CahngeStateInteractiveCoroutine(float duration, Action callback = null)
    {
        yield return new WaitForSeconds(duration);

        SetState(EMainMenuState.Interactive, callback); // callback은 SetState에서 호출됩니다.
    }

    private IEnumerator ChangeStateDisactiveCoroutine(float duration, Action callback = null)
    {
        yield return new WaitForSeconds(duration);

        SetState(EMainMenuState.Disactive, callback); // callback은 SetState에서 호출됩니다.
    }
    
    private void FixedUpdate()
    {
        // 현재 버튼인 경우 buttonRange.Y 값으로 아닌경우 buttonRange.X 값으로 보간합니다.
        for (int index = 0; index < mainMenuItems.Count; index++)
        {
            var item = mainMenuItems[index];

            // 버튼의 위치를 변경합니다.
            var posX = item.transform.localPosition.x;
            var newPosX = Mathf.Lerp(posX, currentItem == item ? buttonRange.y : buttonRange.x, Time.deltaTime * 10.0f);

            item.transform.localPosition = new Vector3(newPosX, item.transform.localPosition.y, item.transform.localPosition.z);

            // 버튼의 색상을 변경합니다.
            var weight = item.Material.GetFloat(item.LerpWeight);
            var newWeight = Mathf.Lerp(weight, currentItem == item ? 1.0f : 0.0f, Time.deltaTime * 10.0f);

            item.Material.SetFloat(item.LerpWeight, newWeight);
        }
    }

#region Event Methods

    public void OnClickMemory(MainMenuItem self)
    {
        if (this.mainMenuState != EMainMenuState.Interactive)
            return;

        Debug.Log("OnClickMemory: " + self.name);

        GameManager.Instance.SetState(EGameState.Memory);
    }

    public void OnClickContact(MainMenuItem self)
    {
        if (this.mainMenuState != EMainMenuState.Interactive)
            return;
            
        Debug.Log("OnClickContact: " + self.name);

        GameManager.Instance.SetState(EGameState.Contact);
    }

    public void OnClickExit(MainMenuItem self)
    {
        if (this.mainMenuState != EMainMenuState.Interactive)
            return;

        if (currentItem != self)
            return;
            
        Debug.Log("OnClickExit: " + self.name);

        GameManager.Instance.SetState(EGameState.More);
    }

    public void OnEnterItem(MainMenuItem self)
    {
        if (this.mainMenuState != EMainMenuState.Interactive)
            return;
            
        // Debug.Log("OnEnterItem: " + self.name);
        
        currentItem = self;
    }

    public void OnExitItem(MainMenuItem self)
    {
        // Debug.Log("OnExitItem: " + self.name);

        if (currentItem != self)
            return;

        currentItem = null;
    }

#endregion
}
