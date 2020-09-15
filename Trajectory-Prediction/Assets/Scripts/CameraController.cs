using UnityEngine;

public class CameraController : MonoBehaviour
{
    // PUBLIC_FIELDS //
    public float rotationMultiplier;
    public float smoothness;

    // PRIVATE_FIELDS //
    float rotY = 0f;

    void Update()
    {
        if (Input.GetMouseButton(1))
            rotY += Input.GetAxis("Mouse X") * rotationMultiplier;

        Quaternion target = Quaternion.Euler(Vector3.up * rotY);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime / smoothness);
    }
}
