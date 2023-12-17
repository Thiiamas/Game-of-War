namespace RPG.SceneManagement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using RPG.Saving;
    using UnityEngine;

    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] bool loadOnPlay = false;
        [SerializeField] float fadeInTime = .5f;
        const string defaultFile = "save";

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {

            if (loadOnPlay)
            {
                yield return GetComponent<SavingSystem>().LoadLastScene(defaultFile);
                Fader fader = FindObjectOfType<Fader>();
                fader.FadeOutImmediate();
                yield return fader.Fade(0, fadeInTime);
            }

        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Delete();
            }
        }

        public void Save()
        {
            print("save");
            GetComponent<SavingSystem>().Save(defaultFile);
        }

        public void Load()
        {
            print("load");
            GetComponent<SavingSystem>().Load(defaultFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultFile);
        }
    }
}

