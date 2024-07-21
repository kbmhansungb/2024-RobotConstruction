using UnityEngine;

public delegate void MainMenuEvent(MainMenuItem self);

// 마우스 이벤트를 받아서 처리할 수 있게 합니다.
public class MainMenuItem : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public MainMenuEvent OnClick = new MainMenuEvent((MainMenuItem self) => { });
    public MainMenuEvent OnEnter = new MainMenuEvent((MainMenuItem self) => { });
    public MainMenuEvent OnExit = new MainMenuEvent((MainMenuItem self) => { });

    public Material Material => meshRenderer.material;
    public string LerpWeight => "_LerpWeight";

    public void Init()
    {
        meshRenderer.material = new Material(meshRenderer.material);
    }

    public void OnPointerClick()
    {
        Debug.Log("Pointer Clicked on " + gameObject.name);

        OnClick?.Invoke(this);
    }

    public void StartEffect()
    {
        OnEnter?.Invoke(this);
    }

    public void EndEffect()
    {
        OnExit?.Invoke(this);
    }
}
