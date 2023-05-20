using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgOST, sfx;

    public AudioClip hitClip;
    public AudioClip fireClip;
    public AudioClip connectClip;
    public AudioClip loseClip;

    public void StopBackgroundOST() => bgOST.Pause();
    public void PlayBackgroundOST() => bgOST.Play();

    public void PlaySFX(AudioClip clip)
    {
        sfx.clip= clip;
        sfx.PlayOneShot(clip);
    }
}
