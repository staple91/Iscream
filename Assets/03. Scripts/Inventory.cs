using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using YoungJaeKim;
namespace No
{
    public class Inventory : MonoBehaviourPun,IPunObservable
    {
        [SerializeField]
        int invenSize;

        ItemObject[] itemArr;

        [SerializeField]
        GameObject[] tpsItemModelArr;

        public Dictionary<ITEM_TYPE, GameObject> tpsItemDic;


        ItemObject curItem;


        int curItemIndex = -1;
        public int CurItemIndex
        {
            get => curItemIndex;
            set
            {
                if (curItem != null)
                {
                    curItem.UnEquip();
                }
                curItemIndex = value;
                curItem = GetItem(curItemIndex);
                if (curItem != null)
                {
                    curItem.Equip();
                }
            }
        }

        private void OnEnable()
        {
            itemArr = new ItemObject[invenSize];
            Debug.Log(gameObject.GetComponent<PhotonView>().ViewID);
            tpsItemDic = new Dictionary<ITEM_TYPE, GameObject>
            {
                { ITEM_TYPE.FLASHLIGHT, tpsItemModelArr[0] }, // 손전등. 인스펙터에서 아이템 모델 넣기.
                { ITEM_TYPE.LANTERN, tpsItemModelArr[1] }, // 손전등. 인스펙터에서 아이템 모델 넣기.
                {ITEM_TYPE.RADIODETECTOR, tpsItemModelArr[2] }
            };
            Debug.Log(tpsItemDic[ITEM_TYPE.FLASHLIGHT]);
        }

        public ItemObject GetItem(int index)
        {
            if (index < invenSize)
                return itemArr[index];
            else
                return null;
        }

        public void AddItem(ItemObject obj)
        {
            for (int i = 0; i < itemArr.Length; i++)
            {
                if(itemArr[i] == null)
                {
                    itemArr[i] = obj;
                    CurItemIndex = i;
                    return;
                }
            }
            Debug.LogWarning("inven full");
        }

        public ItemObject FindItem(ITEM_TYPE type)
        {
            foreach(ItemObject i in itemArr)
            {
                if(i != null && i.itemType == type)
                {
                    return i;
                }
            }
            return null;
        }

        private void Update()
        {
            if (photonView.IsMine)
                ChangeItem();
        }

        void ChangeItem()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CurItemIndex = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CurItemIndex = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CurItemIndex = 2;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                photonView.RPC("DiscardItem", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        void DiscardItem()
        {
            if (curItem != null)
            {
                curItem.Discard();
                curItem = null;
                itemArr[curItemIndex] = null;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(curItemIndex);
            }
            else
            {
                curItemIndex = (int)stream.ReceiveNext();
            }
        }
    }

}