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

            Debug.DrawRay(playerCam.transform.position, transform.forward * 10f, Color.red);


            if (Physics.Raycast(playerCam.transform.position, transform.forward * 10f, out hit, 10))
            {
                

                interactable = hit.transform.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    interactable.Owner = this;
                    interactable.Interact();

                }
                    
               
                Debug.Log(hit.transform.GetComponent<Transform>() + "정보 ");
                
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

            if (Input.GetKey(KeyCode.JoystickButton0))
            {

                Debug.Log("a버튼 ");
                Click();

            }

        }
    }
}
