/// REMEMBER: This script has a custom editor called "LeverSystemControllerEditor", found in the "Editor" folder. You will need to add new properties to this
/// if you create new variables / fields in this script. Contact me if you have any troubles at all!

using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using PangGom;

namespace LeverSystem
{
    public class LeverSystemController : MonoBehaviour
    {
        [Tooltip("Order the levers should be pulled")]
        [SerializeField] private string leverOrder = "12345";

        [Tooltip("Pull Limit - Match this with the number of values in the leverOrder")]
        [SerializeField] private int pullLimit = 5;

        [Tooltip("Time before pulling lever after interacting")]
        [SerializeField] private float pullTimer = 1.0f;

        [Tooltip("Add each of the levers and buttons that you will interact with")]
        [SerializeField] private GameObject[] interactiveObjects = null;

        [Tooltip("Control Box Switches (Animated)")]
        [SerializeField] private Animator readySwitch = null;
        [SerializeField] private Animator limitReachedSwitch = null;
        [SerializeField] private Animator acceptedSwitch = null;
        [SerializeField] private Animator resettingSwitch = null;

        [Tooltip("Control Unit Lights")]
        [SerializeField] private GameObject readyLight = null;
        [SerializeField] private GameObject limitReachedLight = null;
        [SerializeField] private GameObject acceptedLight = null;
        [SerializeField] private GameObject resettingLight = null;

        //Accept / Reset Buttons
        [SerializeField] private Animator testButton = null;
        [SerializeField] private Animator resetButton = null;

        //Switch Object - Animation Names
        [SerializeField] private string switchOnName = "Switch_On";
        [SerializeField] private string switchOffName = "Switch_Off";
        [SerializeField] private string redButtonName = "RedButton_Push";

        //Sound Effects
        [SerializeField] private Sound switchPullSound = null;
        [SerializeField] private Sound switchFailSound = null;
        [SerializeField] private Sound switchDoorSound = null;

        //Unity Events
        [SerializeField] private UnityEvent LeverPower = null;

        private string playerOrder = null;
        private int pulls;
        private bool canPull = true;
        private bool resetting;

        private Material readyBtnMat;
        private Material resettingBtnMat;
        private Material acceptedBtnMat;
        private Material limitBtnMat;
        private BookClose puzzleClear;

        private TextMesh leverText;

        private void Start()
        {
            SetMaterials();
            InitializeSwitches();
            puzzleClear = GetComponentInChildren<BookClose>();
            leverText = GetComponentInChildren<TextMesh>();
        }

        private void SetMaterials()
        {
            readyBtnMat = readyLight.GetComponent<Renderer>().material;
            resettingBtnMat = resettingLight.GetComponent<Renderer>().material;
            acceptedBtnMat = acceptedLight.GetComponent<Renderer>().material;
            limitBtnMat = limitReachedLight.GetComponent<Renderer>().material;
            readyBtnMat.color = Color.green;
        }

        private void InitializeSwitches()
        {
            readySwitch.Play(switchOnName, 0, 0.0f);
            resettingSwitch.Play(switchOffName, 0, 0.0f);
            acceptedSwitch.Play(switchOffName, 0, 0.0f);
            limitReachedSwitch.Play(switchOffName, 0, 0.0f);
        }

        void LeverInteraction()
        {
            LeverPower.Invoke();
        }

        IEnumerator Timer()
        {
            canPull = false;
            yield return new WaitForSeconds(pullTimer);
            canPull = true;
        }

        public void initLeverPull(LeverItem _leverItem, int leverNumber)
        {
            if (canPull && pulls <= pullLimit - 1)
            {
                _leverItem.HandleAnimation();
                LeverPull(leverNumber);
            }
        }

        public void LeverPull(int leverNumber)
        {
            playerOrder = playerOrder + leverNumber;
            leverText.text += leverNumber + (" ");
            pulls++;
            if (canPull)
            {
                StartCoroutine(Timer());
                SwitchPullAudio();
                if (pulls >= pullLimit)
                {
                    UpdateSwitches(Color.red, Color.green, true);
                }
            }
        }

        public void LeverReset()
        {
            leverText.text = ("실패 답 입력 : ");
            pulls = 0;
            playerOrder = "";
            SwitchFailAudio();
            resetButton.Play(redButtonName, 0, 0.0f);
            resettingBtnMat.color = Color.green;
            resettingSwitch.Play(switchOnName, 0, 0.0f);

            StartCoroutine(ResetTimer(1.0f));
            UpdateSwitches(Color.green, Color.red, false);
        }

        private void UpdateSwitches(Color readyColor, Color limitColor, bool playAnimation)
        {
            readyBtnMat.color = readyColor;
            limitBtnMat.color = limitColor;

            readySwitch.Play(switchOffName, 0, 0.0f);
            if (playAnimation)
            {
                limitReachedSwitch.Play(switchOnName, 0, 0.0f);
            }
            else
            {
                limitReachedSwitch.Play(switchOffName, 0, 0.0f);
            }
        }

        public void LeverCheck()
        {
            leverText.text = ("답 입력 : ");
            testButton.Play(redButtonName, 0, 0.0f);
            if (playerOrder == leverOrder)
            {
                CompleteLeverCheck();
            }
            else
            {
                LeverReset();               
            }
        }

        public void CompleteLeverCheck()
        {
            leverText.text = ("성공");
            pulls = 0;
            SwitchDoorOpenAudio();

            LeverInteraction();

            foreach (GameObject obj in interactiveObjects)
            {
                obj.gameObject.tag = "Untagged";
            }

            readyBtnMat.color = Color.red;
            resettingBtnMat.color = Color.red;
            acceptedBtnMat.color = Color.green;
            limitBtnMat.color = Color.red;

            readySwitch.Play(switchOffName, 0, 0.0f);
            resettingSwitch.Play(switchOffName, 0, 0.0f);
            acceptedSwitch.Play(switchOnName, 0, 0.0f);
            limitReachedSwitch.Play(switchOffName, 0, 0.0f);

            puzzleClear.gifAnimator.SetTrigger("Clear");
        }

        IEnumerator ResetTimer(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            readyBtnMat.color = Color.green;
            resettingBtnMat.color = Color.red;

            readySwitch.Play(switchOnName, 0, 0.0f);
            resettingSwitch.Play(switchOffName, 0, 0.0f);
        }

        void SwitchPullAudio()
        {
            SoundManager.Instance.PlayAudio(SoundManager.Instance.leverPullsound, false);
        }

        void SwitchFailAudio()
        {
            SoundManager.Instance.PlayAudio(SoundManager.Instance.leverFailsound, false);
        }

        void SwitchDoorOpenAudio()
        {
            SoundManager.Instance.PlayAudio(SoundManager.Instance.leverSucesssound, false);
        }


        private void OnDestroy()
        {
            Destroy(readyBtnMat);
            Destroy(resettingBtnMat);
            Destroy(acceptedBtnMat);
            Destroy(limitBtnMat);
        }
    }
}
