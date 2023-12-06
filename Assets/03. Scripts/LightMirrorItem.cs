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
        Physics.Raycast(Owner.transform.position, transform.forward, out RaycastHit hit, 1 >> LayerMask.NameToLayer("Floor"));
        Instantiate(this.gameObject, hit.point, Quaternion.identity);
        StartCoroutine(RotateMirrorCo());

    }

    IEnumerator RotateMirrorCo()
    {
        StarterAssetsInputs input = Owner.GetComponent<StarterAssetsInputs>();
        FirstPersonController controller = Owner.GetComponent<FirstPersonController>();
        float threshold = 0.01f;
        while (Input.GetKey(KeyCode.E))
        {

            if (input.look.sqrMagnitude >= threshold)
            {
                float deltaTimeMultiplier = controller.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                controller.rotationVelocity = input.look.x * controller.RotationSpeed * deltaTimeMultiplier;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
