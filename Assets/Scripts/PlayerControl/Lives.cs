using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETD.PlayerControl
{
    public class Lives : MonoBehaviour
    {
        [SerializeField] Text livesTextRef = null;
        [SerializeField] int startingLives = 5;
        [SerializeField] int currentLives;

        private void Start()
        {
            currentLives = startingLives;
            UpdateLivesText();
        }

        public void LoseLife()
        {
            currentLives--;
            UpdateLivesText();
            if(currentLives <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            Debug.Log("You Lost!");
            //this method will probably be in another script.
        }

        private void UpdateLivesText()
        {
            livesTextRef.text = $"Lives : {currentLives}";
        }



    }
}
