using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using LeeJungChul;
using No;
using JetBrains.Annotations;
using UnityEditor.UI;

namespace YoungJaeKim
{
    public abstract class Item
    {
        public ItemObject itemObj;
        public Item(ItemObject itemObj)
        {
            this.itemObj = itemObj;
        }

        public abstract void Interact();
        public abstract void Active();
        public abstract void Explain();

    }
    public abstract class EquipmentItem : Item
    {
        public EquipmentItem(ItemObject im) : base(im) { }
        public override void Interact()
        {
            itemObj.Owner.inven.AddItem(this.itemObj);
        }
    }

    public class LightMirrorItem : Item
    {
        public LightMirrorItem(ItemObject im) : base(im)
        {

        }
        public override void Active()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, 1 << LayerMask.NameToLayer("Floor")))
            {
                itemObj.transform.position = hit.point;
                itemObj.Owner.StartCoroutine(RotateMirrorCo());
            }
        }

        public override void Explain()
        {
        }

        public override void Interact()
        {
            itemObj.Owner.inven.AddItem(this.itemObj);
        }

        IEnumerator RotateMirrorCo()
        {
            itemObj.Owner.IsMoveable = false;
            StarterAssetsInputs input = itemObj.Owner.GetComponent<StarterAssetsInputs>();
            FirstPersonController controller = itemObj.Owner.GetComponent<FirstPersonController>();
            float threshold = 0.01f;
            while (!Input.GetKey(KeyCode.E))
            {
                if (input.look.sqrMagnitude >= threshold)
                {
                    float deltaTimeMultiplier = controller.IsCurrentDevice ? 1.0f : Time.deltaTime;


                    itemObj.transform.Rotate(Vector3.up, input.look.x * controller.RotationSpeed * deltaTimeMultiplier);
                    yield return new WaitForEndOfFrame();
                }
                else
                    yield return new WaitForEndOfFrame();

            }
            itemObj.Owner.IsMoveable = true;
        }
    }


    public class CameraEquip : EquipmentItem
    {
        ScreenShot screenShot;
        public CameraEquip(ItemObject im) : base(im) { }
        public override void Interact()
        {
            Active();
        }
        public override void Active()
        {
            Debug.Log("찍혔닷");

            //string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string fileName = "SCREENSHOT-" + ".png";
            string filePath = Application.dataPath + "/05. Data/ScreenShot/" + fileName;
            
            itemObj.Owner.StartCoroutine(ScreenShotCapture1(filePath));
            
            //ScreenCapture.CaptureScreenshot(Application.dataPath+"/05. Data/ScreenShot/" + fileName);
            //위에거는 되긴하는데 저장이 너무 오래걸림

        }
        public override void Explain()
        {
            //카메라이다. 대상을 찍을수 있으며, 찍은 정보는 노트에 기록되어진다.
        }

        public IEnumerator ScreenShotCapture1(string filePath)
        {
            Debug.Log("스샷"!);
            yield return new WaitForEndOfFrame();


            /*Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();*/ //플레이어 시야로 스크린샷

            RenderTexture.active = itemObj.screenShotTexture;//특정 카메라시점의 랜더 텍스쳐를 따오면 그것의 스크린샷을 찍을수 있습니다.
            Texture2D texture = new Texture2D(itemObj.screenShotTexture.width, itemObj.screenShotTexture.height);
            texture.ReadPixels(new Rect(0, 0, itemObj.screenShotTexture.width, itemObj.screenShotTexture.height), 0, 0);
            texture.Apply();

            byte[] photo = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, photo);
        }

    }
    public class FlashLight : EquipmentItem
    {
        bool isActive = false;
        float lightPower;
        Light light;
        float maxBatttery = 100;
        float curBattery = 0;
        public FlashLight(ItemObject im) : base(im)
        {
            Debug.Log(itemObj.transform.GetChild(0));
            light = itemObj.transform.GetChild(0).GetComponent<Light>();
            curBattery = maxBatttery;
            lightPower = light.intensity;
        }
        public override void Active()
        {
            isActive = !isActive;
            if (isActive)
                itemObj.Owner.StartCoroutine(LightCo());
        }
        IEnumerator LightCo()
        {
            while(isActive && curBattery > 0)
            {
                light.intensity = lightPower;
                curBattery -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        public override void Explain()
        {
            //플래시라이트다. 맵을 밝혀줄수 있으나, 시간에 따라 빛의 세기가 줄어들고
            //결국 꺼지기 때문에 배터리로 재충전이 필요하다.
        }

        public void AddBattery(float value)
        {
            curBattery += value;
        }

    }
    public class Key : EquipmentItem
    {
        public Key(ItemObject im) : base(im) { }

        public override void Active()
        {

        }

        public override void Explain()
        {
            //열쇠다. 어떤 상자를 열수 있을지도??
        }

    }

    public class Battery : Item
    {
        float batteryValue = 50f;
        public Battery(ItemObject im) : base(im) { }
        public override void Interact() { Active(); }
        public override void Active()
        {
             FlashLight tempLight = ((FlashLight)itemObj.Owner.inven.FindItem(ITEM_TYPE.FLASHLIGHT).item);
            tempLight.AddBattery(batteryValue);
        }
        public override void Explain()
        {
            //배터리이다. 플래시라이트를 충전하는데 쓰인다.
        }
    }

    public enum ITEM_TYPE
    {
        CAMERA,
        KEY,
        BATTERY,
        FLASHLIGHT,
    }

    public class ItemObject : MonoBehaviour, IInteractable
    {
        Player owner;
        public Item item;
        public ITEM_TYPE itemType;
        public RenderTexture screenShotTexture;
        Transform fpsTr;
        Transform tpsTr;
        public Player Owner
        {
            get => owner;
            set
            {
                owner = value;
                if(owner != null)
                {
                    fpsTr = owner.fpsHandTr;
                    tpsTr = owner.tpsHandTr;
                }
            }
        }

        public void Interact()
        {
            item.Interact();
            transform.SetParent(fpsTr);
            if(owner != null)
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        }

        // Start is called before the first frame update
        void Start()
        {
            
            switch (itemType)
            {
                case ITEM_TYPE.CAMERA:
                    item = new CameraEquip(this); break;
                case ITEM_TYPE.BATTERY: 
                    item = new Battery(this); break;
                case ITEM_TYPE.KEY:
                    item = new Key(this); break;
                case ITEM_TYPE.FLASHLIGHT: 
                    item = new FlashLight(this); break;
            }
           
        }

    }
}


