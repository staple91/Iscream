using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using No;

public class LeverPuzzleClear : Puzzle
{
    public Sprite sprite;
    public SpriteRenderer parentSprite;

    private void Start()
    {
        parentSprite = GetComponent<SpriteRenderer>();       
    }

    private void Update()
    {
        Interact();
    }

    public override void Interact()
    {      
        Debug.Log("Å¬¸®¾î");
    }

}
