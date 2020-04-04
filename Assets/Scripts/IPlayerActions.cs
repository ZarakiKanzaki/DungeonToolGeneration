using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Interface defining messages sent to the player
/// </summary>
public interface IPlayerActions
{
    float Energy { get; set; }
}
