using UnityEngine;

public class Collectible : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }

    void Collect()
    {
        SpawnManager.Instance.SpawnStar();
        ScoreManager.Instance.Score();
        VFXEmitter.Emit("Star", transform.position);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Collect();
        }

    }
}
