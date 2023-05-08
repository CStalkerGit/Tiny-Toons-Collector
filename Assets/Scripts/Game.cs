using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    Playing,
    Ending,
    Fading,
    Starting
}

public class Game : MonoBehaviour
{
    public string nextScene;
    public GameData gameData;

    AudioSource audioSource;

    static Game ptr = null;

    GameState state = GameState.Playing;
    float timer;
    bool defeat;

    Checkpoint lastCheckpoint;

    // Start is called before the first frame update
    void Awake()
    {
        state = GameState.Starting;
        Time.timeScale = 0;
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

    void Update()
    {
        if (timer > -1) timer -= Time.unscaledDeltaTime;

        switch (state)
        {
            case GameState.Starting:
                if (!BlackScreen.InProcess)
                {
                    state = GameState.Playing;
                    Time.timeScale = 1;
                }
                break;
            case GameState.Ending:
                if (timer < 0)
                {
                    state = GameState.Fading;
                    audioSource.clip = defeat ? gameData.clipDefeat : gameData.clipVictory;
                    audioSource.loop = false;
                    audioSource.Play();
                    BlackScreen.FadeIn(Player.LastPosition, !defeat);
                    Time.timeScale = 0;
                }
                break;
            case GameState.Fading:
                if (!BlackScreen.InProcess && !audioSource.isPlaying)
                {
                    if (defeat)
                    {
                        if (Data.lives < 1) Data.startFromCheckpoint = false; // + LoadMainMap
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    else
                    {
                        Data.startFromCheckpoint = false;
                        SceneManager.LoadScene(nextScene);
                    }
                }
                break;
        }
    }

    public static void EndScene(bool defeat) => ptr?._EndScene(defeat);
    public static void Poof(Vector3 position) => SpawnEffect(ptr?.gameData.poof, position);
    public static void Pickup(Vector3 position) => SpawnEffect(ptr?.gameData.pickup, position);
    public static void Hit(Vector3 position) => SpawnEffect(ptr?.gameData.hit, position);
    public static void DownSound(Vector3 position) => SpawnEffect(ptr?.gameData.down, position);

    public static Checkpoint LastCheckpoint => ptr?.lastCheckpoint;
    public static void SetLastCheckpoint(Checkpoint chk) => ptr.lastCheckpoint = chk;

    static void SpawnEffect(Effect effect, Vector3 position)
    {
        if (ptr && effect) Instantiate(effect, position, Quaternion.identity);
    }

    public static void PlayClip(AudioClip clip)
    {
        if (ptr && clip) ptr.audioSource.PlayOneShot(clip);
    }

    public static void SpawnPopUp(int number, Vector3 position)
    {
        if (ptr)
        {
            var obj = Instantiate(ptr.gameData.popUp, position, Quaternion.identity);
            obj.SetNumber(number);
        }
    }

    public void _EndScene(bool defeat)
    {
        state = GameState.Ending;
        timer = defeat ? 1f : 0f;
        this.defeat = defeat;
        ptr.audioSource.Stop();
    }
}
