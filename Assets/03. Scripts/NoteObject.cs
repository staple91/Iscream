using KimKyeongHun;
using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace No
{
    public class NoteObject : MonoBehaviour, IInteractable
    {
        Player owner;
        public Player Owner { set => owner = value; }

        [SerializeField, TextArea]
        string content;
        public void Interact()
        {
            Debug.Log(content);
            owner.IsMoveable = false;
            CanvasManager.Instance.noteText.text = content;
            CanvasManager.Instance.noteUI.SetActive(true);
            StartCoroutine(PopUpCo());
        }

        IEnumerator PopUpCo()
        {
            while (!Input.GetKey(KeyCode.E))
            {
                yield return new WaitForEndOfFrame();
            }
            owner.IsMoveable = true;
            CanvasManager.Instance.noteUI.SetActive(false);
        }
    }

}