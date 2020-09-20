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

    public float fullAccumulationTime = 1f;

    public float minShootForce;
    public float maxShootForce;

    // PRIVATE_FIELDS //
    float yRot, xRot;
    float shootForce;

    void Start()
    {
        PredictTrajectory();
    }

    void Update()
    {
        Rotation();

        if (Input.GetMouseButton(0))
            AccumulateForce();
        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
            shootForce = minShootForce;
        }
            
    }

    void Rotation()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        xRot += -input.z * rotationSpeed;
        yRot += input.x * rotationSpeed;

        xRot = Mathf.Clamp(xRot, -maxXRotation, -minXRotation);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);

        if (xRot != transform.rotation.eulerAngles.x || yRot != transform.rotation.eulerAngles.y)
            PredictTrajectory();
    }

    void Shoot()
    {
        Rigidbody rb = Instantiate(projectilePrefab, shotPos.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(ShootForce(), ForceMode.Impulse);
    }

    void AccumulateForce()
    {
        float accumulationSpeed = maxShootForce * (Time.deltaTime / fullAccumulationTime);
        shootForce = Mathf.Clamp(shootForce + accumulationSpeed, minShootForce, maxShootForce);
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
