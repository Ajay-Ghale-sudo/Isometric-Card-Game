using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState{ PlayerTurn, EnemyTurn, Busy, GameOver }
public enum TurnPhase { Plan, Attack, Draw, End }
public class GameStateManager : MonoBehaviour
{
    public GameState CurrentGameState { get; set; }
    public TurnPhase CurrentTurnPhase { get; set; }
    public CardFactory _cardFactory;
    public static GameStateManager Instance;
    public static Action<TurnPhase> OnPhaseChanged;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        // Evaluate match begin logic here
    }

    public void StartPlayerTurn()
    {
        print($"Current game state is: {CurrentGameState} and we are transitioning to {GameState.PlayerTurn}");
        if (CurrentGameState != GameState.PlayerTurn)
        {
            _cardFactory.SpawnHand();
        }
    }

    public void StartEnemyTurn()
    {
        
    }

    private IEnumerator HandleTurnPhases()
    {
        SetPhase(TurnPhase.Draw);
        yield return new WaitForSeconds(0.5f);
        
        SetPhase(TurnPhase.Plan);
        yield return new WaitUntil(() => PlayerEndsPlayPhase());
        
        SetPhase(TurnPhase.Attack);
        yield return new WaitUntil(() => PlayerEndsAttackPhase());
        
        SetPhase(TurnPhase.End);
        yield return new WaitForSeconds(0.5f);
        
        StartEnemyTurn();
    }

    private IEnumerator HandleEnemyTurn()
    {
        yield return new WaitForSeconds(1.0f);
        // Event for AI here
    }

    private void SetPhase(TurnPhase phase)
    {
        CurrentTurnPhase = phase;
        OnPhaseChanged?.Invoke(phase);
        Debug.Log($"Phase: {phase}");
        // Event broadcasts on phase change here. Update camera, UI, etc.
    }
    
    // Placeholders. Should be an event that does this.
    private bool PlayerEndsPlayPhase() => Input.GetKeyDown(KeyCode.A);
    private bool PlayerEndsAttackPhase() => Input.GetKeyDown(KeyCode.Space);
}


public interface IState
{
    void Enter();
    void Tick();
    void Exit();
}

[Serializable]
public class GameStateMachine
{
    private IState _currentState;

    private void Start()
    {
        
    }

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }
}

public class PlayerDrawState : IState
{
    private GameStateMachine _stateMachine;
    private GameLoop _game;
    private CardFactory _cardFactory;
    private UnityEvent OnEnterDrawEvent;

    public PlayerDrawState(GameStateMachine stateMachine, GameLoop game, CardFactory cardFactory)
    {
        _stateMachine = stateMachine;
        _game = game;
        _cardFactory = cardFactory;
    }

    public void Enter()
    {
        Debug.Log("Entered draw phase.");
        _cardFactory.SpawnHand();
        _stateMachine.ChangeState(this);
    }

    public void Tick()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Exited draw phase.");
    }

}

public class PlayerPlayState : IState
{
    private GameStateMachine _stateMachine;
    private GameLoop _game;
    private bool endTurnRequested;

    public PlayerPlayState(GameStateMachine stateMachine, GameLoop game)
    {
        _stateMachine = stateMachine;
        _game = game;
    }
    public void Enter()
    {
        Debug.Log("Player play state entered.");
    }

    public void Exit()
    {
        Debug.Log("Player play state exited.");
    }

    public void Tick()
    {
        
    }
}
