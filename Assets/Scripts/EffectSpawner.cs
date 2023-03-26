using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public Effect poof;
    public Effect pickup;
    public Effect hit;
    public Effect down;

    static EffectSpawner ptr = null;

    // Start is called before the first frame update
    void Awake()
    {
        ptr = this;
    }

    void OnDestroy()
    {
        ptr = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Poof(Vector3 position) => SpawnEffect(ptr?.poof, position);
    public static void Pickup(Vector3 position) => SpawnEffect(ptr?.pickup, position);
    public static void Hit(Vector3 position) => SpawnEffect(ptr?.hit, position);
    public static void Down(Vector3 position) => SpawnEffect(ptr?.down, position);

    static void SpawnEffect(Effect effect, Vector3 position)
    {
        if (ptr && effect) Instantiate(effect, position, Quaternion.identity);
    }
}
