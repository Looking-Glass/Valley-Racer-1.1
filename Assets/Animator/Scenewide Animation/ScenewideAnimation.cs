using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The point of the ScenewideAnimation class is to animate any independent objects in the scene with a single Animator.
/// 
/// You record the animation in the editor using empty Game Objects (referred to as surrogates).
/// 
/// Each of these surrogates is a stand-in for an actual Game Object in the scene.
/// 
/// (Tip: While animating, to help visualize the animation, make the actual Game Object a child of the surrogate, then return it to it's normal position in the hierarchy when done animating.)
/// 
/// You list the surrogate pair as a 2-part array in the class definition.
/// 
/// Add a new function to the class with the same name as the animation state (therefore animation states SHOULD NOT have spaces!!)
/// 
/// Inside the definition of this function, set surrogatePair[0] = surrogatePair[1] properties, etc.
/// 
/// To play an animation, start the coroutine Animate and give it the animation name as an argument.
/// 
/// If you want particular things to happen at certain points in the animation, add Events to the animation timeline.
/// 
/// *****
/// IMPORTANT: animation states should not have spaces, because the same name for animation states should also be used for functions.
/// *****
/// </summary>
public class ScenewideAnimation : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Animate(string AnimationName)
    {
        //Play the animation from the start
        animator.Play(AnimationName, -1, 0f);

        //Wait a frame because isName() will not return true until next frame.
        yield return new WaitForEndOfFrame();

        //While the animation is playing and while it's not completed
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName) && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            Invoke(AnimationName, 0);
            yield return new WaitForEndOfFrame();
        }

        //Do one final set of what happens in the loop.
        Invoke(AnimationName, 0);
    }
}
