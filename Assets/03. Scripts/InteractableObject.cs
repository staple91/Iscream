using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{
    Light,
    Door,

}
public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField]
    InteractType type;
    Player owner;

    InteractStratagy stratagy;

    private void Awake()
    {
        switch(type)
        {
            case InteractType.Light:
                stratagy = new LightStratagy(this);
                break;
            case InteractType.Door:
                stratagy = new DoorStratagy(this);
                break;
        }
    }

    public Player Owner 
    {
        get => owner;
        set => owner = value; 
    }

    public void Interact()
    {
        stratagy.Act();
    }
}

public abstract class InteractStratagy
{
    protected InteractableObject target;
    public InteractStratagy(InteractableObject target)
    {
        this.target = target;
    }
    public abstract void Act();
}

public class DoorStratagy : InteractStratagy
{
    public DoorStratagy(InteractableObject target) : base(target)
    {
        this.target = target;
    }
    public override void Act()
    {
    }
}
public class LightStratagy : InteractStratagy
{
    public LightStratagy(InteractableObject target) : base(target)
    {
        this.target = target;
    }
    public override void Act()
    {
    }
}
