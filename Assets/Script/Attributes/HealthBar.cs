namespace RPG.Attributes
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class HealthBar : MonoBehaviour
    {

        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Canvas rootCanvas;
        void Start()
        {

        }

        private void Update()
        {
            if (health == null) { return; }
            if (Mathf.Approximately(health.GetFraction(), 0) || health.isDead)
            {
                rootCanvas.enabled = false;
                return;
            }
            if (health.isFullLife())
            {
                rootCanvas.enabled = false; return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(health.GetFraction(), 1, 1);

        }



    }
}
