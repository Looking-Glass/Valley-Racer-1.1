using UnityEngine;

public class CubeAnimas : MonoBehaviour
{
    public float lifetime = 2f;
    float timer;
    float degPerSecond;
    Vector3 axis;

    void Awake()
    {
        Debug.Log("Awake fired in the " + name);
    }

    void Start()
    {
        Debug.Log("Start fired in the " + name);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable fired in the " + name);
        axis = new Vector3
            (Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        degPerSecond = Random.Range(10f, 45f);
        timer = 0;
    }

    void Update()
    {
        var deg = degPerSecond * Time.deltaTime;
        transform.Rotate(axis, deg);

        timer += Time.deltaTime;
        if (timer > lifetime)
            gameObject.SetActive(false);
    }
}
