using System.Collections;
using System.Collections.Generic;
using UnityEngine;



<<<<<<< Updated upstream
namespace KimKyeongHun
{
=======

public class Player : MonoBehaviour
{


    public GameObject playerEye; // ·¹ÀÌÄ³½ºÆ® ÇÃ·¹ÀÌ¾î ´«
    public Transform doorGet;


    public InteractStratagy stratagy;
    public InteractableObject target;

    IInteractable interactable;

    MicComponent mic;


    public void Interact()
    {
        //¹®¿­¸², ºÒ ÄÑ±â µîµî
        RaycastHit hit;


        if (Physics.Raycast(playerEye.transform.position, transform.forward * 10f, out hit, 10))
        {
            interactable = hit.transform.GetComponent<IInteractable>();
            interactable.Owner = this;
            interactable.Interact();
        }


    }

    public void Click()
    {
        //interactable.Interact();

        Interact();

    }
>>>>>>> Stashed changes

    public class Player : MonoBehaviour
    {
<<<<<<< Updated upstream
        public Camera playerCam;

        IInteractable interactable;
        public void Interact()
        {
            //ë¬¸ì—´ë¦¼, ë¶ˆ ì¼œê¸° ë“±ë“±

            RaycastHit hit;

            Debug.DrawRay(playerCam.transform.position, transform.forward * 10f, Color.red);

            if (Physics.Raycast(playerCam.transform.position, transform.forward * 10f, out hit, 10))
            {
                interactable = hit.transform.GetComponent<IInteractable>();
                interactable.Owner = this;
                interactable.Interact();

                Debug.Log(hit.transform.GetComponent<Transform>() + "ì •ë³´ ");
                
            }


        }

        public void Click()
        {
            Interact();
        }



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

                Debug.Log("aë²„íŠ¼ ");
                Click();

            }

            //if (Input.GetKey(KeyCode.Space))
            //{

            //    Debug.Log("aë²„íŠ¼ ");
            //    Click();

            //}



      
=======
        target = new InteractableObject();
        mic = GetComponent<MicComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {

            Debug.Log("a¹öÆ° ");
            Click();
            //Click();
>>>>>>> Stashed changes
        }
        
    }

    
}
