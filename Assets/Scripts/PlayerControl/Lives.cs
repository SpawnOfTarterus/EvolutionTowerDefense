using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.PlayerControl
{
    public class Lives : MonoBehaviour
    {
        [SerializeField] int startingLives = 5;
        [SerializeField] int currentLives;

        private void Start()
        {
            currentLives = startingLives;
        }

        public void LoseLife()
        {
            currentLives--;
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




    }
}
