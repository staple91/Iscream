using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using LeeJungChul;

public class NextScene : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        SceneNext();
    }
    public void SceneNext()
    {
        PhotonNetwork.LoadLevel("Iscream");
        Debug.Log(PhotonManager.nick);
    }
}
