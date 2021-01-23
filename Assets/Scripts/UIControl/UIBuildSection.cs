using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildSection : MonoBehaviour
{
    [SerializeField] Button towerbutton = null;
    [SerializeField] Button researchCenterbutton = null;
    [SerializeField] Button upgradeCenterbutton = null;
    [SerializeField] Button enemyBreederbutton = null;
    [SerializeField] UISelectionSection selectionSection = null;

    private void Start()
    {
        towerbutton.onClick.AddListener
            (delegate { selectionSection.SetSelected(towerbutton.GetComponent<UISelectionDescription>(), false); });
        researchCenterbutton.onClick.AddListener
            (delegate { selectionSection.SetSelected(researchCenterbutton.GetComponent<UISelectionDescription>(), false); });
        upgradeCenterbutton.onClick.AddListener
            (delegate { selectionSection.SetSelected(upgradeCenterbutton.GetComponent<UISelectionDescription>(), false); });
        enemyBreederbutton.onClick.AddListener
            (delegate { selectionSection.SetSelected(enemyBreederbutton.GetComponent<UISelectionDescription>(), false); });
    }


}
