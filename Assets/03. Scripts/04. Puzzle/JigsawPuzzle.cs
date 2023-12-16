using No;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace PangGom
{
    public class JigsawPuzzle : Puzzle, IPunObservable
    {
        PhotonView pv;
        float distance = 100f;
        [SerializeField]
        LayerMask layerMaskPZPiece;

        GameObject piece = null;
        //GameObject surchTr;//자식 위치 찾아줌
        //GameObject[] clearTr = new GameObject[9];

        Vector3 puzzlePoint;//퍼즐 피스 원래 위치

        float rangeValue = 0.05f;
        public bool puzzleSole = false;
        int solCount = 0;

        public int SolCount
        { get { return solCount; }
            set 
            { 
                solCount = value;
                if (solCount == 9)
                    puzzleSole = true;
            }
        }

        private void Start()
        {
            pv = GetComponent<PhotonView>();
        }
        void Update()
        {
            Debug.Log(Owner);
            if (Owner != null && Owner.controller.photonView.IsMine)
            {
                if (Input.GetMouseButtonDown(0))
                    PuzzlePoint();//퍼즐 초기 위치 저장
                else if (Input.GetMouseButton(0))
                    PuzzlePosition();//퍼즐 움직임
                else if (Input.GetMouseButtonUp(0))
                    PuzzleMatch();
                if (puzzleSole)
                    Destroy(gameObject);
            }
            else
                return;
        }
        void PuzzlePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layerMaskPZPiece))
            {
                puzzlePoint = hit.transform.position;//원래 위치 저장
                piece = hit.transform.gameObject;//레이 맞은 오브젝트 정보
            }
        }
        void PuzzlePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layerMaskPZPiece))
                piece.transform.position = new Vector3(hit.point.x, hit.point.y, puzzlePoint.z);
        }
        void PuzzleMatch()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layerMaskPZPiece))
            {
                if (Vector3.Distance(piece.transform.position, piece.transform.parent.position) < rangeValue)
                {
                    piece.transform.position = piece.transform.parent.position;
                    piece.layer = 0;
                    SolCount++;
                }
                else
                    piece.transform.position = puzzlePoint; //퍼즐 못맞추면 위치 리셋

            }
        }

        public override void Interact()
        {
            pv.TransferOwnership(Owner.controller.photonView.Owner);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(SolCount);
            }
            else
            {
                SolCount = (int)stream.ReceiveNext();
            }
        }
    }
}
