using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSequence : MonoBehaviour
{
    public ExplosionController explosion;
    public CameraShake cameraShake;

    void Start()
    {
        EventManager.playerDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        explosion.Explode();
        cameraShake.enabled = true;
    }

    void OnDestroy()
    {
        EventManager.playerDeath -= OnPlayerDeath;
    }
}
