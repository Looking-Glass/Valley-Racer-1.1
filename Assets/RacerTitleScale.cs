using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RacerTitleScale : MonoBehaviour
{

    [Range(1f, 10f)]
    public float scale;

    void Update()
    {
        this.transform.localScale = new Vector3(
            scale,
            scale,
            1
            );
    }
}
