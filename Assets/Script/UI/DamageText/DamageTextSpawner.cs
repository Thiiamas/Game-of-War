using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;


        void Start()
        {

        }

        public void Spawn(float damage)
        {
            if (damageTextPrefab == null) return;
            DamageText damageText = Instantiate(damageTextPrefab, transform);
            damageText.SetValue(damage);
        }
    }
}
