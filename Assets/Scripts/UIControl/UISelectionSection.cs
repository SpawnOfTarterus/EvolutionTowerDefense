using ETD.BuildingControl;
using ETD.SelectionControl;
using ETD.TowerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETD.UIControl
{
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
        [SerializeField] RectTransform spawnMenu = null;
        [SerializeField] UIEvolutionMenu evolutionMenu = null;

        GameObject buildingToBuild;
        UISelectionDescription lastSelected = null;
        UISelectionDescription selected = null;
        Color buttonColor;
        Color disabledButtonColor = new Color(0.1509434f, 0.1509434f, 0.1509434f);
        public bool isEvolving = false;
        Tower towerEvolving = null;

        public void SetSelected(UISelectionDescription newSelected, bool evolutionSelected)
        {
            if (!evolutionSelected) { FindObjectOfType<UISliderControl>().ForceCloseMenus(); }
            selected = newSelected;
            if(selected == null)
            {
                isEvolving = false;
                SetEnableSelectionUI(false); 
                lastSelected = null; 
                return;
            }
            PostBuildingRedirect(selected);
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
                buildButton.onClick.RemoveAllListeners();
                UpdateSelectionInformation(selected);
            }
        }

        public void ToggleIsEvolving()
        {
            isEvolving = !isEvolving;
            if(isEvolving) { towerEvolving = selected.GetComponent<Tower>(); }
        }

        private void PostBuildingRedirect(UISelectionDescription uISDtoSwitch)
        {
            CheckForResearchCenter(uISDtoSwitch);
            CheckForUpgradeCenter(uISDtoSwitch);
            CheckForEnemyBreeder(uISDtoSwitch);
        }

        private void CheckForResearchCenter(UISelectionDescription uISDtoSwitch)
        {
            if (uISDtoSwitch.GetBuildingType() == buildingTypes.ResearchCenter)
            {
                if (FindObjectOfType<ResearchCenter>())
                {
                    selected = FindObjectOfType<ResearchCenter>().GetComponent<UISelectionDescription>();
                }
            }
        }

        private void CheckForUpgradeCenter(UISelectionDescription uISDtoSwitch)
        {
            if (uISDtoSwitch.GetBuildingType() == buildingTypes.UpgradeCenter)
            {
                if (FindObjectOfType<UpgradeCenter>())
                {
                    selected = FindObjectOfType<UpgradeCenter>().GetComponent<UISelectionDescription>();
                }
            }
        }

        private void CheckForEnemyBreeder(UISelectionDescription uISDtoSwitch)
        {
            if (uISDtoSwitch.GetBuildingType() == buildingTypes.EnemyBreeder)
            {
                if (FindObjectOfType<EnemyBreeder>())
                {
                    selected = FindObjectOfType<EnemyBreeder>().GetComponent<UISelectionDescription>();
                }
            }
        }

        public void UpdateSelectionInformation(UISelectionDescription information)
        {
            DisplayType(information);
            selectedNameRef.text = information.GetMyName();
            selectedImageRef.sprite = information.GetMyImage();
            statisticTextRef.text = information.GetMyStatistics();
            descriptiveTextRef.text = information.GetMyDescription();
            DisplayCost(information);
            ButtonControl(information);
            if (selected.GetComponent<Tower>() && !isEvolving) { evolutionMenu.SetButtons(selected); }
            lastSelected = selected;
        }

        private void DisplayType(UISelectionDescription information)
        {
            if(information.GetMyType() == EvoTypes.None)
            {
                selectedTypeRef.text = "Building";
            }
            else
            {
                selectedTypeRef.text = information.GetMyType().ToString();
            }
        }

        private void BuildButtonDisplayControl(UISelectionDescription information)
        {
            if (information.GetActionType() == actionTypes.Build)
            {
                buildButton.onClick.AddListener(Build);
                buildingToBuild = information.GetBuildingPrefab();
                BuildingSpawner spawner = FindObjectOfType<BuildingSpawner>();
            }
            if (information.GetActionType() == actionTypes.Spawn) { buildButton.onClick.AddListener(Spawn); }
        }

        private void Build()
        {
            FindObjectOfType<BuildingSpawner>().DisplayBuildingToSpawn(buildingToBuild.GetComponent<Building>());
        }

        private void Spawn()
        {
            FindObjectOfType<UISliderControl>().ToggleMenu(spawnMenu);
        }

        private void SetEnableSelectionUI(bool status)
        {
            gameObject.SetActive(status);
        }

        private void DisplayCost(UISelectionDescription information)
        {
            if(information.GetActionType() == actionTypes.Build)
            {
                costTextRef.text = $"Build \nCost - {information.GetBuildCost()}";
            }
            else if(information.GetActionType() == actionTypes.Spawn)
            {
                costTextRef.text = $"Spawn";
            }
            else
            {
                costTextRef.text = $"Build";
            }
        }

        private void EvolveButtonDisplayControl(UISelectionDescription information)
        {
            evolveButton.onClick.RemoveAllListeners();
            if(isEvolving)
            {
                Tower newEvo = information.GetComponent<Tower>();
                evolveButton.onClick.AddListener(delegate { towerEvolving.Evolve(newEvo); });
            }
        }

        private void ButtonControl(UISelectionDescription information)
        {
            actionTypes action = information.GetActionType();
            if(action == actionTypes.Build || action == actionTypes.Spawn)
            {
                SetButtonColorAndActiveStatus(buildButton, true);
                SetButtonColorAndActiveStatus(evolveButton, false);
                SetButtonColorAndActiveStatus(researchButton, false);
                BuildButtonDisplayControl(information);
            }
            if (action == actionTypes.Evolve)
            {
                SetButtonColorAndActiveStatus(buildButton, false);
                SetButtonColorAndActiveStatus(evolveButton, true);
                SetButtonColorAndActiveStatus(researchButton, false);
                EvolveButtonDisplayControl(information);
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
