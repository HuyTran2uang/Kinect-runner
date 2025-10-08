using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public enum SoundType
{
    KIRBY,
    LOSER,
    BGM,
    RICK_ROLL,
    INNOVAR,
    VICTOCUM
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static AudioManager instance;
    private AudioSource audioSource;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType soundType, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)soundType], volume);
    }

    public static void PlayBGM(SoundType soundType, float volume = 1f)
    {
        instance.audioSource.clip = instance.soundList[(int)soundType];
        instance.audioSource.volume = volume;
        instance.audioSource.loop = true;
        instance.audioSource.Play();
    }

    public static void StopBGM()
    {
        instance.audioSource.Stop();
        instance.audioSource.clip = null;
    }
}