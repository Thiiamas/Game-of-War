namespace RPG.Stats
{
    using UnityEngine;
    using UnityEngine.UI;

    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        private void Update()
        {
            GetComponent<Text>().text = baseStats.GetExperienceLevel().ToString();
        }
    }
}