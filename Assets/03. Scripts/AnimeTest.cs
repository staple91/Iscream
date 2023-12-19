using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeTest : MonoBehaviour
{
    public Animator ghostAnime;

    private void Start()
    {
        ghostAnime=GetComponent<Animator>();
    }

    void Anime()
    {
        
        ghostAnime.SetBool("Run", true);
    }
}
