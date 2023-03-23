using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public Effect poof;
    public Effect pickup;

    static EffectSpawner instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Poof(Vector3 position) => SpawnEffect(instance?.poof, position);
    public static void Pickup(Vector3 position) => SpawnEffect(instance?.pickup, position);

    static void SpawnEffect(Effect effect, Vector3 position)
    {
        if (instance && effect) Instantiate(effect, position, Quaternion.identity);
    }
}
