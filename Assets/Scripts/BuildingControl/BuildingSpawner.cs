using ETD.EnemyControl;
using ETD.PlayerControl;
using ETD.SelectionControl;
using ETD.TowerControl;
using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETD.BuildingControl
{
    public class BuildingSpawner : MonoBehaviour
    {
        [SerializeField] GameObject ground = null;
        [SerializeField] Material buildPreviewMaterial = null;
        [SerializeField] Mover pathTester = null;
        [SerializeField] Transform towerParent = null;
        [SerializeField] Transform projectileParent = null;

        Building lastBuildingDisplayed = null;
        Building buildingDisplay = null;
        Color green;
        Color red;
        int groundLayerMask;
        bool isBuildable = true;
        bool isBuildingMultiple = false;
        GoldController bank;

        private void Start()
        {
            green = new Color(0f, 200f, 0f, buildPreviewMaterial.color.a);
            red = new Color(200f, 0f, 0f, buildPreviewMaterial.color.a);
            groundLayerMask = 1 << ground.layer;
            bank = FindObjectOfType<GoldController>();
        }

        private void Update()
        {
            UpdateBuildingDisplay();
        }

        public void ToggleColorBuildIndicator(bool ableToBuild)
        {
            isBuildable = ableToBuild;
            if(ableToBuild) { buildPreviewMaterial.color = green; }
            else { buildPreviewMaterial.color = red; }
        }

        public void DisplayBuildingToSpawn(Building buildingPrefab)
        {
            if (buildingDisplay != null) { return; }
            FindObjectOfType<SelectionHandler>().SetIsBuilding(true);
            if (buildingPrefab == null) { Debug.LogError("buildingPrefab not assigned."); }
            lastBuildingDisplayed = buildingPrefab;
            buildingDisplay = Instantiate(buildingPrefab, towerParent);
            buildingDisplay.SetSpawner(this);
            buildingDisplay.GetComponent<NavMeshObstacle>().enabled = false;
            if (buildingDisplay.TryGetComponent(out Attacker attacker)) { attacker.enabled = false; }
        }

        private void UpdateBuildingDisplay()
        {
            if (buildingDisplay == null) { return; }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask)) { return; }
            buildingDisplay.transform.position = hit.point;
            CancelBuild();
            AttemptBuild();
        }

        private void AttemptBuild()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                isBuildingMultiple = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if(EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Can't build through UI.");
                    return;
                }
                if (!HaveEnoughGold()) 
                {
                    Debug.Log("Not enough gold.");
                    StartCoroutine(FlashVisualIndicatorCantBuilt());
                    return; 
                }
                if (isBuildable)
                {
                    StartCoroutine(BuildTower());
                }
                else
                {
                    Debug.Log("Can't build here.");
                    isBuildingMultiple = false;
                }
            }
        }

        private bool HaveEnoughGold()
        {
            if(bank.GetCurrentGold() >= buildingDisplay.GetCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator BuildTower()
        {
            buildingDisplay.GetComponent<NavMeshObstacle>().enabled = true;
            yield return new WaitForSeconds(Mathf.Epsilon);
            if(pathTester.IsPathBlocked()) 
            {
                buildingDisplay.GetComponent<NavMeshObstacle>().enabled = false;
                StartCoroutine(FlashVisualIndicatorCantBuilt());
                Debug.Log("Can't build here. Blocking Path");
                yield break;
            }
            buildingDisplay.SetAsBuilt();
            if (buildingDisplay.TryGetComponent(out Attacker attacker)) 
            { attacker.enabled = true; attacker.SetProjectileParent(projectileParent); }
            buildingDisplay.GetComponent<GridControl>().SetShowGrid(false);
            bank.SpendGold(buildingDisplay.GetCost());
            buildingDisplay = null;
            if(!lastBuildingDisplayed.GetComponent<Tower>())
            {
                FindObjectOfType<UISelectionSection>().SetSelected
                    (lastBuildingDisplayed.GetComponent<UISelectionDescription>(), false);
                FindObjectOfType<SelectionHandler>().SetIsBuilding(false);
                yield break;
            }
            if (isBuildingMultiple)
            { 
                DisplayBuildingToSpawn(lastBuildingDisplayed); 
                isBuildingMultiple = false;
                yield break;
            }
            FindObjectOfType<SelectionHandler>().SetIsBuilding(false);
            yield return null;
        }

        IEnumerator FlashVisualIndicatorCantBuilt()
        {
            buildPreviewMaterial.color = red;
            yield return new WaitForSeconds(.2f);
            buildPreviewMaterial.color = green;
        }

        private void CancelBuild()
        {
            if(Input.GetMouseButtonDown(1))
            {
                Destroy(buildingDisplay.gameObject);
                FindObjectOfType<SelectionHandler>().SetIsBuilding(false);
            }
        }

        public void EvolveSelectedTower(Tower oldTower, Tower newTower)
        {
            Vector3 spawnPos = oldTower.transform.position;
            Destroy(oldTower.gameObject);
            Tower evolvedTower = Instantiate(newTower, spawnPos, transform.rotation, towerParent);
            evolvedTower.GetComponent<Building>().SetAsBuilt();
            evolvedTower.GetComponent<Attacker>().SetProjectileParent(projectileParent);
            evolvedTower.GetComponent<GridControl>().SetShowGrid(false);
            FindObjectOfType<UISelectionSection>().SetSelected(evolvedTower.GetComponent<UISelectionDescription>(), false);
        }
    }
}
