using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace KimKyeongHun
{

    public class Player : MonoBehaviour
    {
        public GameObject playerCam;
        MicComponent mic;

        IInteractable interactable;
        public void Interact()
        {
            //문열림, 불 켜기 등등

            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * 10f, out hit, 10))
            {
                

                interactable = hit.transform.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    interactable.Owner = this;
                    interactable.Interact();
                    Debug.Log("상호작용");
                }
                
            }
           


        }






        public void Click()
        {
            Interact();
        }



        // Start is called before the first frame update
        void Start()
        {
            //playerCam = FindObjectOfType<Camera>();
            playerCam = GameObject.FindGameObjectWithTag("MainCamera");
            

            mic = GetComponent<MicComponent>();
        }

        // Update is called once per frame
        void Update()
        {

            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 10f, Color.red);



            if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
            {

                Debug.Log("a버튼 ");
                Click();

            }

        }
    }
}
