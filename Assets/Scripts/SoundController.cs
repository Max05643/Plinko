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

    /// <summary>
    /// Mutes the sound
    /// </summary>
    public void MuteSound()
    {
        audioSource.mute = true;
    }

    /// <summary>
    /// Unmutes the sound
    /// </summary>
    public void UnMuteSound()
    {
        audioSource.mute = false;
    }

}
