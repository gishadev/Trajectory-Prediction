using UnityEngine;

public class Turret : MonoBehaviour
{
    // PUBLIC_FIELDS //
    public float rotationSpeed;
    public float maxXRotation;
    public float minXRotation;

    // PRIVATE_FIELDS //
    float yRot,xRot;

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        xRot += -input.z * rotationSpeed;
        yRot += input.x * rotationSpeed;

        xRot = Mathf.Clamp(xRot, -maxXRotation, -minXRotation);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}
