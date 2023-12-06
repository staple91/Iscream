using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Player : MonoBehaviour
{

  
    public Camera playerCam;
    
    

    IInteractable interactable;


    public void Interact()
    {
        //문열림, 불 켜기 등등
        
        RaycastHit hit;

        Debug.DrawRay(playerCam.transform.position, transform.forward * 10f, Color.red);

        if (Physics.Raycast(playerCam.transform.position, transform.forward * 10f, out hit, 10))
        {
            interactable = hit.transform.GetComponent<IInteractable>();
            interactable.Owner = this;
            interactable.Interact();
           
            Debug.Log(hit.transform.GetComponent<Transform>() + "정보 ");
            //Debug.Log(layerMask + "정보 ");
            //Debug.Log(hit.collider.name + "레이캐스트 정보 ");
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


        if(Input.GetKey(KeyCode.JoystickButton0))
        {
            
            Debug.Log("a버튼 ");
            Click();
            
        }

        if (Input.GetKey(KeyCode.Space))
        {

            Debug.Log("a버튼 ");
            Click();

        }


    }
}
