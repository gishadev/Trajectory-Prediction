using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PredictionManager : MonoBehaviour
{
    #region Singleton
    public static PredictionManager Instance { private set; get; }
    #endregion

    // PUBLIC_FIELDS //
    public int maxIterations;
    public Transform objectsParent;

    // PRIVATE_FIELDS //
    Scene currentScene;
    Scene predictionScene;

    PhysicsScene currentPhysicsScene;
    PhysicsScene predictionPhysicsScene;

    GameObject dummy;
    List <GameObject> dummyObjects = new List<GameObject>();

    // COMPONENTS //
    LineRenderer lr;

    void Awake()
    {
        Instance = this;
        lr = GetComponent<LineRenderer>();
    }

    void Start()
    {
        Physics.autoSimulation = false;

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene();

        CopyObjectsToSimulation();
    }

    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
    }

    public void Predict(GameObject prefab, Vector3 origin, Vector3 force)
    {
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {
            if (dummy == null)
            {
                dummy = Instantiate(prefab);
                SceneManager.MoveGameObjectToScene(dummy, predictionScene);
            }

            dummy.transform.position = origin;
            dummy.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            lr.positionCount = 0;
            lr.positionCount = maxIterations;

            for (int i = 0; i < maxIterations; i++)
            {
                predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
                lr.SetPosition(i, dummy.transform.position);
            }

            Destroy(dummy);
        }
    }

    void CopyObjectsToSimulation()
    {
        foreach (Transform t in objectsParent)
        {
            if (t.gameObject.GetComponent<Collider>() != null)
            {
                GameObject fakeT = Instantiate(t.gameObject);
                fakeT.transform.position = t.position;
                fakeT.transform.rotation = t.rotation;
                Renderer fakeR = fakeT.GetComponent<Renderer>();
                if (fakeR)
                    fakeR.enabled = false;
                
                SceneManager.MoveGameObjectToScene(fakeT, predictionScene);
                dummyObjects.Add(fakeT);
            }
        }
    }
}
