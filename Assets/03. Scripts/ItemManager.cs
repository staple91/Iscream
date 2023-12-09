using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using No;
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
    }
    public class Medicine : RecoveryItem
    {
        public Medicine(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("약이닷"); }
        public override void Active()
        {
            //im.player.정신력 스탯 += 회복량
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
    }
    public class Lantern : EquipmentItem
    {
        public Lantern(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("랜턴이닷"); }
        public override void Active()
        {
        }
    }
    public class FlashLight : EquipmentItem
    {

        public FlashLight(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("플래시라이트!"); }
        public override void Active()
        {
            Debug.Log("들어옴?");
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

        public override void Active()
        {
        }

        public override void Interact() { Debug.Log("열쇠닷"); }
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
    }
    public class Battery : ConsumableItem
    {
        public Battery(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("배터리닷"); }
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
        // --노신영 주석--
        // 아이템 구현방법이 잘 전달이 안된것 같아서 주석 남겨요
        // 일단 아이템은 게임 내에 존재해야되는 오브젝트이고
        // 레이케스트를 쏴서 콜라이더와 충돌처리를 해서
        // 그 오브젝트가 가지고 있는 클래스(모노비헤이비어를 상속받은 컴포넌트)의 인터페이스를 가져와서 사용한다는 뜻이였어요 //Player.cs 107번 라인 참고
        // 그래서 MonoBehaviour랑 IInteractivable이랑 동시에 상속을 받아야 해요
        // 전략패턴을 사용하고싶은거는 이해를 했고
        // 그럼 ItemManager에 인터페이스를 상속받으면 좋을 것 같아요
        // 그리고 이름도 객체에 하나하나 들어갈 컴포넌트니까 매니저보다는 ItemObject나 다른 이름으로 쓰는게 좀 더 직관적일거라고 생각해요.
        // + Start문에 if문 나열된거 switch case 문으로 바꿔놨습니다.
        // ++ 변수명은 소문자로 시작해주시구 함수, 프로퍼티명은 대문자 시작으로 해주세요!!
        // 읽어보시고 이해안되거나 궁금하거나 이건좀 아닌것같은데 싶은거는 갠톡or디코 해주시고 이 주석은 다 읽으셧으면 지워주시면 됩니당
        // 남은 프로젝트 기간 화이팅이에요

        public Player player;
        public Item item;
        public ITEM_TYPE itemType;
        public Light flashLight;
        public float BatteryTime = 60f;
        ScreenShot screenShot;

        // Start is called before the first frame update
        void Start()
        {
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


