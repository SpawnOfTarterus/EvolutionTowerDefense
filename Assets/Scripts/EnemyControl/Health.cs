using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.EnemyControl
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] int currentHealth;

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
            Enemy enemyComponent = GetComponent<Enemy>();
            enemyComponent.GetMySpawner().RemoveFromEnemiesInPlay(enemyComponent);
            Destroy(gameObject);
        }

    }
}
