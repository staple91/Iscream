using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicComponent : MonoBehaviour
{
    public float sensitivity = 100;
    public float loudness = 0;
    private AudioSource _audio;
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    void Start()
    {
        _audio.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        _audio.loop = true;
        _audio.mute = false;
        while (Microphone.GetPosition(Microphone.devices[0]) <= 0) { }
        _audio.Play();
    }
    void Update()
    {
        loudness = GetAveragedVolume() * sensitivity;
    }
    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        _audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }
    void SetListener()
    {
        foreach (IListenable ear in ListenerManager.Instance.listeners)
        {
            float dist = Mathf.Max(1, Vector3.Distance(transform.position, ear.Pos));
            if(dist < loudness)
                ear.Loudness = loudness / dist;
        }
    }
}
