using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETD.SelectionControl
{
    public class SelectionHandler : MonoBehaviour
    {
        UISelectionDescription currentSelection = null;
        UISelectionSection selectionSection = null;
        bool mouseOverUI = false;
        bool isBuilding = false;

        public void SetIsBuilding(bool status)
        {
            isBuilding = status;
        }

        public void SetSelectionSection(UISelectionSection newSelectionSection)
        {
            selectionSection = newSelectionSection;
        }

        private void Update()
        {
            mouseOverUI = EventSystem.current.IsPointerOverGameObject();
            MakeSelection();
        }

        private void MakeSelection()
        {
            if (mouseOverUI || isBuilding) { return; }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(!hit.collider.gameObject.GetComponent<UISelectionDescription>())
                    {
                        currentSelection = null;
                        selectionSection.SetSelected(currentSelection);
                        return;
                    }
                    currentSelection = hit.collider.gameObject.GetComponent<UISelectionDescription>();
                    selectionSection.SetSelected(currentSelection);
                }
            }
        }

    }
}
