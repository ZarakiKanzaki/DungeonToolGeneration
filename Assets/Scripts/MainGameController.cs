using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour, IPlayerEvents
{
    [SerializeField] private EventSystemMessages eventSystemMessages = null;
    [SerializeField] private HealthBar healthBar = null;
    private IPlayerActions _playerActions = null;
    
    // Use this for initialization
    private void Awake()
    {
        eventSystemMessages.AddListener(gameObject);
        
        var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
            var result = rootGameObject.GetComponentInChildren<IPlayerActions>();
            if (result == null)
            {
                continue;
            }

            _playerActions = result;
            break;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        const int energy = 100; // TODO: Retrieve from saved data.
        _playerActions.Energy = energy;
        healthBar.ShouldAnimate = false;
        healthBar.TargetValue = Convert.ToInt32(energy);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void OnPlayerHurt(int newHealth)
    {
        Debug.Log("OnPlayerHurt!");
    }

    public void OnPlayerPowerUp(float energy)
    {
        Debug.Log("OnPlayerPowerUp!");
        healthBar.ShouldAnimate = true;
        healthBar.TargetValue = Convert.ToInt32(energy);
    }

    public void OnPlayerReachedExit()
    {
        Debug.Log("OnPlayerReachedExit!");
    }
}
