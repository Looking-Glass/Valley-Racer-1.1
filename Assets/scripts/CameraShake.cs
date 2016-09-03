using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float totalTime;
    float timer;
    float strength;
    Vector3 heldPosition;
    public Transform target;

    void Update()
    {
        if (timer > 0)
        {
            var x1 = Random.Range(-strength, strength);
            var y1 = Random.Range(-strength, strength);
            var z1 = Random.Range(-strength, strength);

            if (target != null)
                target.position = heldPosition + new Vector3(x1, y1, z1);

            timer -= Time.deltaTime;
            strength = Mathf.Lerp(0f, strength, timer / totalTime);
        }
    }


    /// <summary>
    /// Attach to a gameobject (not necessarily the one that you want to shake), call this function and give it a time, strength, and a target.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="str"></param>
    /// <param name="target"></param>
    public void Shake(float t, float str, Transform target)
    {
        totalTime = t;
        timer = t;
        strength = str;
        this.target = target;
        if (target != null)
            heldPosition = target.position;
    }
}
