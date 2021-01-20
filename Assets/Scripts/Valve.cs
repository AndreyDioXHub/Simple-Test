using UnityEngine;

public class Valve : MonoBehaviour
{
    [SerializeField]
    private float _angle;
    [SerializeField]
    private Transform _anchor;
    private Vector3 _lustOfPoint;
    // Start is called before the first frame update
    void Start()
    {
        float result = 0;

        result = ScalarMultiply(new Vector3(1,3,0), new Vector3(2, 1, 0)) / (VectorModule(new Vector3(1, 3, 0)) * VectorModule(new Vector3(2, 1, 0)));

        result = Mathf.Acos(result) * 180/Mathf.PI;
        Debug.Log(result);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyAngle();
    }

    public void ApplyAngle()
    {
        _anchor.localEulerAngles = new Vector3(0, _angle, 0);
    }

    public void SetContact(Vector3 position)
    {
        _lustOfPoint = position - _anchor.position;
    }

    public void SetValveRotation(Vector3 position)
    {
        float result = 0;
        position = position - _anchor.position;

        if (_lustOfPoint != position)
        {
            result = ScalarMultiply(position, _lustOfPoint) / (VectorModule(position) * VectorModule(_lustOfPoint));
        }

        result = Mathf.Acos(result) * 180 / Mathf.PI;

        _angle = result;
    }

    private float ScalarMultiply(Vector3 vector1, Vector3 vector2)
    {
        float result = 0;

        result = vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;

        return result;
    }

    private float VectorModule(Vector3 vector)
    {
        float module = 0;
        module = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        return module;
    }
}
