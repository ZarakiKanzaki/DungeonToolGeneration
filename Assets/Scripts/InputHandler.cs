using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    private const float MaxX = 1.0f;
    private const float Sensitivity = 3f;
    private const float Dead = 0.001f;

    private float _horizontalCurrent;
    private float _horizontalTarget;
    
    private bool _jump;
    private float _lastJumpTime;

    [SerializeField] private float maxJumpDuration = 0.2f;
    [SerializeField] private bool isMobile = true;

    #region Public Methods

    /// <summary>
    /// Horizontal Axis. Smooths the input buttons to mimic Unity's keyboard input.
    /// </summary>
    /// <remarks>https://answers.unity.com/questions/958683/using-unitys-same-smoothing-from-getaxis-on-arrow.html</remarks>
    /// <value></value>
    public float HorizontalAxis
    {
        get
        {
            _horizontalCurrent = Mathf.MoveTowards(_horizontalCurrent, _horizontalTarget, Sensitivity * Time.deltaTime);
            return Mathf.Abs(_horizontalCurrent) < Dead ? 0f : _horizontalCurrent;
        }
    }

    /// <summary>
    /// Jump Button Down. Determine if the jump button is currently down.
    /// </summary>
    public bool JumpButton
    {
        get
        {
            if (_jump && Time.time > _lastJumpTime + maxJumpDuration)
            {
                _jump = false;
            }
            
            return _jump;
        }
    }

    #endregion
    
    #region Public Event Handlers

    public void DidPressLeft(BaseEventData data)
    {
        _horizontalTarget = -MaxX;
    }

    public void DidPressRight(BaseEventData data)
    {
        _horizontalTarget = MaxX;
    }
    
    public void DidReleaseLeft(BaseEventData data)
    {
        _horizontalTarget = 0;
    }

    public void DidReleaseRight(BaseEventData data)
    {
        _horizontalTarget = 0;
    }

    public void DidPressJump(BaseEventData data)
    {
        _jump = true;
        _lastJumpTime = Time.time;
    }

    public void DidReleaseJump(BaseEventData data)
    {
        _jump = false;
    }

    #endregion

    #region Lifecycle Methods

    private void Update()
    {
        if (isMobile)
        {
            return;
        }
        
        _horizontalTarget = Input.GetAxisRaw("Horizontal");

        if (!_jump && Input.GetButton("Jump"))
        {
            _jump = true;
            _lastJumpTime = Time.time;
        }
        else if (!Input.GetButton("Jump"))
        {
            _jump = false;
        }
    }

    #endregion
}
