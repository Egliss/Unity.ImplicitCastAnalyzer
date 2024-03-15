namespace UnityEngine;

public class Vector2
{
    public float x;
    public float y;
    
    public static implicit operator Vector3(Vector2 v)
    {
        return new Vector3 { x = v.x, y = v.y, z = (float)0 };
    }
}
public class Vector3
{
    public float x;
    public float y;
    public float z;
    
    public static implicit operator Vector2(Vector3 v)
    {
        return new Vector2 { x = v.x, y = v.y };
    }
}
