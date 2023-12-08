using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeeJungChul;
public class LightMirrorItem : Item
{
    public override void Interact()
    {
        //Owner.GetItem()
    }
    public override void Use()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, 1 << LayerMask.NameToLayer("Floor")))
        {
            Debug.Log("a");
            this.gameObject.transform.position = hit.point;
            StartCoroutine(RotateMirrorCo());
        }

    }

    IEnumerator RotateMirrorCo()
    {
        StarterAssetsInputs input = Owner.GetComponent<StarterAssetsInputs>();
        FirstPersonController controller = Owner.GetComponent<FirstPersonController>();
        float threshold = 0.01f;
        while (!Input.GetKey(KeyCode.E))
        {
            if (input.look.sqrMagnitude >= threshold)
            {
                float deltaTimeMultiplier = controller.IsCurrentDevice ? 1.0f : Time.deltaTime;


                transform.Rotate(Vector3.up, input.look.x * controller.RotationSpeed * deltaTimeMultiplier);
                yield return new WaitForEndOfFrame();
            }
            else
                yield return new WaitForEndOfFrame();

        }
    }
}
