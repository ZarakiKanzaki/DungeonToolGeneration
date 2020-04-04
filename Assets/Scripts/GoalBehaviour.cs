using UnityEngine;

/// <summary>
/// Interface defining messages that can be sent to the gem.
/// </summary>
public interface IGoalEvents
{
    Goal OnGoalReached();
}

public enum Goal
{
    Exit
}

[RequireComponent(typeof(Collider2D))]
public class GoalBehaviour : MonoBehaviour, IGoalEvents
{
    [SerializeField] private Goal goalType = Goal.Exit;

    public Goal OnGoalReached()
    {
        return goalType;
    }
}
