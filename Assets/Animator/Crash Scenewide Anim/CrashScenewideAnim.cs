using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Comment this out when not editing
[ExecuteInEditMode]
public class CrashScenewideAnim : MonoBehaviour
{
    public Transform[] hypercubeSurrogatePair = new Transform[2];
    public Transform[] bikerSurrogatePair = new Transform[2];
    public hypercubeCamera hypercube;
    public float hypercubeTubeFactor;
    public float hypercubePerspectiveFactor;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        EventManager.playerDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        StartCoroutine(CrashAnim());
    }

    IEnumerator CrashAnim()
    {
        animator.Play("CrashAnim", -1, 0f);
        var initHypercubePos = hypercubeSurrogatePair[0].position;
        var initHypercubeRot = hypercubeSurrogatePair[0].rotation;
        var initHypercubeScale = hypercubeSurrogatePair[0].localScale;

        yield return new WaitForEndOfFrame();

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("CrashAnim") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    //Comment this out when not editing.
    void Update()
    {
        hypercubeSurrogatePair[0].position = hypercubeSurrogatePair[1].position;
        hypercubeSurrogatePair[0].rotation = hypercubeSurrogatePair[1].rotation;
        hypercubeSurrogatePair[0].localScale = hypercubeSurrogatePair[1].localScale;
    }

    void OnDestroy()
    {
        EventManager.playerDeath -= OnPlayerDeath;
    }
}
