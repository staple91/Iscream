using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

namespace YoungJaeKim
{
    public class Ghost : MonoBehaviour, IListenable
    {

        public Detective detective;
        Transform[] roamingPosition = new Transform[3];
        Vector3 targetPosition;
        RaycastHit hit;
        NavMeshAgent ghostAgent;
        public bool isRandgeDetection;
        Rigidbody rb;
        public Transform RoamingRocation1;
        public Transform RoamingRocation2;
        public Transform RoamingRocation3;
        //public Transform R4;
        //public Transform R5;
        


        [SerializeField]
        //float cubeVolume = 10;
        float checkTime = 1f;
        public float Loudness { set => throw new System.NotImplementedException(); }
        private bool is_SetPath = false;
        public Vector3 Pos => throw new System.NotImplementedException();
        Vector3 patrolableSpace;
        public float patrolable = 3f;
        public int currentpath = 0;
        public float roamingInterval = 10f;
        // Start is called before the first frame update
        void Start()
        {
            
            rb = GetComponent<Rigidbody>();
            ghostAgent = GetComponent<NavMeshAgent>();
            //StartCoroutine(PlayerCheck());
            roamingPosition[0] = RoamingRocation1;
            roamingPosition[1] = RoamingRocation2;
            roamingPosition[2] = RoamingRocation3;
            //roamingPosition[3]= R4;
            //roamingPosition[4] = R5;
            
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(currentpath);
            //ghostAgent.isStopped = false;
            Patrol();
            
        }

        void Patrol()
        {
            Debug.Log("패트롤되니?");
            Debug.DrawLine(transform.position, transform.forward, Color.green);          
            Vector3 randPos;
            bool canMove = PatrolableRange(transform.position, out randPos);
            if (detective.cols.Length == 0)//detective.col == null
            {
                StartCoroutine(Roaming());
                /*.destination = randPos;
                if (canMove == false)
                {
                    canMove = PatrolableRange(transform.position, out randPos);
                }
                else
                {
                    ghostAgent.destination = randPos;
                    if (Vector3.Distance(transform.position, randPos) < 0.1f)
                    {
                        canMove = false;
                    }
                }*/

            }
            else 
            {
                
                //ghostAgent.destination = detective.col.transform.position;
                
                ghostAgent.SetDestination(detective.col.transform.position);
                //StartCoroutine(PlayerCheck());
            }


        }
        private bool PatrolableRange(Vector3 pos, out Vector3 randPos, float patrolableSpace = 3)
        {
            Vector3 point = UnityEngine.Random.insideUnitSphere * patrolableSpace;
            point.y = 0;
            point += pos;
            NavMeshHit navMeshHit;
            bool moveable = NavMesh.SamplePosition(point, out navMeshHit, patrolableSpace, 1);
            randPos = navMeshHit.position;
            return moveable;

        }
        IEnumerator PlayerCheck()
        {
            

            while (true)
            {
                yield return new WaitForSeconds(3f);
                if (detective.cols.Length >0)
                {
                    ghostAgent.SetDestination(detective.col.transform.position);
                }
                
                if (detective.cols.Length == 0)
                {                    
                    ghostAgent.ResetPath();                    
                    Debug.Log("코루틴 아웃");                  
                }
                yield return null;
            }
            
            
        }

        IEnumerator Roaming()
        {
            Debug.Log("단계");
            if(!is_SetPath)
            {
                is_SetPath = true;
                ghostAgent.SetDestination(roamingPosition[currentpath].position);
                yield return new WaitForSeconds(roamingInterval);
                currentpath = currentpath+1;
                if (currentpath == roamingPosition.Length)
                    currentpath = 0;
                is_SetPath= false;
            }
           
            
            
        }
        public void Next(int n)
        {
            ghostAgent.destination = roamingPosition[n].position;
        }
        

    }
}

