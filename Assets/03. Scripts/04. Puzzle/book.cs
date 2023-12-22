using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using No;
using PangGom;

public class book : Puzzle
{
    public GameObject bookEvent;
    public Book bookpage;
    public BookClose bookClose;

    private void Update()
    {
        ExitBook();
    }

    public override void Interact()
    {
        if (Owner.controller.photonView.IsMine)
        {
            bookEvent.SetActive(true);
            Cursor.visible = true;
            SoundManager.Instance.PlayAudio(SoundManager.Instance.bookOpen, false);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ExitBook()
    {
        if (Owner.controller.photonView.IsMine && bookpage.currentPage == 6)
        {
            bookEvent.SetActive(false);
            Cursor.visible = false;
            bookpage.currentPage = 0;
            bookClose.gifAnimator.SetTrigger("BookClose");
            SoundManager.Instance.PlayAudio(SoundManager.Instance.girlLaughSound, false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
