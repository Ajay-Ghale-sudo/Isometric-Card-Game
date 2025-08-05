using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{ PlayerTurn, EnemyTurn, Busy, GameOver }
public enum TurnPhase { Play, Attack, Draw, End }
public class GameStateManager : MonoBehaviour
{
    public GameState CurrentGameState { get; set; }
    public TurnPhase CurrentTurnPhase { get; set; }
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
        
    }

    public void StartEnemyTurn()
    {
        
    }

    private IEnumerator HandleTurnPhases()
    {
        SetPhase(TurnPhase.Draw);
        yield return new WaitForSeconds(0.5f);
        
        SetPhase(TurnPhase.Play);
        yield return new WaitUntil(() => PlayerEndsPlayPhase());
        
        SetPhase(TurnPhase.Attack);
        yield return new WaitUntil(() => PlayerEndsAttackPhase());
        
        SetPhase(TurnPhase.End);
        yield return new WaitForSeconds(0.5f);
        
        StartEnemyTurn();
    }

    private IEnumerator HandleTurnPhase()
    {
        yield return new WaitForSeconds(0.5f);
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
