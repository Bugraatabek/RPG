using System;
using System.Collections;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable 
    {
        LazyValue<float> mana;
        BaseStats baseStats;
        [SerializeField] UnityEvent noMana;


        private void Awake() 
        {
            baseStats = GetComponent<BaseStats>();
            mana = new LazyValue<float>(GetMaxMana);
        }

        private void Start() 
        {
            mana.ForceInit();
        }

        private void Update() 
        {
            StartCoroutine(RegenerateMana());
        }

        public float GetCurrentMana()
        {
            return mana.value;
        }

        public float GetMaxMana()
        {
            return baseStats.GetStat(Stat.Mana);
        }

        public float GetRegenRate()
        {
            return baseStats.GetStat(Stat.ManaRegenRate);
        }
        
        public float GetFraction()
        {
            return (mana.value / GetMaxMana());
        }

        public bool UseMana(float manaToUse)
        {
            if(manaToUse > mana.value)
            {
                noMana.Invoke();
                return false;
            }
            mana.value -= manaToUse;
           
            return true;
        }

        public void ChangeMana(float manaChange)
        {
            mana.value += manaChange;
        }

        public IEnumerator RegenerateMana()
        {
            if(mana.value < GetMaxMana())
            {
                mana.value += Time.deltaTime * GetRegenRate();
                yield return null;
            }
            if (mana.value >= GetMaxMana())
            {
                mana.value = GetMaxMana();
                yield return null;
            }
        }

        public object CaptureState()
        {
            return mana.value;
        }

        public void RestoreState(object state)
        {
            mana.value = (float)state;
        }

        
    }
}