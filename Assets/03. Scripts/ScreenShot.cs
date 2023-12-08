using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YoungJaeKim
{
    public class ScreenShot : MonoBehaviour
    {
        public IEnumerator ScreenShotCapture(string filePath)
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
