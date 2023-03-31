using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    Playing,
    Ending,
    Fading
}

public class Game : MonoBehaviour
{
    public Effect poof;
    public Effect pickup;
    public Effect hit;
    public Effect down;

    public AudioClip clipDefeat;

    AudioSource audioSource;

    static Game ptr = null;

    GameState state = GameState.Playing;
    float timer;
    bool defeat;

    // Start is called before the first frame update
    void Awake()
    {
        ptr = this;
        audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        ptr = null;
    }

    void FixedUpdate()
    {
        if (timer > -1) timer -= Time.deltaTime;

        switch (state)
        {
            case GameState.Ending:
                if (timer < 0)
                {
                    state = GameState.Fading;
                    audioSource.PlayOneShot(clipDefeat);
                    BlackScreen.FadeIn(Player.LastPosition);
                }
                break;
            case GameState.Fading:
                if (!BlackScreen.InProcess) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    public static void EndScene(bool defeat) => ptr?._EndScene(defeat);
    public static void Poof(Vector3 position) => SpawnEffect(ptr?.poof, position);
    public static void Pickup(Vector3 position) => SpawnEffect(ptr?.pickup, position);
    public static void Hit(Vector3 position) => SpawnEffect(ptr?.hit, position);
    public static void Down(Vector3 position) => SpawnEffect(ptr?.down, position);

    static void SpawnEffect(Effect effect, Vector3 position)
    {
        if (ptr && effect) Instantiate(effect, position, Quaternion.identity);
    }

    public void _EndScene(bool defeat)
    {
        state = GameState.Ending;
        timer = 1f;
        this.defeat = defeat;
    }
}
