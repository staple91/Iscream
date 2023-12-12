using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRota : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        Vector3 vector = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(vector).normalized;
    }
}
