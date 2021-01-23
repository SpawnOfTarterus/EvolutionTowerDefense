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
        EvoTypes parentType;
        Enemy target;

        public void SetDamage(float newDamage)
        {
            damage = newDamage;
        }

        public void SetParentType(EvoTypes type)
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
            other.GetComponent<DefenceApplicator>().ApplyDamageReduction(damage, parentType);
            Destroy(gameObject);
        }
    }
}
