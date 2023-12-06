using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectRayLight : MonoBehaviour
{
    [SerializeField]
    Vector3 dir;

    LineRenderer lineRenderer;

    List<Vector3> vectors = new List<Vector3>();
    List<LightMirrorItem> mirrors = new List<LightMirrorItem>();
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }
    private void Update()
    {
        vectors.Clear();
        mirrors.Clear();
        RaycastHit hit;
        Vector3 tempDir = dir;


        bool isHit = false;
        vectors.Add(transform.position);
        if (Physics.Raycast(transform.position, tempDir, out hit, 99f))
        {
            if (hit.transform.TryGetComponent<LightMirrorItem>(out LightMirrorItem mirror) && !mirrors.Contains(mirror))
            {
                tempDir = hit.transform.forward;
                mirrors.Add(mirror);
                isHit = true;
                Debug.Log(tempDir);
            }
            vectors.Add(hit.point);
        }


        while (isHit)
        {
            ShotRay(hit, out isHit, ref tempDir);
        }
        lineRenderer.SetPositions(vectors.ToArray());
    }

    void ShotRay(RaycastHit hit, out bool isHit, ref Vector3 dir)
    {

        if (Physics.Raycast(transform.position, dir, out hit, 99f))
        {
            if (hit.transform.TryGetComponent<LightMirrorItem>(out LightMirrorItem mirror) && !mirrors.Contains(mirror))
            {
                dir = hit.transform.forward;
                mirrors.Add(mirror);
                isHit = true;
                Debug.Log(dir);
            }
            else
                isHit = false;
            vectors.Add(hit.point);
        }
        isHit = false;
    }
}
