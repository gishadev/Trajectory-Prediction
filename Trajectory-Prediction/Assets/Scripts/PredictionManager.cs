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
    GameObject dummyObjectsParent;

    // COMPONENTS //
    LineRenderer lr;

    void Awake()
    {
        Instance = this;
        lr = GetComponent<LineRenderer>();

        Physics.autoSimulation = false;
        ApplyScenes();
        CopyObjectsToSimulation();

        lr.enabled = true;
    }

    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
    }

    void ApplyScenes()
    {
        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene();
    }

    public void Predict(GameObject subject, Vector3 origin, Vector3 force)
    {
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {
            if (dummy == null)
            {
                dummy = Instantiate(subject);
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
        dummyObjectsParent = Instantiate(objectsParent.gameObject, objectsParent.position, objectsParent.rotation);
        foreach (Transform t in dummyObjectsParent.GetComponentsInChildren<Transform>())
        {
                Renderer fakeR = t.GetComponent<Renderer>();
                if (fakeR)
                    fakeR.enabled = false;
        }

        SceneManager.MoveGameObjectToScene(dummyObjectsParent, predictionScene);
    }
}
