using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorExampleScenewideAnim : ScenewideAnimation
{
    public Transform[] CubeSurrogatePair = new Transform[2];
    public Transform[] CylinderSurrogatePair = new Transform[2];
    public Transform[] SphereSurrogatePair = new Transform[2];
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(AnimatingObjects());
    }

    public IEnumerator AnimatingObjects()
    {
        //Play the animation from the start
        animator.Play("AnimatingObjects", -1, 0f);

        //Wait a frame because isName() will not return true until next frame.
        yield return new WaitForEndOfFrame();

        var SphereInitialPosition = SphereSurrogatePair[0].position;

        //While the animation is playing and while it's not completed
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("AnimatingObjects") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return new WaitForEndOfFrame();
            SphereSurrogatePair[0].position = SphereInitialPosition + SphereSurrogatePair[1].position;
            AnimatingObjectsUpdate();
        }

        //Do one final set of what happens in the loop.
        AnimatingObjectsUpdate();
    }

    void AnimatingObjectsUpdate()
    {
        CubeSurrogatePair[0].position = CubeSurrogatePair[1].position;

        CylinderSurrogatePair[0].position = CylinderSurrogatePair[1].position;
        CylinderSurrogatePair[0].rotation = CylinderSurrogatePair[1].rotation;
    }

    void SomeEvent()
    {
        Debug.Log("Some Event fired! From animate timeline!");
    }
}
