using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETD.PlayerControl
{
    public class GoldController : MonoBehaviour
    {
        [SerializeField] Text goldTextRef = null;
        [SerializeField] int startingGold = 100;
        [SerializeField] int currentGold;

        public int GetCurrentGold()
        {
            return currentGold;
        }

        private void Start()
        {
            currentGold = startingGold;
            UpdateGoldText();
        }

        public void SpendGold(int goldUsed)
        {
            currentGold -= goldUsed;
            UpdateGoldText();
        }

        public void GainGold(int goldGained)
        {
            currentGold += goldGained;
            UpdateGoldText();
        }

        private void UpdateGoldText()
        {
            goldTextRef.text = $"Gold  : {currentGold}";
        }


    }
}
