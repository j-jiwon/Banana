using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;
    [Header("# SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // init bgm player
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // init sfx player
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for (int i = 0; i < sfxPlayers.Length; ++i)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; ++i)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;
            
            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            // if multiple sounds are available
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }
            
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();            
            break;
        }
    }
}
