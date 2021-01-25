using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 1f;

        float damage;
        evoTypes parentType;
        Enemy target;
        DamageModifier attacker;

        public void SetDamage(float newDamage)
        {
            damage = newDamage;
        }

        public void SetAttacker(DamageModifier myInstantiator)
        {
            attacker = myInstantiator;
        }

        public void SetParentType(evoTypes type)
        {
            parentType = type;
        }

        public void SetTarget(Enemy newTarget)
        {
            target = newTarget;
        }

        private void Update()
        {
            SelfDestruct();
            MoveAtTarget();
        }

        private void MoveAtTarget()
        {
            if(target == null) { return; }
            Vector3 targetPos = target.GetHitTransform().position;
            transform.LookAt(targetPos);
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }

        private void SelfDestruct()
        {
            if(target == null)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
            other.GetComponent<DefenceApplicator>().ApplyDamageReduction(damage, parentType, attacker);
            Destroy(gameObject);
        }
    }
}
