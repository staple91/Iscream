using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookClose : MonoBehaviour
{
    public Animator gifAnimator;

    private void Start()
    {
        gifAnimator = GetComponent<Animator>();
    }
}
