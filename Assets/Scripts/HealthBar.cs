using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int TargetValue { get; set; }
    public bool ShouldAnimate { get; set; }

    private int _actualValue = 100;
    private Image _foregroundImage;
    private bool _isForegroundImageNotNull;

    private void Update()
    {
        if (ShouldAnimate)
        { 
            if (_actualValue < TargetValue)
            {
                _actualValue++;
            }
            else if (_actualValue > TargetValue)
            {
                _actualValue--;
            }
        }
        else
        {
            _actualValue = TargetValue;
        }

        if (_isForegroundImageNotNull)
        {
            _foregroundImage.fillAmount = _actualValue / 100f;
        }
    }

    private void Awake()
    {
        _foregroundImage = gameObject.GetComponent<Image>();
        _isForegroundImageNotNull = _foregroundImage != null;
    }
}
