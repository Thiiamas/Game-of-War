namespace RPG.Combat
{
    using UnityEngine;
    using UnityEngine.UI;

    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            GetComponent<Text>().text = string.Format("{0:0}/{1:0}", fighter.GetTarget().GetHealthPoint().ToString(), fighter.GetTarget().GetMaxHealthPoint().ToString());
        }
    }
}