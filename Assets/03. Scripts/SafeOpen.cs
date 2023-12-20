using NavKeypad;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.WSA;


namespace YoungJaeKim
{
    public class SafeOpen : MonoBehaviour
    {
        private Quaternion Open = Quaternion.identity;
        [SerializeField]
        private float doorAngle = -110f;
        [SerializeField]
        private float doorOpenSpeed = 1f;
        public bool isOpened;
        private Keypad keypad;
        private void Start()
        {
            isOpened = true;
            Open.eulerAngles = new Vector3(0, doorAngle, 0);

        }
        private void Update()
        {
            if(isOpened)
            {

            transform.rotation = Quaternion.Slerp(transform.rotation, Open, Time.deltaTime * doorOpenSpeed);
            }



        }

        public void OpenTheDoor()
        {
            isOpened = true;
        }
    }
}