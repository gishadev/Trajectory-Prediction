using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Singleton
    public static SpawnManager Instance { private set; get; }
    #endregion

    // PUBLIC_FIELDS //
    public GameObject starPrefab;
    public MeshRenderer roomRenderer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnStar();
    }

    public void SpawnStar()
    {
        Instantiate(starPrefab, GetRandomPositionInsideBox(), Quaternion.identity);
    }

    Vector3 GetRandomPositionInsideBox()
    {
        float xPos = Random.Range(roomRenderer.bounds.min.x, roomRenderer.bounds.max.x);
        float yPos = Random.Range(roomRenderer.bounds.min.y, roomRenderer.bounds.max.y);
        float zPos = Random.Range(roomRenderer.bounds.min.z, roomRenderer.bounds.max.z);

        return new Vector3(xPos, yPos, zPos);
    }
}
