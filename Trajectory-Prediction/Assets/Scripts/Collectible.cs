using UnityEngine;

public class Collectible : MonoBehaviour
{
    void Collect()
    {
        Destroy(gameObject);
        SpawnManager.Instance.SpawnStar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
            Collect();
    }
}
