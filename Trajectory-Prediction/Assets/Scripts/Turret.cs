using UnityEngine;

public class Turret : MonoBehaviour
{
    // PUBLIC_FIELDS //
    [Header("Rotation")]
    public float rotationSpeed;
    public float maxXRotation;
    public float minXRotation;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform shotPos;

    public float shootForce;

    // PRIVATE_FIELDS //
    float yRot, xRot;


    void Update()
    {
        Rotation();

        if (Input.GetMouseButtonUp(0))
            Shoot();

        PredictTrajectory();
    }

    void Rotation()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        xRot += -input.z * rotationSpeed;
        yRot += input.x * rotationSpeed;

        xRot = Mathf.Clamp(xRot, -maxXRotation, -minXRotation);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
    }

    void Shoot()
    {
        Rigidbody rb = Instantiate(projectilePrefab, shotPos.position, Quaternion.identity).GetComponent<Rigidbody>();
        VFXEmitter.Emit("Shoot", shotPos.position);
        rb.AddForce(ShootForce(), ForceMode.Impulse);
    }

    Vector3 ShootForce()
    {
        return shotPos.forward * shootForce;
    }

    void PredictTrajectory()
    {
        PredictionManager.Instance.Predict(projectilePrefab, shotPos.position, ShootForce());
    }
}
