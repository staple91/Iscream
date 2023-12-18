using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YoungJaeKim
{
    public class ScreenShot : MonoBehaviour
    {
        public RenderTexture screenShotTexture;

        public void Active()
        {
            Debug.Log("ÂïÇû´å");

            //string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string fileName = "SCREENSHOT-" + ".png";
            string filePath = Application.dataPath + "/05. Data/ScreenShot/" + fileName;
            
            StartCoroutine(ScreenShotCapture123(filePath));
        }
        public IEnumerator ScreenShotCapture123(string filePath)
        {
            yield return new WaitForEndOfFrame();
            RenderTexture.active = screenShotTexture;
            Texture2D texture = new Texture2D(screenShotTexture.width, screenShotTexture.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, screenShotTexture.width, screenShotTexture.height), 0, 0);            
            texture.Apply();

            byte[] photo = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, photo);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                Active();
            }
        }


    }
}
