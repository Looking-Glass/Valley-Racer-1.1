using UnityEngine;

public static class VectorEdit
{
    public enum Component
    {
        x,
        y,
        z
    }

    //X
    public static Vector2 SetX(Vector2 vector, float value)
    {
        return SetComponent(vector, value, Component.x);
    }

    public static Vector3 SetX(Vector3 vector, float value)
    {
        return SetComponent(vector, value, Component.x);
    }

    //Y
    public static Vector2 SetY(Vector2 vector, float value)
    {
        return SetComponent(vector, value, Component.y);
    }

    public static Vector3 SetY(Vector3 vector, float value)
    {
        return SetComponent(vector, value, Component.y);
    }

    //Z
    public static Vector3 SetZ(Vector3 vector, float value)
    {
        return SetComponent(vector, value, Component.z);
    }

    static Vector3 SetComponent(Vector3 vector, float value, Component component)
    {
        var newVector = new Vector3(
            component == Component.x ? value : vector.x,
            component == Component.y ? value : vector.y,
            component == Component.z ? value : vector.z
            );
        
        return newVector;
    }
}