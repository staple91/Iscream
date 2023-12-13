using No;
using UnityEngine;

namespace LeverSystem
{
    public class LeverItem : Puzzle
    {
        public ObjectType _objectType = ObjectType.None;
        public enum ObjectType { None, Lever, TestButton, ResetButton }

        [SerializeField] public int leverNumber = 1;

        [SerializeField] private string animationName = "Handle_Pull";

        [SerializeField] private LeverSystemController _leverSystemController = null;

        private Animator handleAnimation;

        private void Start()
        {
            handleAnimation = GetComponentInChildren<Animator>();
            _leverSystemController = GetComponentInParent<LeverSystemController>();
        }

        public override void Interact()
        {
            switch (_objectType)
            {
                case ObjectType.Lever:
                    LeverNumber();
                    break;
                case ObjectType.TestButton:
                    LeverCheck();
                    break;
                case ObjectType.ResetButton:
                    LeverReset();
                    break;
            }
        }

        public void LeverNumber()
        {
            _leverSystemController.initLeverPull(this, leverNumber);
        }

        public void HandleAnimation()
        {
            handleAnimation.Play(animationName, 0, 0.0f);
        }

        public void LeverReset()
        {
            _leverSystemController.LeverReset();
        }

        public void LeverCheck()
        {
            _leverSystemController.LeverCheck();
        }    
    }
}