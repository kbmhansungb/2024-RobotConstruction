using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllComponent : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Space(10)]
    [SerializeField] private float baseMovementSpeed = 10.0f;
    [SerializeField] private float baseRotationSpeed = 40.0f;
    [SerializeField] private float baseZoomSpeed = 50.0f;
    [SerializeField] private float lerpSpeed = 5.0f; // 보간 속도를 조절할 변수

    private Vector3 desiredPosition;
    private Vector3 desiredEulerRotation;
    float desiredCameraPositionZ;

    private void Start()
    {
        desiredPosition = transform.position;
        desiredEulerRotation = transform.rotation.eulerAngles;
        desiredCameraPositionZ = cameraTransform.transform.localPosition.z;
    }

    private void Update()
    {
        // 카메라와 원점 사이의 거리 계산
        float distance = Mathf.Abs(desiredCameraPositionZ);

        // 거리에 따라 속도 조절
        float movementSpeed = baseMovementSpeed * distance;
        float rotationSpeed = baseRotationSpeed; // 회전 속도는 거리에 따라 변하지 않습니다.
        float zoomSpeed = baseZoomSpeed * distance;

        // 마우스 왼쪽 버튼을 누르고 있을 경우 카메라를 회전합니다.
        if (Input.GetMouseButton(0))
        {
            var deltaX = Input.GetAxis("Mouse X");
            var deltaY = Input.GetAxis("Mouse Y");

            desiredEulerRotation.x -= deltaY * rotationSpeed * Time.deltaTime;
            desiredEulerRotation.y += deltaX * rotationSpeed * Time.deltaTime;

            // x축 회전 각도를 제한합니다.
            desiredEulerRotation.x = Mathf.Clamp(desiredEulerRotation.x, -80f, 80f);
        }
        // 마우스 오른쪽 버튼을 누르고 있을 경우 카메라를 이동합니다.
        else if (Input.GetMouseButton(1))
        {
            var deltaX = Input.GetAxis("Mouse X");
            var deltaY = Input.GetAxis("Mouse Y");

            desiredPosition -= cameraTransform.right * deltaX * movementSpeed * Time.deltaTime;
            desiredPosition -= cameraTransform.up * deltaY * movementSpeed * Time.deltaTime;
        }

        // 마우스 휠을 이동할 경우 카메라를 이동합니다.
        var deltaWheel = Input.GetAxis("Mouse ScrollWheel");
        if (deltaWheel != 0.0f)
        {
            var delta = deltaWheel * zoomSpeed * Time.deltaTime;
            desiredCameraPositionZ += delta;

            // 카메라 거리를 제한합니다.
            desiredCameraPositionZ = Mathf.Clamp(desiredCameraPositionZ, -10.0f, -0.3f);
        }

        // 현재 위치와 회전에서 목표 위치와 회전으로 보간합니다.
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(desiredEulerRotation), lerpSpeed * Time.deltaTime);

        // 카메라의 거리를 보간합니다.
        var cameraLocalPosition = cameraTransform.localPosition;
        cameraLocalPosition.z = Mathf.Lerp(cameraLocalPosition.z, desiredCameraPositionZ, lerpSpeed * Time.deltaTime);
        cameraTransform.localPosition = cameraLocalPosition;
    }

    /// <summary>
    /// <para>카메라의 위치, 회전, 거리를 설정합니다.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eulerRotation"></param>
    /// <param name="cameraDistance"></param>
    public void SetCameraPosition(Vector3 position, Vector3 eulerRotation, float cameraDistance)
    {
        desiredPosition = position;
        desiredEulerRotation = eulerRotation;
        desiredCameraPositionZ = -cameraDistance;
    }
}
