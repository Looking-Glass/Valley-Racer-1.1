using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSequence : MonoBehaviour
{
    public ExplosionController explosion;
    public CameraShake cameraShake;
    public SpinDegPerSecond hypercubeSpin;
    public Transform hypercubeTransform;
    public AnimationCurve hypercubeTransformCurve;
    public FollowBiker followBiker;
    public SpriteRenderer sky;
    public SettingChanges settingChanges;
    public GameObject biker;

    void Start()
    {
        EventManager.playerDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        explosion.Explode();
        cameraShake.Shake();
        followBiker.enabled = false;
        biker.SetActive(false);
        FindObjectOfType<HighScoreSkip>().enabled = true;
        FindObjectOfType<TouchscreenManager>().

        StartCoroutine(HypercubeSpinTransition());
        StartCoroutine(HypercubeTransformTransition());
        StartCoroutine(SkyEffect());
    }

    IEnumerator SkyEffect()
    {
        settingChanges.enabled = false;
        sky.color = Color.white;

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        sky.enabled = false;
    }

    IEnumerator HypercubeSpinTransition()
    {
        yield return new WaitForSeconds(1f);

        hypercubeSpin.enabled = true;
        hypercubeSpin.degreesPerSecond = 0f;

        while (hypercubeSpin.degreesPerSecond < 20f)
        {
            hypercubeSpin.degreesPerSecond += Time.deltaTime * 4f;
            yield return new WaitForEndOfFrame();
        }

        hypercubeSpin.degreesPerSecond = 20f;
    }

    IEnumerator HypercubeTransformTransition()
    {
        yield return new WaitForSeconds(1f);

        var initHypercubePos = hypercubeTransform.position;
        var initHypercubeScale = hypercubeTransform.localScale;
        

        var finalHypercubePos = hypercubeTransform.position - Vector3.back;
        var finalHypercubeScale = Vector3.Scale(hypercubeTransform.localScale, Vector3.one * 3f);

        var timer = 0f;
        var dur = 10f;

        while (timer < dur)
        {
            var lerp = hypercubeTransformCurve.Evaluate(timer / dur);
            hypercubeTransform.position = Vector3.Lerp(initHypercubePos, finalHypercubePos, lerp);
            hypercubeTransform.localScale = Vector3.Lerp(initHypercubeScale, finalHypercubeScale, lerp);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        hypercubeTransform.position = finalHypercubePos;
        hypercubeTransform.localScale = finalHypercubeScale;
    }

    void OnDestroy()
    {
        EventManager.playerDeath -= OnPlayerDeath;
    }
}
