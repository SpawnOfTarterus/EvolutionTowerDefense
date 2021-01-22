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

    public void SelectBuildingInstead()
    {
        Debug.Log("this was called.");
        researchCenterbutton.onClick.RemoveAllListeners();
    }




}
