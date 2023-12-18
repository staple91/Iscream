using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerManager : Singleton<ListenerManager>
{
    
    [SerializeField]
    public List<IListenable> listeners = new List<IListenable>();
}
