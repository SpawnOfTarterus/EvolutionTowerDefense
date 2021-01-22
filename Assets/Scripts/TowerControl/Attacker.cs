using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] float range;
        [SerializeField] float timeBetweenAttacks;
        [SerializeField] Projectile projectile;

        Transform projectileParent;
        Enemy target;
        float attackTimer = Mathf.Infinity;

        public void SetProjectileParent(Transform parent)
        {
            projectileParent = parent;
        }

        public int GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return range;
        }

        public float GetAttackSpeed()
        {
            return timeBetweenAttacks;
        }

        private void Update()
        {
            AttackMechanic();
        }

        private void AttackMechanic()
        {
            GetTarget();
            if (!IsTargetInRange()) { return; }
            AttackTarget();
        }

        private void GetTarget()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) > range) { continue; }
                if (target != null)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) <
                        Vector3.Distance(enemy.transform.position, target.transform.position))
                    {
                        target = enemy;
                    }
                }
                else
                {
                    target = enemy;
                }
            }
        }

        private bool IsTargetInRange()
        {
            if (target == null) { return false; }
            if (Vector3.Distance(target.transform.position, transform.position) > range)
            {
                target = null;
                return false;
            }
            return true;
        }

        private void AttackTarget()
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= timeBetweenAttacks)
            {
                Projectile projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity, projectileParent);
                projectileInstance.SetTarget(target);
                projectileInstance.SetDamage(damage);
                attackTimer = 0f;
            }
        }

        [ExecuteAlways]
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
