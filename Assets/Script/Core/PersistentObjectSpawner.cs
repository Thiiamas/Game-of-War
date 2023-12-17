namespace RPG.Core
{
    using UnityEngine;

    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (!hasSpawned)
            {
                hasSpawned = true;
                SpawnPersistentObject();
            }
        }

        void SpawnPersistentObject()
        {
            GameObject go = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(go);
        }
    }
}