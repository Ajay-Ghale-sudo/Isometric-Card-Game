using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private GameStateMachine _stateMachine;
    private CardFactory _cardFactory;

    private void Start()
    {
        _stateMachine = new GameStateMachine();
        _cardFactory = GetComponent<CardFactory>();
        _stateMachine.ChangeState(new PlayerDrawState(_stateMachine, this, _cardFactory));
    }
}
