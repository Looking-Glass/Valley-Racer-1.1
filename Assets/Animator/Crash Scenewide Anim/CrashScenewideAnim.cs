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
    public ExplosionController explosionController;
    public FollowBiker followBiker;
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

        var initBikerPos = bikerSurrogatePair[0].position;
        var initBikerRot = bikerSurrogatePair[0].rotation;

        yield return new WaitForEndOfFrame();

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("CrashAnim") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return new WaitForEndOfFrame();
            hypercubeSurrogatePair[0].position = initHypercubePos + hypercubeSurrogatePair[1].position;
            hypercubeSurrogatePair[0].rotation = initHypercubeRot * hypercubeSurrogatePair[1].rotation;
            hypercubeSurrogatePair[0].localScale = Vector3.Scale(initHypercubeScale, hypercubeSurrogatePair[1].localScale);

            bikerSurrogatePair[0].position = initBikerPos + bikerSurrogatePair[1].position;
            bikerSurrogatePair[0].rotation = initBikerRot * bikerSurrogatePair[1].rotation;

            hypercube.tubeFactor = hypercubeTubeFactor;
            hypercube.perspectiveFactor = hypercubePerspectiveFactor;
        }

        followBiker.enabled = false;

        hypercubeSurrogatePair[0].position = initHypercubePos + hypercubeSurrogatePair[1].position;
        hypercubeSurrogatePair[0].rotation = initHypercubeRot * hypercubeSurrogatePair[1].rotation;
        hypercubeSurrogatePair[0].localScale = Vector3.Scale(initHypercubeScale, hypercubeSurrogatePair[1].localScale);

        bikerSurrogatePair[0].position = initBikerPos + bikerSurrogatePair[1].position;
        bikerSurrogatePair[0].rotation = initBikerRot * bikerSurrogatePair[1].rotation;

        hypercube.tubeFactor = hypercubeTubeFactor;
        hypercube.perspectiveFactor = hypercubePerspectiveFactor;
    }

    void Explode()
    {
        explosionController.Explode();
    }

    void OnDestroy()
    {
        EventManager.playerDeath -= OnPlayerDeath;
    }
}
