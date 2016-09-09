using UnityEngine;
using System.Collections;

public class SphereTimedFlow : TimedFlow
{
    public ParticleSystem confetti;
    public ParticleSystem ring;
    public float speed = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines(); //so it won't repeat actions
            DoAct();
        }
    }

    public override void OnBeginAct()
    {
        confetti.Emit(100);
    }

    public override void OnContinueAct()
    {
        if (transform.position.x > 4f)
            speed = -Mathf.Abs(speed);
        if (transform.position.x < -4f)
            speed = Mathf.Abs(speed);
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public override void OnEndAct()
    {
        ring.Emit(100);
    }
}
