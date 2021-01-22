using ETD.PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.EnemyControl
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] int currentHealth;

        bool isDead = false;

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        public void Die()
        {
            if(isDead) { return; }
            isDead = true;
            Enemy enemyComponent = GetComponent<Enemy>();
            enemyComponent.GetMySpawner().RemoveFromEnemiesInPlay(enemyComponent);
            if(currentHealth == 0)
            {
                FindObjectOfType<GoldController>().GainGold(enemyComponent.GetGoldReward());
            }
            Destroy(gameObject);
        }

    }
}
