namespace RPG.SceneManagement
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.SceneManagement;
    using UnityEngine.AI;
    using System;
    using RPG.Control;

    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D
        }

        [SerializeField] int SceneToLoad;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 1.5f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.Fade(1, fadeOutTime);
            //Save current level
            SavingWrapper sWrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;
            sWrapper.Save();

            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            // Load current level
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            sWrapper.Load();
            Portal destinationPortal = GetOtherPortal();
            UpdatePlayer(destinationPortal);
            sWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            fader.Fade(0, fadeInTime);
            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.transform.position);
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;
        }

        public Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != this.destination) continue;
                return portal;
            }
            return null;
        }
    }
}