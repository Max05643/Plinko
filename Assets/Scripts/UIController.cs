using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class UIController : MonoBehaviour, IInitializable
{
    [Inject]
    SoundController soundController;

    [SerializeField]
    GameObject muteButton, unMuteButton;

    [SerializeField]
    TextMeshProUGUI scoreText;

    int score = 0;

    /// <summary>
    /// Adds one score and display it on UI
    /// </summary>
    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
        scoreText.DOKill();
        scoreText.DOColor(Color.green, 0.2f).SetLoops(2, LoopType.Yoyo);
        scoreText.transform.DOKill();
        scoreText.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    /// <summary>
    /// Resets the score to 0
    /// </summary>
    public void ResetScoreButtonPressed()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// Mutes the sound
    /// </summary>
    public void MuteButtonPressed()
    {
        soundController.MuteSound();
        RepaintSoundButton();
    }

    /// <summary>
    /// Unmutes the sound
    /// </summary>
    public void UnMuteButtonPressed()
    {
        soundController.UnMuteSound();
        RepaintSoundButton();
    }

    void RepaintSoundButton()
    {
        muteButton.SetActive(!soundController.IsMuted);
        unMuteButton.SetActive(soundController.IsMuted);
    }

    void IInitializable.Initialize()
    {
        RepaintSoundButton();
    }
}
