using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Image restartProgress;
    [SerializeField] private float restartTime = 2f;
    [SerializeField] private GameObject restartPanel;
    
    private Controls _controls;
    private float _currentRestartTime;
    private bool _isRestarting;
    
    private void Awake()
    {
        _controls = new Controls();
        _controls.Game.Enable();
        
        _controls.Game.Restart.started += OnRestartStarted;
        _controls.Game.Restart.canceled += OnRestartCanceled;
        
        restartPanel.SetActive(false);
    }
    
    private void OnDestroy()
    {
        _controls.Game.Restart.started -= OnRestartStarted;
        _controls.Game.Restart.canceled -= OnRestartCanceled;
        _controls.Dispose();
    }
    
    private void Update()
    {
        if (!_isRestarting) return;
        _currentRestartTime += Time.deltaTime;
        restartProgress.fillAmount = _currentRestartTime / restartTime;
            
        if (_currentRestartTime >= restartTime)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    private void OnRestartStarted(InputAction.CallbackContext context)
    {
        _isRestarting = true;
        _currentRestartTime = 0f;
        restartPanel.SetActive(true);
    }
    
    private void OnRestartCanceled(InputAction.CallbackContext context)
    {
        _isRestarting = false;
        restartPanel.SetActive(false);
    }
}
