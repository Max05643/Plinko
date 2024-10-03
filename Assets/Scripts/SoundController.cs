using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls sound effects
/// </summary>
public class SoundController : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool IsMuted => audioSource.mute;

    /// <summary>
    /// Plays the clip with the given index
    /// </summary>
    public void PlayClip(int index)
    {
        audioSource.PlayOneShot(clips[index]);
    }

    public void MuteSound()
    {
        audioSource.mute = true;
    }

    public void UnMuteSound()
    {
        audioSource.mute = false;
    }

}
