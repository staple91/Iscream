using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzleClear : MonoBehaviour
{
    public Animator gifAnimator;

    private void Start()
    {
        gifAnimator = GetComponent<Animator>();
    }
}
