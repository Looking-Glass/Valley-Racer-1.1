using UnityEngine;
using System.Collections;

public class RainVelocity : MonoBehaviour
{
    MotoController moto;
    ParticleSystem ps;

    void Start()
    {
        moto = FindObjectOfType<MotoController>();
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var vo = ps.velocityOverLifetime;
        var v = moto.GetVelocity();
        print(v);
        vo.x = new ParticleSystem.MinMaxCurve(v.x);
        vo.y = new ParticleSystem.MinMaxCurve(v.y);
        vo.z = new ParticleSystem.MinMaxCurve(v.z);
        vo.enabled = true;
    }
}
