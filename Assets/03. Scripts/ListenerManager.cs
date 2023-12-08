using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerManager : Singleton<ListenerManager>
{
    
    [SerializeField]
    public IListenable[] listeners;
}
