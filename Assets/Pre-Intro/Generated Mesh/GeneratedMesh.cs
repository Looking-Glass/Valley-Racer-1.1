using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GeneratedMesh : MonoBehaviour
{
    Mesh mesh;
    MeshFilter mf;
    int counter;
    float timer;
    public float interval = 0.5f;

    void Start()
    {
        //Setup
        mesh = new Mesh();
        mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;
    }

    void Update()
    {
        //don't do this more than 100 times
        if (counter == 100)
            return;

        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;
            counter += 1;

            var v1 = new Vector3(counter - 1, 0);
            var v2 = new Vector3(counter - 1, 1);
            var v3 = new Vector3(counter, 1);
            var v4 = new Vector3(counter, 0);

            //if the counter is odd, make a quad. if even make a tri
            if (counter % 2 != 0)
                GMesh.MakeQuad(mf.sharedMesh, v1, v2, v3, v4);
            else
                GMesh.MakeTri(mf.sharedMesh, v1, v2, v3);
        }
    }
}
