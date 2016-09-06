using UnityEngine;

public class MouseControls : MonoBehaviour
{
    public ObjectPool objectPool;
    public float interval = 0.01f;
    float timer;

    void Update()
    {
        if (Input.GetMouseButton(0))
            CreateCube();
    }

    void CreateCube()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;

            var pos = Input.mousePosition;
            pos += transform.position - Camera.main.transform.position;
            pos = Camera.main.ScreenToWorldPoint(pos);

            var obj = objectPool.ActivateObject();
            obj.transform.position = pos;
            obj.transform.eulerAngles = new Vector3
                (Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
    }
}
