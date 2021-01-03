using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace ETD.TowerControl
{
    public class TowerSpawner : MonoBehaviour
    {
        [SerializeField] GameObject ground = null;
        [SerializeField] Material buildPreviewMaterial = null;
        [SerializeField] Mover pathTester = null;

        Tower lastTowerDisplay = null;
        Tower towerDisplay = null;
        Color green;
        Color red;
        int groundLayerMask;
        Material originalMaterial;
        bool isBuildable = true;
        bool isBuildingMultiple = false;

        private void Start()
        {
            green = new Color(0f, 200f, 0f, buildPreviewMaterial.color.a);
            red = new Color(200f, 0f, 0f, buildPreviewMaterial.color.a);
            groundLayerMask = 1 << ground.layer;
        }

        private void Update()
        {
            UpdateTowerDisplay();
        }

        public void ToggleColorBuildIndicator(bool ableToBuild)
        {
            isBuildable = ableToBuild;
            if(ableToBuild) { buildPreviewMaterial.color = green; }
            else { buildPreviewMaterial.color = red; }
        }

        public void DisplayTowerToSpawn(Tower towerPrefab)
        {
            lastTowerDisplay = towerPrefab;
            towerDisplay = Instantiate(towerPrefab);
            towerDisplay.SetSpawner(this);
            var meshRenderer = towerDisplay.GetComponentInChildren<MeshRenderer>();
            originalMaterial = meshRenderer.material;
            meshRenderer.material = buildPreviewMaterial;
            towerDisplay.GetComponent<NavMeshObstacle>().enabled = false;
        }

        private void UpdateTowerDisplay()
        {
            if (towerDisplay == null) { return; }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask)) { return; }
            towerDisplay.transform.position = hit.point;
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

        IEnumerator BuildTower()
        {
            towerDisplay.GetComponent<NavMeshObstacle>().enabled = true;
            yield return new WaitForSeconds(Mathf.Epsilon);
            if(pathTester.IsPathBlocked()) 
            {
                towerDisplay.GetComponent<NavMeshObstacle>().enabled = false;
                StartCoroutine(FlashVisualIndicatorCantBuilt());
                Debug.Log("Can't build here. Blocking Path");
                yield break;
            }
            towerDisplay.GetComponentInChildren<MeshRenderer>().material = originalMaterial;
            towerDisplay.SetAsBuilt();
            towerDisplay.GetComponent<GridControl>().SetShowGrid(false);
            towerDisplay = null;
            if(isBuildingMultiple) 
            { 
                DisplayTowerToSpawn(lastTowerDisplay); 
                isBuildingMultiple = false; 
            }
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
                Destroy(towerDisplay.gameObject);
            }
        }

    }
}
