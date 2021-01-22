using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETD.UIControl
{
    public class UIMenuSection : MonoBehaviour
    {
        [Header("MenuButtons")]
        [SerializeField] Button pauseButton = null;

        public bool isPaused = false;
        List<Button> alreadyDisabledButtons = new List<Button>();

        #region ButtonControl

        public void TogglePause()
        {
            if (!isPaused) 
            {
                Time.timeScale = 0f; 
                isPaused = true; 
                pauseButton.GetComponentInChildren<Text>().text = "Resume";
                SetAllButtons(false);
                return; 
            }
            else if (isPaused) 
            {
                Time.timeScale = 1f; 
                isPaused = false;
                pauseButton.GetComponentInChildren<Text>().text = "Pause";
                SetAllButtons(true);
                return; 
            }
        }

        private void SetAllButtons(bool state)
        {
            var buttons = FindObjectsOfType<Button>();
            foreach(Button button in buttons)
            {
                if(button.CompareTag("DoNotDisable")) { continue; }
                if(state == false)
                {
                    if(button.interactable == false) { alreadyDisabledButtons.Add(button); }
                }
                if(state == true)
                {
                    if(alreadyDisabledButtons.Contains(button)) { alreadyDisabledButtons.Remove(button); continue; }
                }
                button.interactable = state;
            }
        }

        #endregion

    }
}
