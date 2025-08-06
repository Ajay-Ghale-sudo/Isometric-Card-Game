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
    [SerializeField] private FocusTarget _camTarget;

    private CameraStateMachine _cameraStateMachine;
    public UnityEvent TryFocusTargetEvent;

    void Awake()
    {
        _cam = Camera.main;
        print(_cam);
        _cameraStateMachine = new CameraStateMachine();
        _cameraStateMachine.ChangeState(new TestState());
    }

    void Update()
    {
        _cameraStateMachine.Tick();
    }

    public void FocusTarget()
    {
        if (_camTarget is FocusTarget focusTarget)
        {
            FocusTargetState focusState = new FocusTargetState(_cameraStateMachine, null, _camTarget, _cam);
            _cameraStateMachine.ChangeState(focusState);
            TryFocusTargetEvent?.Invoke();
        }
        else
        {
            Debug.LogError("Camera target is not a FocusTarget ScriptableObject");
        }
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
        if (_currentState != null)
        {
            _currentState?.Enter();
        }
        else
        {
            Debug.LogError("Camera attempted transition to invalid state.");
            Application.Quit();
        }
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
    private Transform _moveTarget;
    private UnityEvent OnFocusTargetEvent;

    public FocusTargetState(CameraStateMachine cameraStateMachine, GameLoop game, FocusTarget focusTarget, Camera cam)
    {
        _cameraStateMachine = cameraStateMachine;
        _game = game;
        _focusTarget = focusTarget.focusTransform;
        _camera = cam;
        if (focusTarget.moveTransform != null) _moveTarget = focusTarget.moveTransform;
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
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _moveTarget.position, Time.deltaTime * 10);
        Vector3 direction = _focusTarget.position - _moveTarget.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, targetRotation, Time.deltaTime * 10);
    }
}

public class TestState : IState
{
    public void Enter()
    {
        Debug.Log("Entering test state.");
    }

    public void Exit()
    {
        Debug.Log("Exiting test state.");
    }

    public void Tick()
    {
        
    }
}
