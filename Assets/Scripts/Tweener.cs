using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tweener : MonoBehaviour
{
    [SerializeField]
    private Vector3 Begin;
    [SerializeField]
    private Vector3 End;
    [SerializeField]
    private float Duration;
    [SerializeField]
    private AnimationCurve Curve;
    private float Alpha = 0;
    private bool bIsPlaying = false;
    private bool bIsReverse = false;
    private UnityEvent OnTweenCompleted = new UnityEvent();
    public UnityEvent OnComplete => OnTweenCompleted;
    public void SetUp(Vector3 t_start, Vector3 t_end, float t_duration, AnimationCurve t_curve = null)
    {
        Begin = t_start;
        End = t_end;
        Duration = t_duration;
        if (t_curve != null)
            Curve = t_curve;
    }
    public void Play()
    {
        Alpha = 0;
        bIsPlaying = true;
        bIsReverse = false;
    }
    public void Pause()
    {
        bIsPlaying = false;
    }
    public void Resume()
    {
        if (Alpha != 0)
            bIsPlaying = true;
    }
    public void Reverse()
    {
        Alpha = 0;
        bIsPlaying = true;
        bIsReverse = true;
    }
    public void Stop()
    {
        bIsPlaying = false;
        Alpha = 0;
    }
    private void Update()
    {
        if (!bIsPlaying)
        {
            return;
        }
        if (Alpha / Duration > 1)
        {
            OnTweenCompleted?.Invoke();
            bIsPlaying = false;
            Alpha = 0;
            return;
        }
        if (bIsReverse)
        {
            gameObject.transform.SetPositionAndRotation(Vector3.Lerp(End, Begin, Curve.Evaluate(Alpha / Duration)), Quaternion.identity); //= Vector3.Lerp(End, Begin, Curve.Evaluate(Alpha / Duration));
        }
        else
        {
            gameObject.transform.SetLocalPositionAndRotation(Vector3.Lerp(Begin, End, Curve.Evaluate(Alpha / Duration)), Quaternion.identity); //gameObject.transform.position = Vector3.Lerp(Begin, End, Curve.Evaluate(Alpha / Duration));
        }
        Alpha += Time.deltaTime;
    }
}
