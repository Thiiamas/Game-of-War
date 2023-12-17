namespace RPG.Combat
{
    using UnityEngine;
    using UnityEngine.Events;

    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHitEvent;
        public void OnHit()
        {
            onHitEvent.Invoke();
        }
    }
}