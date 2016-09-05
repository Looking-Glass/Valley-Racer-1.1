using UnityEngine;

public class SpecialRing : MonoBehaviour
{
    public GameObject specialSprite;
    public Transform specialRotator;
    public Transform[] sprites;
    public Transform[] ghosts;
    public int numberOfSprites;
    public float degreesPerSecond;
    
    void Start()
    {
        sprites = new Transform[numberOfSprites];
        ghosts = new Transform[numberOfSprites];

        for (var i = 0; i < numberOfSprites; i++)
        {
            //Make sprite
            var newSpecialSprite = (GameObject)Instantiate(specialSprite, transform.position, Quaternion.identity);
            newSpecialSprite.transform.parent = transform;
            newSpecialSprite.GetComponent<SpriteRenderer>().enabled = true;
            sprites[i] = newSpecialSprite.transform;

            //Determine ghost position
            float angleRadians = (float)i / numberOfSprites;
            angleRadians *= Mathf.PI * 2;
            var ghostPosition = new Vector3
                (Mathf.Cos(angleRadians), 0, Mathf.Sin(angleRadians));

            //Make ghost
            var ghostGameObject = new GameObject("ghost " + i);
            ghostGameObject.transform.parent = specialRotator;
            ghostGameObject.transform.localPosition = ghostPosition;
            ghosts[i] = ghostGameObject.transform;
        }
    }
    
    void Update()
    {
        //Rotate rotator
        specialRotator.localEulerAngles += Vector3.up * degreesPerSecond * Time.deltaTime;

        //Put the sprite's position at it's paired ghost's position
        for (int i = 0; i < sprites.Length; i++)
            sprites[i].position = ghosts[i].position;
    }
}