using ETD.PlayerControl;
using ETD.TowerControl;
using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEvolutionMenu : MonoBehaviour
{
    [SerializeField] Button[] EvoButtons;
    [SerializeField] UISelectionSection selectionSection = null;

    GoldController bank;

    private void Start()
    {
        bank = FindObjectOfType<GoldController>();
    }

    public void SetButtons(UISelectionDescription selected)
    {
        Tower[] evolutions = selected.GetPossibleEvolutions();
        foreach(Button button in EvoButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
        for(int i = 0; i < evolutions.Length; i++)
        {
            Button button = EvoButtons[i];
            button.gameObject.SetActive(true);
            Tower tower = selected.GetComponent<Tower>();
            var myEvoNumber = evolutions[i];
            button.onClick.AddListener
                (delegate { selectionSection.SetSelected(myEvoNumber.GetComponent<UISelectionDescription>(), true); });
            button.image.sprite = evolutions[i].GetComponent<UISelectionDescription>().GetMyImage();
            button.transform.Find("Name Text").GetComponent<Text>().text = 
                evolutions[i].GetComponent<UISelectionDescription>().GetMyName();
            button.transform.Find("Cost Text Number").GetComponent<Text>().text =
                evolutions[i].GetComponent<UISelectionDescription>().GetBuildCost().ToString();
        }
    }

    private void Update()
    {
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        foreach(Button button in EvoButtons)
        {
            if(button.gameObject.activeInHierarchy)
            {
                int cost = int.Parse(button.transform.Find("Cost Text Number").GetComponent<Text>().text);
                if (cost <= bank.GetCurrentGold())
                {
                    button.interactable = true;
                }
                else { button.interactable = false; }
            }
        }
    }
}
