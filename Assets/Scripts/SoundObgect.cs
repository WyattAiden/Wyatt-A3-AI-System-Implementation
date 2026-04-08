using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundObgect : MonoBehaviour
{
    public UnityEvent<SoundObgect> onSoundTriggered = new UnityEvent<SoundObgect>();

    AudioSource AudioSource;

    public virtual void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Charicter>()  != null)
        {
            AudioSource.Play();
            onSoundTriggered.Invoke(this);
        }
    }
}
