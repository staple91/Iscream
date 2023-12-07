using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMouthObj : MonoBehaviour
{
    [SerializeField]
    MicComponent mic;

    [SerializeField]
    Vector3 originMouthPos1;
    [SerializeField]
    Vector3 originMouthPos2;

    [SerializeField]
    Transform MouthTr1;
    [SerializeField]
    Transform MouthTr2;

    [SerializeField]
    Vector3 targetMouthPos1;
    [SerializeField]
    Vector3 targetMouthPos2;

    float maxLoudness = 100f;

    private void Update()
    {
        MouthTr1.localPosition = Vector3.Lerp(originMouthPos1, targetMouthPos1, mic.loudness / maxLoudness);
        MouthTr2.localPosition = Vector3.Lerp(originMouthPos2, targetMouthPos2, mic.loudness / maxLoudness);
    }
}
