using System;
using UnityEngine;
using Cinemachine;

public class AimingCameraLookAt : MonoBehaviour
{
    private float rotationSpeed = 20f;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float bottomClamp = -40f;
    [SerializeField] private float topClamp = 70f;
    [SerializeField] private float smoothingSpeed = 10f;
    private float cinemachineTargetPitch;
    private float cinemachineTargetYaw;

    private void LateUpdate()
    {
        CameraLogic();
    }

    private void OnEnable()
    {
        // Debug.Log("Camera Enabled");
        UpdateCameraRotation();
    }

    private void OnDisable()
    {
        Vector3 currentEulerAngles = followTarget.rotation.eulerAngles;
        followTarget.rotation = Quaternion.Euler(0, currentEulerAngles.y, currentEulerAngles.z);
    }


    private void CameraLogic()
    {
        float mouseX = GetMouseInput("Mouse X");
        float mouseY = GetMouseInput("Mouse Y");

        cinemachineTargetPitch = UpdateRotation(cinemachineTargetPitch, mouseY, bottomClamp, topClamp, true);
        cinemachineTargetYaw = UpdateRotation(cinemachineTargetYaw, mouseX, float.MinValue, float.MaxValue, false);

        ApplyRotations(cinemachineTargetPitch, cinemachineTargetYaw);
    }

    private void UpdateCameraRotation()
    {
        Vector3 targetDirection = followTarget.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        cinemachineTargetYaw = targetRotation.eulerAngles.y;
        cinemachineTargetPitch = targetRotation.eulerAngles.x;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothingSpeed * Time.deltaTime);
    }

    private void ApplyRotations(float pitch, float yaw)
    {
        followTarget.rotation = Quaternion.Euler(pitch, yaw, followTarget.rotation.eulerAngles.z);
    }

    private float UpdateRotation(float currentRotation, float input, float min, float max, bool isAxis)
    {
        currentRotation += isAxis ? -input : input;
        return Mathf.Clamp(currentRotation, min, max);
    }

    private float GetMouseInput(string axis)
    {
        return Input.GetAxis(axis) * rotationSpeed * Time.deltaTime;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(-mouseY, mouseX, 0);
    }
}
