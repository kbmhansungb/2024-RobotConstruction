using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;

public enum EGameState
{
    None,
    Main,
    More,
    // Exit // WebGL에서는 Application.Quit()을 사용할 수 없습니다.
}

public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Space(10)]
    [SerializeField] private Environment environment;
    [SerializeField] private MainPresenter mainPresenter;

    private EGameState gameState = EGameState.Main;

    Socket sock;
    public static string[] GetAllLocalIPv4(NetworkInterfaceType _type)
    {
        List<string> ipAddrList = new List<string>();
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddrList.Add(ip.Address.ToString());
                    }
                }
            }
        }
        return ipAddrList.ToArray();
    }

    private void Start()
    {
        // 싱글톤 패턴을 적용합니다.
        Instance = this;

        // 초기화를 진행합니다.
        environment.Init();
        mainPresenter.Init();
    }

    public void SetState(EGameState state)
    {
        gameState = state;

        switch (gameState)
        {
            // 2024 07 06
            // 각 상태로의 이동은 메인 메뉴에서만 이동하는 것을 전제로 합니다.
            // 다른 상태에서 이동할 경우, 각각 상태에서의 전환 효과를 추가해야 합니다.
            case EGameState.Main:
                {
                }
                break;
            case EGameState.More:
                {
                    // 아래의 url을 엽니다
                    string url = "https://www.gamejob.co.kr/User/Resume/View?R_NO=273777";
                    Application.OpenURL(url);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
