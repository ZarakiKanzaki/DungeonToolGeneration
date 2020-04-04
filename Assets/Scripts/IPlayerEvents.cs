using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Interface defining messages sent by the player
/// </summary>
public interface IPlayerEvents : IEventSystemHandler
{
    void OnPlayerHurt(int newHealth);
    void OnPlayerPowerUp(float energy);
    void OnPlayerReachedExit();
}
