namespace GameOfWar.Characters
{
    using UnityEngine;
    using UnityEngine.Events;

    public class Mausoleum : MonoBehaviour
    {
        //SpawnEvent to add in enemies in tower
        [SerializeField] SpawnEvent spawnEvent;
        [System.Serializable]
        public class SpawnEvent : UnityEvent<GameObject>
        {
        }
        public GameObject infantryPrefab;
        public Transform spawnPoint;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnCharacter();
            }
        }
        public void SpawnCharacter()
        {
            GameObject instance = Instantiate(infantryPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnEvent.Invoke(instance);
        }

    }
}