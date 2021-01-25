using ETD.EnemyControl;
using ETD.UIControl;
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
        [SerializeField] int numberCanAttack = 1;

        Transform projectileParent;
        Enemy primaryTarget;
        Enemy[] targets;
        float attackTimer = Mathf.Infinity;

        public void SetProjectileParent(Transform parent)
        {
            projectileParent = parent;
        }

        public Enemy GetCurrentTarget()
        {
            return primaryTarget;
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
                if (primaryTarget != null)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) <
                        Vector3.Distance(enemy.transform.position, primaryTarget.transform.position))
                    {
                        primaryTarget = enemy;
                    }
                }
                else
                {
                    primaryTarget = enemy;
                }
            }
        }

        private bool IsTargetInRange()
        {
            if (primaryTarget == null) { return false; }
            if (Vector3.Distance(primaryTarget.transform.position, transform.position) > range)
            {
                primaryTarget = null;
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
                float processedDamage = GetComponent<DamageModifier>().CalculateDamageForProjectile
                    (damage, GetComponent<UISelectionDescription>().GetMyType(), primaryTarget.GetComponent<UISelectionDescription>().GetMyType());
                projectileInstance.SetTarget(primaryTarget);
                projectileInstance.SetDamage(processedDamage);
                projectileInstance.SetParentType(GetComponent<UISelectionDescription>().GetMyType());
                projectileInstance.SetAttacker(GetComponent<DamageModifier>());
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
