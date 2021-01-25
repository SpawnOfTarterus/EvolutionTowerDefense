using ETD.BuildingControl;
using ETD.EnemyControl;
using ETD.TowerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETD.UIControl
{
    public class UISelectionDescription : MonoBehaviour
    {
        [Header("Unique Information")]
        [SerializeField] evoTypes myType = evoTypes.None;
        [SerializeField] string myName = "No Name";
        [SerializeField] Sprite myImage = null;
        [SerializeField] int buildCost = 0;
        [SerializeField] actionTypes actionType = actionTypes.None;
        [SerializeField] Tower[] possibleEvolutions;
        [SerializeField] buildingTypes buildingType = buildingTypes.Tower;
        [TextArea]
        [SerializeField] string myStatistics = "No stats.";
        [TextArea]
        [SerializeField] string myDescription = "No description.";
        [SerializeField] GameObject buildingPrefab = null;
        
        public evoTypes GetMyType() { return myType; }
        public string GetMyName() { return myName; }
        public Sprite GetMyImage() { return myImage; }
        public int GetBuildCost() 
        { 
            if(myType == evoTypes.Human) { return Mathf.RoundToInt(buildCost - (buildCost * 0.25f)); }
            return buildCost; 
        }
        public actionTypes GetActionType() { return actionType; }
        public buildingTypes GetBuildingType() { return buildingType; }
        public string GetMyStatistics() { return myStatistics; }
        public string GetMyDescription() { return myDescription; }
        public GameObject GetBuildingPrefab() { return buildingPrefab; }

        public Tower[] GetPossibleEvolutions()
        {
            return possibleEvolutions;
        }

        private void Start()
        {
            SetInfo();
        }

        private void SetInfo()
        {
            //type put in inspector
            //name put in inspector
            //image put in inspector
            //action cost put in inspector
            //action type put in inspector
            SetStatistics();
            //description set in inspector;
            //prefab set in inspector (for building buttons only)
        }

        private void SetStatistics()
        {
            if (GetComponent<Tower>())
            {
                int damage = GetComponent<Attacker>().GetDamage();
                float range = GetComponent<Attacker>().GetRange();
                float attackSpeed = GetComponent<Attacker>().GetAttackSpeed();
                myStatistics = $"Damage \t\t-\t {damage} \nRange \t\t\t-\t {range} \nAttackSpeed -\t {attackSpeed}";
            }
            else if (GetComponent<Enemy>())
            {
                float moveSpeed = GetComponent<Mover>().GetMoveSpeed();
                int maxHealth = GetComponent<Health>().GetMaxHealth();
                myStatistics = $"Health - {maxHealth} \n Speed - {moveSpeed}";
            }
            else if(GetComponent<Button>())
            {
                if(buildingPrefab.GetComponent<Tower>())
                {
                    int damage = buildingPrefab.GetComponent<Attacker>().GetDamage();
                    float range = buildingPrefab.GetComponent<Attacker>().GetRange();
                    float attackSpeed = buildingPrefab.GetComponent<Attacker>().GetAttackSpeed();
                    myStatistics = $"Damage \t\t-\t {damage} \nRange \t\t\t-\t {range} \nAttackSpeed -\t {attackSpeed}";
                }
                else
                {
                    myStatistics = "None";
                }
            }
            else
            {
                myStatistics = "None";
            }
            
        }

    }
}
