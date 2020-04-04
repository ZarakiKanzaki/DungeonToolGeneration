using System;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject, IPlayerActions
{
    [SerializeField] private EventSystemMessages eventSystemMessages = null;
    [SerializeField] private InputHandler input = null;
    [SerializeField] private float jumpTakeOffSpeed = 7;
    [SerializeField] private float maxSpeed = 7;

    public float Energy { get; set; }
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    #region Unity Events
    
    // Use this for initialization
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    // Called when player collides with another game object
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectibleEvents = other.GetComponentInParent<ICollectibleEvents>();
        if (collectibleEvents != null)
        {
            ApplyHealthBoost(collectibleEvents.OnItemCollected());
        }

        var goalEvents = other.GetComponentInParent<IGoalEvents>();
        if (goalEvents != null)
        {
            ApplyGoal(goalEvents.OnGoalReached());
        }
    }
    
    #endregion
    
    #region Base Overrides
    
    protected override void ComputeVelocity()
    {
        var move = Vector2.zero;
        move.x = input.HorizontalAxis;

        if (input.JumpButton && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (!input.JumpButton)
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (move.x > 0.01f)
        {
            if (_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = false;
            }
        }
        else if (move.x < -0.01f)
        {
            if (_spriteRenderer.flipX == false)
            {
                _spriteRenderer.flipX = true;
            }
        }

        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
    
    #endregion

    #region Private Methods

    private void ApplyHealthBoost(HealthBoost healthBoost)
    {
        Energy = Mathf.Clamp(Energy += healthBoost.Energy, 0, 100);
        eventSystemMessages.OnPlayerPowerUp(Energy);
    }

    private void ApplyGoal(Goal goal)
    {
        switch(goal)
        {
            case Goal.Exit:
                eventSystemMessages.OnPlayerReachedExit();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
        }
    }

    #endregion
}
