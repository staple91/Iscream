using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Test : MonoBehaviour
{
    public CinemachineSmoothPath path;
    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<CinemachineSmoothPath>();
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(path.isActiveAndEnabled);
    }
}
