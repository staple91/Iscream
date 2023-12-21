using PangGom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundObject : MonoBehaviour
{
    AudioSource source;
    float time;
    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
        time = 0;
    }
    private void Update()
    {
        if (source.clip != null)
        {
            time += Time.deltaTime;

            if (time > source.clip.length)
                SoundManager.Instance.ReturnObj(this.gameObject);
        }
    }

    public void EffectSound(float volume)
    {
        source.volume = volume;
    }
}
