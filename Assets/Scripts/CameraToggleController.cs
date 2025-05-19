using UnityEngine;
using Cinemachine;

public class CameraToggleController : MonoBehaviour
{
    public CinemachineVirtualCamera thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;

    private bool isFirstPerson = false;

    void Start()
    {
        SetCameraMode(isFirstPerson);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            SetCameraMode(isFirstPerson);
        }
    }

    void SetCameraMode(bool firstPerson)
    {
        firstPersonCam.Priority = firstPerson ? 20 : 0;
        thirdPersonCam.Priority = firstPerson ? 0 : 20;

        Cursor.lockState = firstPerson ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !firstPerson;
    }
}