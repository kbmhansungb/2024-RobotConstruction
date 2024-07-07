using System;
using UnityEngine;

public enum EGameState
{
    None,
    MainMenu,
    Memory,
    Contact,
    More,
    // Exit // WebGL에서는 Application.Quit()을 사용할 수 없습니다.
}

public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Space(10)]
    [SerializeField] private Environment environment;
    [SerializeField] private MainMenuPresenter mainMenuPresenter;
    [SerializeField] private MemoryPresenter memoryPresenter;

    private EGameState gameState = EGameState.MainMenu;

    private void Start()
    {
        // 싱글톤 패턴을 적용합니다.
        Instance = this;

        // 초기화를 진행합니다.
        environment.Init();
        mainMenuPresenter.Init();
        memoryPresenter.Init();
        
        SetMainMenuStateImmediate();
    }

    public void SetMainMenuStateImmediate()
    {
        mainMenuPresenter.SetState(MainMenuPresenter.EMainMenuState.Visible);
        memoryPresenter.SetState(MemoryPresenter.EMemoryState.Disactive); 
    }

    public void SetState(EGameState state)
    {
        gameState = state;

        switch (gameState)
        {
            // 2024 07 06
            // 각 상태로의 이동은 메인 메뉴에서만 이동하는 것을 전제로 합니다.
            // 다른 상태에서 이동할 경우, 각각 상태에서의 전환 효과를 추가해야 합니다.
            case EGameState.MainMenu:
                {
                    environment.FadeInOut(0.6f, 
                    ()=>{ 
                        mainMenuPresenter.SetState(MainMenuPresenter.EMainMenuState.Visible);
                        memoryPresenter.SetState(MemoryPresenter.EMemoryState.Disactive); 
                    }, 
                    ()=>{  
                    });
                }
                break;
            case EGameState.Memory:
                {
                    mainMenuPresenter.SetState(MainMenuPresenter.EMainMenuState.Invisible);
                    environment.FadeInOut(0.6f, 
                    ()=>{
                        mainMenuPresenter.SetState(MainMenuPresenter.EMainMenuState.Disactive);
                        memoryPresenter.SetState(MemoryPresenter.EMemoryState.Active);
                    }, 
                    ()=>{
                    });
                }
                break;
            case EGameState.Contact:
                {
                    
                }
                break;
            case EGameState.More:
                {
                    Application.Quit();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
