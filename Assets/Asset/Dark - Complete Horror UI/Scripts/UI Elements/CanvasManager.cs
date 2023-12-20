using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Michsky.UI.Dark
{
    public class CanvasManager : Singleton<CanvasManager>
    {
        public CanvasScaler canvasScaler;
        public GameObject noteUI;
        public TextMeshProUGUI noteText;

        void Start()
        {
            if (canvasScaler == null)
                canvasScaler = gameObject.GetComponent<CanvasScaler>();
        }

        public void ScaleCanvas(int scale = 1080)
        {
            canvasScaler.referenceResolution = new Vector2(canvasScaler.referenceResolution.x, scale);
        }

        public void NoteExit()
        {
            noteUI.SetActive(false);
        }
    }
}