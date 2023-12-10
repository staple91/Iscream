using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using No;
using JetBrains.Annotations;

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
        public abstract void Active();
        public abstract void Explain();

    }
    public abstract class RecoveryItem : Item
    {
        public RecoveryItem(ItemManager im) : base(im) { }
        public override void Interact()
        {

        }
    }
    public class Food : RecoveryItem
    {
        public Food(ItemManager im) : base(im) { }

        public override void Interact() { Debug.Log("음식이닷"); }

        public override void Active()
        {
            //im.player.정신력 스탯 += 회복량
        }
        public override void Explain()
        {
            //음식이다. 플레이어의 정신력을 회복시켜준다.
        }

    }
    public class Medicine : RecoveryItem
    {
        public Medicine(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("약이닷"); }
        public override void Active()
        {
            //im.player.정신력 스탯 += 회복량
        }
        public override void Explain()
        {
            //약이다. 플레이어의 정신력을 대량 회복시켜준다.
        }


    }
    public class Beverage : RecoveryItem
    {
        public Beverage(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("음료수닷"); }
        public override void Active()
        {
            //im.player.정신력 스탯 += 회복량
        }
        public override void Explain()
        {
            //음료수이다. 플레이어의 정신력을 소량 회복시켜준다.
        }
    }
    public abstract class EquipmentItem : Item
    {
        public EquipmentItem(ItemManager im) : base(im) { }
        public override void Interact()
        {

        }
    }
    public class CameraEquip : EquipmentItem
    {
        public CameraEquip(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("카메라닷"); }
        public override void Active()
        {
            Debug.Log("찍혔닷");


            //ScreenCapture.CaptureScreenshot(Application.dataPath+"/05. Data/ScreenShot/" + fileName);
            //위에거는 되긴하는데 저장이 너무 오래걸림



        }
        public override void Explain()
        {
            //카메라이다. 대상을 찍을수 있으며, 찍은 정보는 노트에 기록되어진다.
        }

    }
    public class Lantern : EquipmentItem
    {
        public Lantern(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("랜턴이닷"); }
        public override void Active()
        {
        }
        public override void Explain()
        {
            //랜턴이다. 빛을 밝히는 용도로도 쓰지만, 특별한 능력을 가지고 있다.
        }

    }
    public class FlashLight : EquipmentItem
    {

        public FlashLight(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("플래시라이트!"); }
        public override void Active()
        {
            Debug.Log("들어옴?");
            
            
            //im.flashLight.intensity = im.lightPower;
            im.lightOn = true;
            

        }
        public override void Explain()
        {
            //플래시라이트다. 맵을 밝혀줄수 있으나, 시간에 따라 빛의 세기가 줄어들고
            //결국 꺼지기 때문에 배터리로 재충전이 필요하다.
        }

    }
    public class Key : EquipmentItem
    {
        public Key(ItemManager im) : base(im) { }

        public override void Active()
        {
        }

        public override void Interact() { Debug.Log("열쇠닷"); }
        public override void Explain()
        {
            //열쇠다. 어떤 상자를 열수 있을지도??
        }

    }
    public class ConsumableItem : Item
    {
        public ConsumableItem(ItemManager im) : base(im) { }

        public override void Active()
        {
        }

        public override void Interact()
        {

        }
        public override void Explain() { }
    }
    public class Battery : ConsumableItem
    {
        public Battery(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("배터리닷"); }
        public override void Active()
        {
            im.flashLight.intensity = 2.1f;
        }
        public override void Explain()
        {
            //배터리이다. 플래시라이트를 충전하는데 쓰인다.
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
        public bool lightOn;
        public float lightPower;
        public Player player;
        public Item item;
        public ITEM_TYPE itemType;
        public Light flashLight;
        public float batteryTime;
        ScreenShot screenShot;

        // Start is called before the first frame update
        void Start()
        {
            lightOn = false;
            //flashLight = GetComponent<Light>();
            switch (itemType)
            {
                case ITEM_TYPE.FOOD:
                    item = new Food(this); break;
                case ITEM_TYPE.MEDICINE:
                    item = new Medicine(this); break;
                case ITEM_TYPE.BEVERAGE:
                    item = new Beverage(this); break;
                case ITEM_TYPE.CAMERA:
                    item = new CameraEquip(this); break;
                case ITEM_TYPE.BATTERY: 
                    item = new Battery(this); break;
                case ITEM_TYPE.LANTERN:
                    item = new Lantern(this); break;
                case ITEM_TYPE.KEY:
                    item = new Key(this); break;
                case ITEM_TYPE.FLASHLIGHT: 
                    item = new FlashLight(this); break;
            }
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
                Debug.Log("스샷"!);
                StartCoroutine(ScreenShotCapture1(filePath));
            }
            //lightPower -= Time.deltaTime;
            batteryTime += 0.0001f;
            if(batteryTime == 1) { batteryTime = 1; }
            if (lightOn)
            {
                
               
                lightPower = Mathf.Lerp(2.3f, 0, batteryTime);


                flashLight.intensity = lightPower;


                if (flashLight.intensity <= 0)
                {
                    lightOn = false;
                }
                Debug.Log(flashLight.intensity);
                
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


