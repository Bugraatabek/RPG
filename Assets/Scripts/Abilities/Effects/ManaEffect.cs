using System;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Mana Effect", menuName = "Abilities/Effects/Mana", order = 0)]
    public class ManaEffect : EffectStrategy
    {
        [SerializeField] float manaChange = 0;
        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var mana = target.GetComponent<Mana>();
                if(mana)
                {
                    if(manaChange < 0)
                    {
                        mana.ChangeMana(-manaChange);
                    }
                    else
                    {
                        mana.ChangeMana(manaChange);
                    }
                }
                finished();
            }
        }
    }
}
