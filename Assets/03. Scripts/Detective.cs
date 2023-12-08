using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YoungJaeKim
{
    public class Detective : MonoBehaviour
    {
        public float radious = 3;

        public float maxDistance = 3;
        public Collider[] cols;
    public LayerMask targetLayerMask;

        public LayerMask RayableLayerMask;
        public Collider col;


        public bool isRangeDetection;

        public bool isRayDetection;

        public Vector3 LastDetectivePos//최종적인 위치 정보

        {
            get;
            private set;
        }

        public bool IsDection
        {
            get
            {
                return isRangeDetection && isRayDetection;
            }
        }

        bool CheckInLayerMask(int layerIndex)

        {

            return (targetLayerMask & (1 << layerIndex)) != 0;

        }

    // Start is called before the first frame update
    // Update is called once per frame

    void Update()

        {
            cols = Physics.OverlapSphere(transform.position, radious, targetLayerMask);

            Debug.DrawLine(transform.position, transform.position + transform.forward * maxDistance, Color.white);

            isRangeDetection = cols.Length > 0;
            if(cols.Length > 0 )
            {

            col = cols[0];
            }
            foreach(Collider item in cols)
            {
                if (Vector3.Distance(col.transform.position, transform.position) > Vector3.Distance(item.transform.position, transform.position))
                    col = item;
            }
            
            if (isRangeDetection)
            {
                RaycastHit hit;

                Vector3 direction = (col.transform.position - transform.position).normalized;//normalized - 방향만 추출

                Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.blue);

                if (Physics.Raycast(transform.position, direction, out hit, maxDistance))

                {

                    isRayDetection = CheckInLayerMask(hit.collider.gameObject.layer);

                    if (isRayDetection)

                    {

                        LastDetectivePos = hit.transform.position;



                        Debug.DrawLine(transform.position, transform.position + (direction * maxDistance), Color.black);

                    }

                }
        }

        }

        private void OnDrawGizmos()

        {

            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, radious);



        }
    }
}
