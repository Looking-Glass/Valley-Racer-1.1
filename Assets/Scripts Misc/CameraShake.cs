using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration;
    Vector3 heldPosition;
    public float startingStrength;
    float strength;
    float timer;

    void OnEnable()
    {
        timer = 0;
        heldPosition = transform.position;
    }

    void Update()
    {
        if (timer > duration)
            enabled = false;

        timer += Time.deltaTime;

        var x1 = Random.Range(-strength, strength);
        var y1 = Random.Range(-strength, strength);
        var z1 = Random.Range(-strength, strength);

        transform.position = heldPosition + new Vector3(x1, y1, z1);

        strength = Mathf.Lerp(startingStrength, 0f, timer / (duration != 0 ? duration : 1f));
    }

    public void Shake()
    {
        enabled = true;
    }
}
