using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Uncomment this while previewing in editor
//[ExecuteInEditMode]
public class InitialScenewideAnim : MonoBehaviour
{
    public Transform[] hypercubeSurrogate = new Transform[2];
    public Transform[] bikerSurrogate = new Transform[2];

    public SpriteRenderer sky;
    public BikeController bikeController;
    public FollowBiker followBiker;
    public Mountains mountains;
    public Texture2D mainHeightmap;
    public hypercubeCamera hypercube;
    public float hypercubeTubeFactor;
    public float hypercubePerspectiveFactor;

    Animator animator;

    //Uncomment to preview changes in editor
    /*
    void Update()
    {
        hypercube.tubeFactor = hypercubeTubeFactor;
        hypercube.perspectiveFactor = hypercubePerspectiveFactor;
    }
    */

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(InitialAnim());
    }

    public IEnumerator InitialAnim()
    {
        //Play the animation from the start
        animator.Play("InitialAnim", -1, 0f);

        //Wait a frame because isName() will not return true until next frame.
        yield return new WaitForEndOfFrame();

        //While the animation is playing and while it's not completed
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("InitialAnim") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return new WaitForEndOfFrame();
            UpdateSurrogates();
        }

        //Do one final set of what happens in the loop.
        UpdateSurrogates();

        //Do the last stuff
        sky.enabled = true;
        bikeController.controlsOn = true;
        followBiker.enabled = true;
        mountains.ReplaceHeightmap(mainHeightmap);

        //Start the score counting
        if (EventManager.gameStart != null)
            EventManager.gameStart();
    }

    void UpdateSurrogates()
    {
        hypercube.tubeFactor = hypercubeTubeFactor;
        hypercube.perspectiveFactor = hypercubePerspectiveFactor;

        hypercubeSurrogate[0].position = hypercubeSurrogate[1].position;
        hypercubeSurrogate[0].rotation = hypercubeSurrogate[1].rotation;
        hypercubeSurrogate[0].localScale = hypercubeSurrogate[1].localScale;

        bikerSurrogate[0].localScale = bikerSurrogate[1].localScale;

        hypercubeSurrogate[0].position =
            hypercubeSurrogate[0].position.SetY(hypercubeSurrogate[0].position.y + bikerSurrogate[0].position.y);
    }
}
