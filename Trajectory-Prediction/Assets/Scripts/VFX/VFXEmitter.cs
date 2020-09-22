using System;
using UnityEngine;

public static class VFXEmitter
{
    public static void Emit(string effectName, Vector3 position)
    {
        Effect effect = Array.Find(VFXStorage.Instance.effects, x => x.name == effectName);
        GameObject.Instantiate(effect.prefab, position, effect.prefab.transform.rotation);
    }
}
