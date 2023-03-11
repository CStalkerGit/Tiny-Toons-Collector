using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public AudioClip coinSound;

    Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var items = FindObjectsOfType<Item>();
        foreach(var item in items)
        {
            if (item.IsCollision(entity))
            {
                if (coinSound) GetComponent<AudioSource>().PlayOneShot(coinSound);
                Destroy(item.gameObject, 0.1f); //item.Delete();
                item.gameObject.SetActive(false);
            }
        }
    }
}
