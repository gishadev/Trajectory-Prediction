using System;
using UnityEngine;

public class VFXStorage : MonoBehaviour
{
    #region Singleton
    public static VFXStorage Instance { private set; get; }
    #endregion

    public Effect[] effects;

    void Awake()
    {
        Instance = this;
    }
}

[Serializable]
public class Effect
{
    public string name;
    public GameObject prefab;
}