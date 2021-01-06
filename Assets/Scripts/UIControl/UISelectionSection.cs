using ETD.BuildingControl;
using ETD.SelectionControl;
using ETD.TowerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETD.UIControl
{
    public enum actionTypes { None, Build, Evolve, Research };

    public class UISelectionSection : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] Text selectedTypeRef = null;
        [SerializeField] Text selectedNameRef = null;
        [SerializeField] Image selectedImageRef = null;
        [SerializeField] Text statisticTextRef = null;
        [SerializeField] Text descriptiveTextRef = null;
        [SerializeField] Text costTextRef = null;
        [SerializeField] Button buildButton = null;
        [SerializeField] Button evolveButton = null;
        [SerializeField] Button researchButton = null;
        [SerializeField] Building buildingToBuild;

        UISelectionDescription lastSelected = null;
        UISelectionDescription selected = null;
        Color buttonColor;
        Color disabledButtonColor = new Color(0.1509434f, 0.1509434f, 0.1509434f);

        public void SetSelected(UISelectionDescription newSelected)
        {
            selected = newSelected;
            if(selected == null)
            {
                SetEnableSelectionUI(false); 
                lastSelected = null; 
                return;
            }
            SetEnableSelectionUI(true);
        }

        private void Start()
        {
            FindObjectOfType<SelectionHandler>().SetSelectionSection(this);
            SetEnableSelectionUI(false);
            buttonColor = buildButton.image.color;
        }

        private void Update()
        {
            if (selected != lastSelected)
            {
                UpdateSelectionInformation(selected);
            }
        }

        public void UpdateSelectionInformation(UISelectionDescription information)
        {
            selectedTypeRef.text = information.GetMyType();
            selectedNameRef.text = information.GetMyName();
            selectedImageRef.sprite = information.GetMyImage();
            statisticTextRef.text = information.GetMyStatistics();
            descriptiveTextRef.text = information.GetMyDescription();
            costTextRef.text = information.GetActionCost();
            ButtonControl(information.GetActionType());
            buildingToBuild = information.GetBuildingPrefab();
            BuildingSpawner spawner = FindObjectOfType<BuildingSpawner>();
            buildButton.onClick.AddListener(Build);
            lastSelected = selected;
        }

        private void Build()
        {
            FindObjectOfType<BuildingSpawner>().DisplayBuildingToSpawn(buildingToBuild);
        }

        private void SetEnableSelectionUI(bool status)
        {
            gameObject.SetActive(status);
        }

        private void ButtonControl(actionTypes action)
        {
            if(action == actionTypes.Build)
            {
                SetButtonColorAndActiveStatus(buildButton, true);
                SetButtonColorAndActiveStatus(evolveButton, false);
                SetButtonColorAndActiveStatus(researchButton, false);
            }
            if (action == actionTypes.Evolve)
            {
                SetButtonColorAndActiveStatus(buildButton, false);
                SetButtonColorAndActiveStatus(evolveButton, true);
                SetButtonColorAndActiveStatus(researchButton, false);
            }
            if (action == actionTypes.Research)
            {
                SetButtonColorAndActiveStatus(buildButton, false);
                SetButtonColorAndActiveStatus(evolveButton, false);
                SetButtonColorAndActiveStatus(researchButton, true);
            }
            if (action == actionTypes.None)
            {
                SetButtonColorAndActiveStatus(buildButton, false);
                SetButtonColorAndActiveStatus(evolveButton, false);
                SetButtonColorAndActiveStatus(researchButton, false);
            }

        }

        private void SetButtonColorAndActiveStatus(Button button, bool activeStatus)
        {
            button.interactable = activeStatus;
            if(activeStatus)
            {
                button.image.color = buttonColor;
            }
            else
            {
                button.image.color = disabledButtonColor;
            }
        }




    }
}
