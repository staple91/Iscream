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
        Init();
        RaycastHit hit;
        Vector3 tempDir = dir;


        bool isHit = false;
        vectors.Add(transform.position);

        if (Physics.Raycast(transform.position, tempDir, out hit, 99f))
        {
            ShotRay(hit, out isHit, ref tempDir);
        }


        while (isHit)
        {
            if (Physics.Raycast(hit.point, tempDir, out hit, 99f))
            {
                ShotRay(hit, out isHit, ref tempDir);
            }
            else
                break;
        }
        lineRenderer.positionCount = vectors.Count;
        lineRenderer.SetPositions(vectors.ToArray());
    }
    void Init()
    {
        vectors.Clear();
        mirrors.Clear();
    }
    void ShotRay(RaycastHit hit, out bool isHit, ref Vector3 dir)
    {

        vectors.Add(hit.point);
        if (hit.transform.TryGetComponent<LightMirrorItem>(out LightMirrorItem mirror) && !mirrors.Contains(mirror))
        {
            mirrors.Add(mirror);
            isHit = true;

            Debug.Log(hit.normal);
            dir = Vector3.Reflect(dir, hit.normal).normalized;
            Debug.Log(dir);
        }
        else
            isHit = false;
    }

}
