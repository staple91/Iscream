using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

namespace YoungJaeKim
{
    public class Ghost : MonoBehaviour, IListenable
    {
        public Detective detective;
        public Transform targetPlayer;
        Vector3 targetPosition;
        RaycastHit hit;
        NavMeshAgent ghostAgent;
        public bool isRandgeDetection;
        Rigidbody rb;
        
        [SerializeField]       
        float cubeVolume = 10;
        float checkTime = 1f;
        public float Loudness { set => throw new System.NotImplementedException(); }

        public Vector3 Pos => throw new System.NotImplementedException();
        Vector3 patrolableSpace ;
        public float partrolable = 3f;
        // Start is called before the first frame update
        void Start()
        {           
            rb = GetComponent<Rigidbody>();
            ghostAgent = GetComponent<NavMeshAgent>();   
            
        }
        
        // Update is called once per frame
        void Update()
        {
            Patrol();
            if (detective.IsDection)
            {

                //Vector3 targetVec = (detective.LastDetectivePos - transform.position).normalized;// 방향추출

                //transform.forward = targetVec;//타겟방향으로 전진하도록 방향수정

                //rb.velocity = targetVec * enemySpeed;
                ghostAgent.SetDestination(detective.col.transform.position);
                Debug.Log("몬스터 움직임");

            }

            // Collider[] cols1 = Physics.OverlapSphere(transform.position, cubeVolume, 1 << 7);

            
        }
        
        void Patrol()
        {
            
            Vector3 dir = (transform.forward*partrolable - transform.position).normalized;
            dir.y = 0;
            float distance = dir.magnitude;
            Debug.DrawLine(transform.position, transform.forward, Color.green);
            Debug.DrawLine(transform.position, transform.position + dir * partrolable, Color.magenta);
            float dot = Vector3.Dot(transform.forward, dir);
            Vector3 randPos;
            bool canMove = PatrolableRange(transform.position, out randPos);
            //bool canMove = PatrolableRange(transform.position, out Vector3 randPos);
            if (distance < partrolable && dot > 0)//
            {
                Debug.Log("if에 들어감");

                if(detective.col ==null)
                {
                    ghostAgent.destination = randPos;
                }

                StartCoroutine(PlayerCheck());
                //ghostAgent.destination = detective.col.transform.position;
            }


            else
            {
                Debug.Log("돌아가냐");
                if (canMove == false)
                {
                    canMove = PatrolableRange(transform.position, out randPos, 10);
                }
                else
                {
                    ghostAgent.destination = randPos;
                    if (Vector3.Distance(transform.position, randPos) < 0.1f)
                    {
                        canMove = false;
                    }
                }

            }
            
        }
        private bool PatrolableRange(Vector3 pos, out Vector3 randPos, float patrolableSpace = 10)
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
            Debug.Log("코루틴 돌아가냐");

            while(true)
            {
                if (detective.col == null)
                {
                    //yield break;

                    yield return new WaitForSeconds(checkTime);
                }
                ghostAgent.destination = detective.col.transform.position;

                

                if (detective.IsDection == false)//detective.isRangeDetection==false && 
                {
                    yield break;
                }

                yield return null;
            }

            

           
        }



    }
}

