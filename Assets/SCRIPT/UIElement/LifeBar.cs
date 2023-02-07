using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{ 
    [SerializeField] private float _targetValue = 1f;
    [SerializeField] private float _feedbackTargetValue = 1f;
    [SerializeField] private float _smoothtime = 0.2f;
    private Vector3 _velocity;
    private Vector3 _feedbackVelocity;

    [SerializeField] RectTransform _Foreground;
    [SerializeField] RectTransform _FeedbackBar;
    

    // Update is called once per frame
    void Update()
    {
        _Foreground.localScale = Vector3.SmoothDamp(_Foreground.localScale, new Vector3(_targetValue, 1f, 1f), ref _velocity, _smoothtime);
        _FeedbackBar.localScale = Vector3.SmoothDamp(_FeedbackBar.localScale, new Vector3(_feedbackTargetValue, 1f, 1f), ref _feedbackVelocity, _smoothtime);
    }

    public void SetValue(float pAmount)
    {
        _targetValue = pAmount;
        StartCoroutine(FeedbackBarDisplay());
    }

    IEnumerator FeedbackBarDisplay()
    {
        yield return new WaitForSeconds(_smoothtime + 0.2f);
        _feedbackTargetValue = _targetValue;
    }
}
