using UnityEngine;
using System.Collections;

public class TimedFlow : MonoBehaviour
{
    public virtual void OnBeginAct() { }
    public virtual void OnContinueAct() { }
    public virtual void OnEndAct() { }
    public float duration;
    float timeStarted;

    public void DoAct()
    {
        Act(duration);
    }

    public void DoAct(float customDuration)
    {
        Act(customDuration);
    }

    void Act(float dur)
    {
        timeStarted = Time.time;
        var toStop = StartCoroutine(ContinuedAct());
        StartCoroutine(ContinuedActTimer(toStop, dur));
    }

    IEnumerator ContinuedAct()
    {
        OnBeginAct();
        while (true)
        {
            yield return new WaitForEndOfFrame();
            OnContinueAct();
        }
    }

    IEnumerator ContinuedActTimer(Coroutine toStop, float dur)
    {
        yield return new WaitForSeconds(dur);
        timeStarted = Time.time - dur;
        StopCoroutine(toStop);
        OnContinueAct();
        OnEndAct();
    }


    /// <summary>
    /// Returns time in seconds since the flowstarted.
    /// </summary>
    /// <returns>Time in seconds (dependent on timescale)</returns>
    public float GetTimeSinceStart()
    {
        var timeSince = Time.time - timeStarted;
        if (Mathf.Approximately(timeSince, duration))
            timeSince = duration;
        return timeSince;
    }


    /// <summary>
    /// Returns the time since the flow started, normalized between 0-1. Optionally, feed it an exponent to smooth it by (0.5f will ease out, 2f will ease in, etc.)
    /// </summary>
    /// <param name="curveExponent">Exponent to smooth it by (0.5f will ease out, 2f will ease in, etc.)</param>
    /// <returns>Time since the flow started, normalized between 0-1</returns>
    public float GetTimeNormalized(float curveExponent = 1)
    {
        var timeSince = Time.time - timeStarted;

        if (Mathf.Approximately(timeSince, duration))
            timeSince = duration;

        timeSince = duration != 0 ? timeSince / duration : 0;

        if (curveExponent != 1)
            timeSince = Mathf.Pow(timeSince, curveExponent);

        return timeSince;
    }
}
