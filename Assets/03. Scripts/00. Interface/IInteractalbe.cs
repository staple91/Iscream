using KimKyeongHun;

namespace No
{
    interface IInteractable
    {
        void Interact();
        public Player Owner { get; set; }
    }
}