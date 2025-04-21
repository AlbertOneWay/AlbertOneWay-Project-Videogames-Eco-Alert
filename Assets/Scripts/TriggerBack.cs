using System;
using UnityEngine;

public class TriggerBack : MonoBehaviour
{
    private bool _isShowingPopup = false;
    public GameObject Popup;

    private void Awake()
    {
        _isShowingPopup = false;
        Popup.gameObject.SetActive(false);
    }

    public void ShowPoup()
    {
        if (_isShowingPopup)
        {
            _isShowingPopup = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Popup.gameObject.SetActive(false);
        }
        else
        {
            _isShowingPopup = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Popup.gameObject.SetActive(true);
        }
    }

    public void BackToLobby()
    {
        SceneLoader.Instance.ReturnToLobby();
    }
}
