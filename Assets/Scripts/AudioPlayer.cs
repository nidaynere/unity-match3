using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;
#pragma warning restore CS0649

    private void OnValidate()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }
}