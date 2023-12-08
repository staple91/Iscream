using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTest : MonoBehaviour
{

    public CinemachineDollyCart c;

    public Camera enemyCamera;


    // Start is called before the first frame update
    void Start()
    {
        c = FindObjectOfType<CinemachineDollyCart>();
        c.enabled = false;
        enemyCamera = FindObjectOfType<Camera>();

        StartCoroutine(StartDeadScene());
        
    }


    IEnumerator StartDeadScene()
    {
        float curTime = 0f;
        float termTime = 5f;


        while(curTime < termTime)
        {
            curTime += Time.deltaTime;

            if(curTime > termTime)
            {
                c.enabled = true;
                yield return null;
            }
            yield return null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        FoundTargetCameraAction();
    }


    void FoundTargetCameraAction()
    {

        

        //RaycastHit ray;

        //Debug.DrawLine(enemyCamera.transform.position, transform.forward * 10f);

        //if(Physics.Raycast(enemyCamera.transform.position, transform.forward * 10f, out ray))
        //{
        //    Debug.Log("플레이어 레이어 쏨 ");
        //}
    }
}
