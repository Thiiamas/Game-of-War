namespace RPG.Cinematics
{
    using RPG.Control;
    using RPG.Core;
    using UnityEngine;
    using UnityEngine.Playables;

    public class CinematicControlRemover : MonoBehaviour
    {
        PlayableDirector playableDirector;

        GameObject player;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        void DisableControl(PlayableDirector pd)
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;

        }

        private void OnEnable()
        {
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            playableDirector.played -= DisableControl;
            playableDirector.stopped -= EnableControl;
        }

        void EnableControl(PlayableDirector pd)
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            player.GetComponent<PlayerController>().enabled = true; ;

        }

    }
}