using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YoungJaeKim;
namespace No
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        int invenSize;


        ItemObject[] itemArr;
        private void Awake()
        {
            itemArr = new ItemObject[invenSize];
        }

        public void AddItem(ItemObject obj)
        {
            for (int i = 0; i < itemArr.Length; i++)
            {
                if(itemArr[i] == null)
                {
                    itemArr[i] = obj;
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
    }

}