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
    public string nextScene;

    public Effect poof;
    public Effect pickup;
    public Effect hit;
    public Effect down;

    public AudioClip clipDefeat;
    public AudioClip clipVictory;

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

    void Start()
    {
        
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
                    audioSource.clip = defeat ? clipDefeat : clipVictory;
                    audioSource.loop = false;
                    audioSource.Play();
                    BlackScreen.FadeIn(Player.LastPosition);
                    Time.timeScale = 0;
                }
                break;
            case GameState.Fading:
                if (!BlackScreen.InProcess && !audioSource.isPlaying)
                {
                    if (defeat)
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    else
                        SceneManager.LoadScene(nextScene);
                }
                break;
        }
    }

    public static void EndScene(bool defeat) => ptr?._EndScene(defeat);
    public static void Poof(Vector3 position) => SpawnEffect(ptr?.poof, position);
    public static void Pickup(Vector3 position) => SpawnEffect(ptr?.pickup, position);
    public static void Hit(Vector3 position) => SpawnEffect(ptr?.hit, position);
    public static void DownSound(Vector3 position) => SpawnEffect(ptr?.down, position);

    static void SpawnEffect(Effect effect, Vector3 position)
    {
        if (ptr && effect) Instantiate(effect, position, Quaternion.identity);
    }

    public static void PlayClip(AudioClip clip)
    {
        if (ptr && clip) ptr.audioSource.PlayOneShot(clip);
    }

    public void _EndScene(bool defeat)
    {
        state = GameState.Ending;
        timer = defeat ? 1f : 0f;
        this.defeat = defeat;
        ptr.audioSource.Stop();
    }
}
