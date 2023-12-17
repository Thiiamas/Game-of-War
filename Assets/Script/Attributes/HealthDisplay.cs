namespace RPG.Attributes
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
        private void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoint().ToString(), health.GetMaxHealthPoint().ToString());
        }
    }
}