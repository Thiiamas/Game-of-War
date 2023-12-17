namespace RPG.Cinematics
{
    using System;
    using RPG.Saving;
    using UnityEngine;
    using UnityEngine.Playables;

    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player" || alreadyTriggered) return;
            GetComponent<PlayableDirector>().Play();
            alreadyTriggered = true;
        }

        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (Boolean)state;
        }
    }
}

