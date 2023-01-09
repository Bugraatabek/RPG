using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Globes
{
    [CreateAssetMenu(menuName =("RPG/Globes/GlobeLibrary"))]
    public class GlobeDropLibrary : ScriptableObject
    {
        [SerializeField] int maxDrop;
        [SerializeField] int minDrop;
        [SerializeField] GlobeDrop[] drops;

            [System.Serializable]
            public class GlobeDrop
            {
                public Globe globe = null;
                public float relativeChance;
            }

            public struct ReadyToDrop
            {
                public Globe globe;
            }    
          
        public IEnumerable<ReadyToDrop> GlobesToDrop(int dropChance)
        {
            if(!ShouldDrop(dropChance)) yield break;
            for (int i = 0; i < GetNumberToDrop(); i++)
            {
                yield return GetRandomDrop();
            }
        }

        private bool ShouldDrop(int dropChance)
        {
            
            if(dropChance >= UnityEngine.Random.Range(0,100))
            {
                return true;
            }
            return false;
        }

        private float GetNumberToDrop()
        {
            return UnityEngine.Random.Range(minDrop, maxDrop);
        }

        private ReadyToDrop GetRandomDrop()
        {
            var drop = SelectRandomGlobe();
            var result = new ReadyToDrop();
            result.globe = drop.globe;
            return result;
        }

        private GlobeDrop SelectRandomGlobe()
        {
            var totalChance = TotalChance();
            float randomRoll = UnityEngine.Random.Range(0, totalChance);
            float cumulativeChance = 0;
            foreach(GlobeDrop drop in drops)
            {
                cumulativeChance += drop.relativeChance; 
                if(cumulativeChance >= randomRoll)
                {
                    return drop;
                }
            }
            return null;
        }

        private float TotalChance()
        {
            float totalChance = 0;
            foreach (var globe in drops)
            {
                totalChance += globe.relativeChance;
            }
            return totalChance; 
        }
    }
}
