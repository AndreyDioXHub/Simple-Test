using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Power = 4;

    private Camera _mainCamera;

    private CameraStateActions _m_TargetCameraState = new CameraStateActions();
    private CameraStateActions _m_InterpolatingCameraState = new CameraStateActions();

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        _m_TargetCameraState.SetFromTransform(transform);
        _m_InterpolatingCameraState.SetFromTransform(transform);
    }

    Vector3 GetDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        return direction;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "ipadbutton")
                {
                    hit.transform.gameObject.GetComponent<IpadButton>().MyIpad.IpadModel.SetActive(false);
                }

                if (hit.collider.tag == "valve")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.transform.gameObject.GetComponent<Valve>().SetContact(hit.point);
                        hit.transform.gameObject.GetComponent<Valve>().SetValveRotation(hit.point);
                    }
                    hit.transform.gameObject.GetComponent<Valve>().SetValveRotation(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        if (Input.GetMouseButton(1))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * -1);

            _m_TargetCameraState.yaw += mouseMovement.x;
            _m_TargetCameraState.pitch += mouseMovement.y;
        }

        var translation = GetDirection() * Time.deltaTime;
        
        translation *= Mathf.Pow(2.0f, Power);

        _m_TargetCameraState.Translate(translation);
        var positionLerpPct = 1f - Time.deltaTime;
        var rotationLerpPct = 1f - Time.deltaTime;

        _m_InterpolatingCameraState.LerpTowards(_m_TargetCameraState);
        _m_InterpolatingCameraState.UpdateTransform(transform);
    }
}

public class CameraStateActions
{
    public float yaw;
    public float pitch;
    public float roll;
    public float x;
    public float y;
    public float z;

    public void SetFromTransform(Transform t)
    {
        pitch = t.eulerAngles.x;
        yaw = t.eulerAngles.y;
        roll = t.eulerAngles.z;
        x = t.position.x;
        y = t.position.y;
        z = t.position.z;
    }

    public void Translate(Vector3 translation)
    {
        Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

        x += rotatedTranslation.x;
        y += rotatedTranslation.y;
        z += rotatedTranslation.z;
    }

    public void LerpTowards(CameraStateActions target)
    {
        yaw = Mathf.Lerp(yaw, target.yaw, 0.2f);
        pitch = Mathf.Lerp(pitch, target.pitch, 0.2f);
        roll = Mathf.Lerp(roll, target.roll, 0.2f);

        x = Mathf.Lerp(x, target.x, 0.2f);
        y = Mathf.Lerp(y, target.y, 0.2f);
        z = Mathf.Lerp(z, target.z, 0.2f);
    }

    public void UpdateTransform(Transform t)
    {
        t.eulerAngles = new Vector3(pitch, yaw, roll);
        t.position = new Vector3(x, y, z);
    }
}