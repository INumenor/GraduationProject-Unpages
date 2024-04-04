using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Camera _mainCamera;

    private bool _isInActivationDelay;
    private void Start()
    {
        if(Runner.LocalPlayer == Object.StateAuthority) 
        {
           _playerCamera.gameObject.SetActive(true);
            GameObject gameObject = GameObject.FindGameObjectWithTag("TopDowmCamera");
            _mainCamera = gameObject.GetComponent<Camera>();
        }
    }


    public void Camera()
    {
        if (_isInActivationDelay) return;
        CameraChange();
        StartActivationDelay();
    }


    public void CameraChange()
    {
        if (_playerCamera.enabled)
        {
            _playerCamera.enabled = false;
            _mainCamera.enabled = true;
        }
        else
        {
            _playerCamera.enabled = true;
            _mainCamera.enabled = false;
        }
    }
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
