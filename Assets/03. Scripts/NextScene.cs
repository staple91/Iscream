using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NextScene : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        SceneNext();
    }
    public void SceneNext()
    {
        PhotonNetwork.LoadLevel("Iscream");
    }
}
