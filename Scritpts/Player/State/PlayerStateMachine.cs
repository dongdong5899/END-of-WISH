using System.Collections.Generic;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    private Dictionary<PlayerStateEnum, PlayerState> stateDictionary;

    private PlayerBrain player;

    public PlayerStateMachine()
    {
        stateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();
    }

    public void Initialize(PlayerStateEnum playerStateEnum, PlayerBrain player)
    {
        this.player = player;
        CurrentState = stateDictionary[playerStateEnum];
        CurrentState.Enter();
    }

    public void StateChange(PlayerStateEnum changeState)
    {
        if (player.CanStateChangeable == false) return;

        CurrentState.Exit();
        CurrentState = stateDictionary[changeState];
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum playerEnum, PlayerState playerState)
    {
        stateDictionary.Add(playerEnum, playerState);
    }
}
