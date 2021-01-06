using ETD.BuildingControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETD.UIControl
{
    public class UISelectionDescription : MonoBehaviour
    {
        [Header("Unique Information")]
        [SerializeField] string myType = "No Type";
        [SerializeField] string myName = "No Name";
        [SerializeField] Sprite myImage = null;
        [SerializeField] string actionCost = "No Cost";
        [SerializeField] actionTypes actionType = actionTypes.None;
        [TextArea]
        [SerializeField] string myStatistics = "No stats.";
        [TextArea]
        [SerializeField] string myDescription = "No description.";
        [SerializeField] Building buildingPrefab = null;
        

        public string GetMyType() { return myType; }
        public string GetMyName() { return myName; }
        public Sprite GetMyImage() { return myImage; }
        public string GetActionCost() { return actionCost; }
        public actionTypes GetActionType() { return actionType; }
        public string GetMyStatistics() { return myStatistics; }
        public string GetMyDescription() { return myDescription; }
        public Building GetBuildingPrefab() { return buildingPrefab; }
    }

}
