using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.PlayerControl
{
    public class GoldController : MonoBehaviour
    {
        [SerializeField] int startingGold = 100;
        [SerializeField] int currentGold;

        public int GetCurrentGold()
        {
            return currentGold;
        }

        private void Start()
        {
            currentGold = startingGold;
        }

        public void SpendGold(int goldUsed)
        {
            currentGold -= goldUsed;
        }

        public void GainGold(int goldGained)
        {
            currentGold += goldGained;
        }



    }
}
