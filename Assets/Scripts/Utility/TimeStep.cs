using UnityEngine;

[System.Serializable]
public struct Vector3Serializable
{
    public Vector3Serializable(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public float x;
    public float y;
    public float z;

    override public string ToString()
    {
        string ret = "";
        ret += x.ToString("N1") + " ";
        ret += y.ToString("N1") + " ";
        ret += z.ToString("N1");
        return ret;
    }
}

[System.Serializable]
struct QuaternionSerializable
{
    public QuaternionSerializable(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }
    public float x;
    public float y;
    public float z;
    public float w;
}

[System.Serializable]
public class TimeStep {

    private Vector3Serializable position;
    private QuaternionSerializable rotation;
    private Vector3Serializable velocity;
    public float time;
    public float speed;

    public string PositionToString()
    {
        string ret = "";
        ret += position.x.ToString("N1") + " ";
        ret += position.y.ToString("N1") + " ";
        ret += position.z.ToString("N1") + " ";
        return ret;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(position.x, position.y, position.z);
    }

    public void SetPosition(Vector3 v)
    {
        position = new Vector3Serializable(v);
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(velocity.x, velocity.y, velocity.z);
    }

    public void SetVelocity(Vector3 v)
    {
        velocity = new Vector3Serializable(v);
    }

    public Quaternion GetRotation()
    {
        return new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
    }

    public void SetRotation(Quaternion q)
    {
        rotation = new QuaternionSerializable(q);
    }

}
