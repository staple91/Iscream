using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KimKyeongHun
{
    public class Player : MonoBehaviour
    {


        public GameObject playerEye; // 레이캐스트 플레이어 눈
        public Transform doorGet;


        public InteractStratagy stratagy;
        public InteractableObject target;

        IInteractable interactable;

        MicComponent mic;
        public Camera playerCam;






        // Start is called before the first frame update
        void Start()
        {
            playerCam = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.JoystickButton0))
            {
                Click();

            }
            target = new InteractableObject();
            mic = GetComponent<MicComponent>();
        }
        public void Click()
        {
            //interactable.Interact();

            Interact();

        }

        public void Interact()
        {

            RaycastHit hit;

            Debug.DrawRay(playerCam.transform.position, transform.forward * 10f, Color.red);

            if (Physics.Raycast(playerCam.transform.position, transform.forward * 10f, out hit, 10))
            {
                interactable = hit.transform.GetComponent<IInteractable>();
                interactable.Owner = this;
                interactable.Interact();

                Debug.Log(hit.transform.GetComponent<Transform>() + " ");

            }
        }
    }
}
