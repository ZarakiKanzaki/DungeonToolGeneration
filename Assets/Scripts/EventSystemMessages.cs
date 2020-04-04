using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Component for broadcasting game messages.
/// </summary>
public class EventSystemMessages : MonoBehaviour
{
    private static EventSystemMessages _main;
    private readonly List<GameObject> _listeners = new List<GameObject>();

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Awake ()
    {
        // Check only one instance of EventSystemMessages exists
        if (_main == null)
        {
            _main = this;
        }
        else
        {
            Debug.LogWarning ("EventSystemMessages re-creation attempted, destroying the new one");
            Destroy (gameObject);
        }
    }

    /// <summary>
    /// Add Listener for Event System Messages
    /// </summary>
    public void AddListener(GameObject listener)
    {
        if (_listeners.Contains(listener))
        {
            return;
        }
        
        _listeners.Add(listener);
    }

    /// <summary>
    /// Invoke OnPlayerPowerUp Event
    /// </summary>
    /// <param name="energy"></param>
    public void OnPlayerPowerUp(float energy)
    {
        foreach (var listener in _listeners)
        {
            ExecuteEvents.Execute<IPlayerEvents>(listener,null,(x, y) => x.OnPlayerPowerUp(energy));
        }
    }

    /// <summary>
    /// Invoke OnPlayerReachedExit Event
    /// </summary>
    public void OnPlayerReachedExit()
    {
        foreach (var listener in _listeners)
        {
            ExecuteEvents.Execute<IPlayerEvents>(listener,null,(x, y) => x.OnPlayerReachedExit());
        }
    }
}
