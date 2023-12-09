using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialPuzzle : Puzzle
{
    public override void Interact()
    {
    }
    public void Update()
    {
        transform.Rotate(Vector3.forward);
    }

}
