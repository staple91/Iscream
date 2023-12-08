using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;

namespace YoungJaeKim
{
    public abstract class Item : IInteractable
    {
        public ItemManager im;
        Player owner;
        public Item(ItemManager im)
        {
            this.im = im;
        }
        public Player Owner
        {
            get => owner;
            set => owner = value;
        }

        public abstract void Interact();
        public virtual void Active()
        { }
    }
    public class RecoveryItem : Item
    {
        public RecoveryItem(ItemManager im) : base(im) { }
        public override void Interact()
        {

        }
    }
    public class Food : RecoveryItem
    {
        public Food(ItemManager im) : base(im) { }

        public override void Interact() { Debug.Log("¿ΩΩƒ¿Ã¥Â"); }

        public override void Active() 
        {
            //im.player.¡§Ω≈∑¬ Ω∫≈» += »∏∫π∑Æ
        }
    }
    public class Medicine : RecoveryItem
    {
        public Medicine(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("æ‡¿Ã¥Â"); }
        public override void Active()
        {
            //im.player.¡§Ω≈∑¬ Ω∫≈» += »∏∫π∑Æ
        }

    }
    public class Beverage : RecoveryItem
    {
        public Beverage(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("¿Ω∑·ºˆ¥Â"); }
        public override void Active()
        {
            //im.player.¡§Ω≈∑¬ Ω∫≈» += »∏∫π∑Æ
        }

    }
    public class EquipmentItem : Item
    {
        public EquipmentItem(ItemManager im) : base(im) { }
        public override void Interact()
        {

        }
    }
    public class CameraEquip : EquipmentItem
    {
        public CameraEquip(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("ƒ´∏ﬁ∂Û¥Â"); }
        public override void Active()
        {
            Debug.Log("¬Ô«˚¥Â");

            //ScreenCapture.CaptureScreenshot(Application.dataPath+"/05. Data/ScreenShot/" + fileName);
            //¿ßø°∞≈¥¬ µ«±‰«œ¥¬µ• ¿˙¿Â¿Ã ≥ π´ ø¿∑°∞…∏≤


        }
    }
    public class Lantern : EquipmentItem
    {
        public Lantern(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("∑£≈œ¿Ã¥Â"); }
    }
    public class FlashLight : EquipmentItem
    {

        public FlashLight(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("«√∑°Ω√∂Û¿Ã∆Æ!"); }
        public override void Active()
        {
            Debug.Log("µÈæÓø»?");
            im.BatteryTime -= Time.deltaTime;
            Debug.Log(im.BatteryTime);
            /*while (im.BatteryTime > 0)
            {
                
                im.flashLight.intensity -= 0.0001f;
                
                
                
            }*/

        }
    }
    public class Key : EquipmentItem
    {
        public Key(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("ø≠ºË¥Â"); }
    }
    public class ConsumableItem : Item
    {
        public ConsumableItem(ItemManager im) : base(im) { }
        public override void Interact()
        {

        }
    }
    public class Battery : ConsumableItem
    {
        public Battery(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("πË≈Õ∏Æ¥Â"); }
        public override void Active()
        {
            im.flashLight.intensity = 2.1f;
        }
    }

    public enum ITEM_TYPE
    {
        FOOD = 1 << 0,
        MEDICINE = 1 << 1,
        BEVERAGE = 1 << 2,
        CAMERA = 1 << 3,
        LANTERN = 1 << 4,
        KEY = 1 << 5,
        BATTERY = 1 << 6,
        FLASHLIGHT = 1 << 7,
    }

    public class ItemManager : MonoBehaviour
    {
        public Player player;
        public Item item;
        public ITEM_TYPE itemType;
        public Light flashLight;
        public float BatteryTime = 60f;
        
        // Start is called before the first frame update
        void Start()
        {
            //flashLight = GetComponent<Light>();
            if (itemType == ITEM_TYPE.FOOD) { item = new Food(this); }
            if (itemType == ITEM_TYPE.MEDICINE) { item = new Medicine(this); }
            if (itemType == ITEM_TYPE.BEVERAGE) { item = new Beverage(this); }
            if (itemType == ITEM_TYPE.CAMERA) { item = new CameraEquip(this); }
            if (itemType == ITEM_TYPE.BATTERY) { item = new Battery(this); }
            if (itemType == ITEM_TYPE.LANTERN) { item = new Lantern(this); }
            if (itemType == ITEM_TYPE.KEY) { item = new Key(this); }
            if (itemType == ITEM_TYPE.FLASHLIGHT) { item = new FlashLight(this); }
            item.Interact();
            item.Active();


        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.P))
            {
                //string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                string fileName = "SCREENSHOT-" + ".png";
                string filePath = Application.dataPath + "/05. Data/ScreenShot/" + fileName;
                Debug.Log("Ω∫º¶"!);
                StartCoroutine(ScreenShotCapture1(filePath));
            }

        }
        public IEnumerator ScreenShotCapture1(string filePath)
        {
            yield return new WaitForEndOfFrame();

            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            byte[] photo = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, photo);
        }
    }
}


