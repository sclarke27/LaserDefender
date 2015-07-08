using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{

    public static MusicPlayer musicPlayer;
    public AudioSource bgMusic;
    public AudioSource menuBgMusic1;
    public AudioSource menuBgMusic2;
    public AudioSource menuBgMusic3;
    private bool isPlaying = false;
    private GameData gameData;
    private bool isInMenus;


    // Use this for initialization
    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>(); //used to get music volume
        isInMenus = true;
        if (musicPlayer == null)
        {
            DontDestroyOnLoad(gameObject);
            musicPlayer = this;
        }
        else if (this != musicPlayer)
        {
            Destroy(gameObject);
        }
    }

    public void SetInMenu(bool inMenu)
    {
        if (isInMenus != inMenu)
        {
            isInMenus = inMenu;
            bgMusic.Stop();
            menuBgMusic1.Stop();
            menuBgMusic2.Stop();
            menuBgMusic3.Stop();
            isPlaying = false;
        }
    }

    void Start()
    {
        if (isInMenus)
        {
            if (!isPlaying)
            {
                menuBgMusic1.Play();
                menuBgMusic2.Play();
                menuBgMusic3.Play();
                isPlaying = true;
            }

        }
        else
        {
            if (!isPlaying)
            {
                bgMusic.Play();
                isPlaying = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float volume = gameData.GetMusicVolume();
        if (isInMenus)
        {
            menuBgMusic1.volume = volume;
            menuBgMusic2.volume = volume;
            menuBgMusic3.volume = volume;
            if (!isPlaying)
            {
                menuBgMusic1.Play();
                menuBgMusic2.Play();
                menuBgMusic3.Play();
                isPlaying = true;
            }

        }
        else
        {
            bgMusic.volume = volume;
            if (bgMusic != null && !isPlaying)
            {
                bgMusic.Play();
                isPlaying = true;
            }
        }
    }
}
