using System.Collections;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable 
    {
        LazyValue<float> mana;
        BaseStats baseStats;

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

        public bool UseMana(float manaToUse)
        {
            if(manaToUse > mana.value)
            {
                return false;
            }
            mana.value -= manaToUse;
           
            return true;
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