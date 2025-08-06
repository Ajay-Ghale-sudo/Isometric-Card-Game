using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private Scene _scene;
    [SerializeField] private ScriptableObject _camTarget;

    void Start()
    {
        _cam = Camera.main;
    }
}

[Serializable]
public class CameraStateMachine
{
    private IState _currentState;

    private void Start()
    {
        
    }

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }
}

public class FocusTargetState : IState
{
    private CameraStateMachine _cameraStateMachine;
    private GameLoop _game;
    private Camera _camera;
    private Transform _focusTarget;
    private Transform _transform;
    private UnityEvent OnFocusTargetEvent;

    public FocusTargetState(CameraStateMachine cameraStateMachine, GameLoop game)
    {
        _cameraStateMachine = cameraStateMachine;
        _game = game;
    }
    
    public void Enter()
    {
        Debug.Log("Camera entered focus target state.");
    }

    public void Exit()
    {
        Debug.Log("Camera exited focus target state.");
    }

    public void Tick()
    {
        
    }
}
