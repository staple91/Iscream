using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using No;

public class LeverPuzzle : Puzzle
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            parentSprite.sprite = sprite;
            Debug.Log("Å¬¸¯");
        }
    }

}
