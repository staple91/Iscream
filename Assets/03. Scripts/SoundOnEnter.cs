using KimKyeongHun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnEnter : MonoBehaviour
{
    AudioSource audio;
    float length;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            if(player.controller.photonView.IsMine)
            {
                audio.Play();
                Destroy(this);
            }
        }
    }
}
