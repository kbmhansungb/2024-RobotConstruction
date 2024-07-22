using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    private void Update()
    {
        // 이미지 플랜을 기준으로 반전시킵니다.
        var forward = Camera.main.transform.forward;
        transform.forward = forward;
    }
}
