using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YoungJaeKim
{
    public abstract class Item: IInteractable
    {
        ItemManager IM;
        Player owner;
        public Player Owner
        {
            get => owner;
            set => owner = value;
        }

        public abstract void Interact();

        
        

        
    }
    public class RecoveryItem : Item
    {
        public override void Interact()
        {
            
        }
    }
    public class Food : RecoveryItem
    {
        public Food(ItemManager IM) { }
        
        public override void Interact() { Debug.Log("????????"); }
    }
    public class Medicine : RecoveryItem
    {
        public Medicine(ItemManager IM) { }
        public override void Interact() { Debug.Log("??????"); }
    }
    public class Beverage : RecoveryItem
    {
        public Beverage(ItemManager IM) { }
        public override void Interact() { Debug.Log("????????"); }
    }
    public class EquipmentItem : Item
    {
        public override void Interact()
        {

        }
    }
    public class CameraEquip : EquipmentItem
    {
        public CameraEquip(ItemManager IM) { }
        public override void Interact() { Debug.Log("????????"); }
    }
    public class Lantern : EquipmentItem
    {
        public Lantern(ItemManager IM) { }
        public override void Interact() { Debug.Log("????????"); }
    }
    public class Key : EquipmentItem
    {
        public Key(ItemManager IM) { }
        public override void Interact() { Debug.Log("??????"); }
    }
    public class ConsumableItem : Item
    {
        public override void Interact()
        {

        }
    }
    public class Battery : ConsumableItem
    {
        public Battery(ItemManager IM) { }
        public override void Interact() { Debug.Log("????????"); }
    }

    public enum ITEM_TYPE
    {
        FOOD=1<<0,
        MEDICINE=1<<1,
        BEVERAGE=1<<2,
        CAMERA=1<<3,
        LANTERN=1<<4,
        KEY=1<<5,
        BATTERY=1<<6,
    }

    public class ItemManager : MonoBehaviour
    {
        public Item item;
        public ITEM_TYPE itemType;
        // Start is called before the first frame update
        void Start()
        {
            if(itemType == ITEM_TYPE.FOOD) { item=new Food(this); }            
            if (itemType == ITEM_TYPE.MEDICINE) {  item=new Medicine(this); }
            if(itemType == ITEM_TYPE.BEVERAGE) { item= new Battery(this); }
            if (itemType == ITEM_TYPE.CAMERA) { item= new Medicine(this);}
            if (itemType == ITEM_TYPE.BATTERY) { item= new Battery(this);}
            if(itemType == ITEM_TYPE.LANTERN) { item= new Lantern(this);}
            if (itemType == ITEM_TYPE.KEY) { item= new Key(this);}            
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
