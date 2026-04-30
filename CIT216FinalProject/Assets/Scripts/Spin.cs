using UnityEngine;

public class Spin : MonoBehaviour
{
    public enum OnAxis
    {
      X, 
      Y, 
      Z
    }
    public OnAxis axis;
    public float speed;
    private Transform tf;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = new Vector3(0f,0f,0f);
        if (axis == OnAxis.X)
        {
            rot = new Vector3(speed,0f,0f); 
            tf.Rotate(rot);
        }
        else if (axis == OnAxis.Y)
        {
            rot = new Vector3(0f, speed, 0f);
            tf.Rotate(rot);
        }
        else if (axis == OnAxis.Z)
        {
            rot = new Vector3(0f, 0f, speed);
            tf.Rotate(rot);
        }
    }
}
