using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using Photon.Pun;

public class MicComponent : MonoBehaviour, IPunObservable
{
    public static float sensitivity = 1000;
    public float loudness = 0;
    private AudioSource _audio;
    public Player player;
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        player = GetComponent<Player>();
    }

    bool isUseMic = false;
    void Start()
    {
        isUseMic = Microphone.devices.Length > 0 && player.controller.photonView.IsMine;
        if (!isUseMic)
            return;

        _audio.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        _audio.loop = true;
        _audio.mute = false;
        while (Microphone.GetPosition(Microphone.devices[0]) <= 0) { }
        _audio.Play();

    }

    private void Update()
    {
        //Debug.Log(PhotonNetwork.CountOfPlayers);
        if (PhotonNetwork.CountOfPlayers > 1)
            return;
        if (isUseMic)
            loudness = GetAveragedVolume() * sensitivity;

    }
    float GetAveragedVolume()
    {
        if (!isUseMic)
            return 0;
        float[] data = new float[256];
        float a = 0;
        _audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }
    public void SetListener()
    {
        if (!isUseMic)
            return;
        foreach (IListenable ear in ListenerManager.Instance.listeners)
        {
            float dist = Mathf.Max(1, Vector3.Distance(transform.position, ear.Pos));
            if (dist < loudness)
            {
                float curLoud = loudness / dist;
                if (ear.Loudness < curLoud || ear.LoudPlayer == null)
                {
                    ear.Loudness = loudness / dist;
                    ear.LoudPlayer = player;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, loudness);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            if (isUseMic)
                loudness = GetAveragedVolume() * sensitivity;
            stream.SendNext(loudness);
        }
        else
        {   
            loudness = (float)(stream.ReceiveNext());
        }
    }
}
