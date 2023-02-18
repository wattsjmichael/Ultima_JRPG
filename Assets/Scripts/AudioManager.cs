using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] sfx;
    public AudioSource[] bgm;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() { }

    public void PlaySFX(int sfxClip)
    {
        if (sfxClip < sfx.Length)
        {
            sfx[sfxClip].Play();
        }
    }

    public void PlayBGM(int bgmClip)
    {
        if (!bgm[bgmClip].isPlaying)
        {
            StopBGM();

            if (bgmClip < bgm.Length)
            {
                bgm[bgmClip].Play();
            }
        }
    }

    public void StopBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
