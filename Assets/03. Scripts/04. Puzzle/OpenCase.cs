using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCase : MonoBehaviour
{
    [SerializeField]
    Vector3 targetDir;

    public void Open()
    {
        StartCoroutine(OpenCo());
    }

    public IEnumerator OpenCo()
    {
        while(Vector3.Distance(transform.localPosition, targetDir) > 0.01f)
        {
            Debug.Log("asd");
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetDir, Time.deltaTime);
            yield return null;
        }
    }
}
