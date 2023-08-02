using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource au;
    public AudioClip clip;
    void Start()
    {
        au = GetComponent<AudioSource>();
        au.PlayOneShot(clip);
    }
}
