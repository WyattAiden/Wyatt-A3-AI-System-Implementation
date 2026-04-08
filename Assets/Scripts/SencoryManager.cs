using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SencoryManager : MonoBehaviour
{
    public ZombieAI Zombie;
    public GameObject GlassObgs;
    public GameObject Zoms;

    private void Awake()
    {
        foreach (Transform child in GlassObgs.transform)
        {
            SoundObgect soundObgect = child.GetComponent<SoundObgect>();
            if (soundObgect != null)
            {
                foreach (Transform child2 in Zoms.transform)
                {
                    ZombieAI zom2 = child2.GetComponent<ZombieAI>();
                    soundObgect.onSoundTriggered.AddListener(zom2.SoundTriggered);
                }
                    
            }
        }

    }
}
