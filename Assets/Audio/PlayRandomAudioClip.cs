using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource;

    private void Awake()
    {
        // Automatically cache the attached AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Selects a random audio clip from the list and plays it.
    /// Call this function from your game logic, buttons, or triggers.
    /// </summary>
    public void PlayRandomClip()
    {
        // Verify the list has available clips to avoid errors
        if (audioClips == null || audioClips.Count == 0)
        {
            Debug.LogWarning("Cannot play sound: The Audio Clips list is empty!", this);
            return;
        }

        // Pick a random index between 0 (inclusive) and the size of the list (exclusive)
        int randomIndex = Random.Range(0, audioClips.Count);
        AudioClip selectedClip = audioClips[randomIndex];

        // PlayOneShot prevents the audio from being cut off if triggered multiple times rapidly
        audioSource.PlayOneShot(selectedClip);
    }
}