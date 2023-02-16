using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;
using RPG.Attributes;
using RPG.Core;
using RPG.Control;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "My Ability", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem 
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float cooldown = 0;
        [SerializeField] float manaCost = 0;
        CooldownStore cooldownStore;
        Mana userMana;
        
        public override void Use(GameObject user)
        {
            userMana = user.GetComponent<Mana>();
            if(userMana.GetCurrentMana() < manaCost) return;

            cooldownStore = user.GetComponent<CooldownStore>();
            if(cooldownStore.GetTimeRemaining(this) > 0) return;

            AbilityData data = new AbilityData(user);
            targetingStrategy.StartTargeting(data, () => {TargetAquired(data); });
        }

        private void TargetAquired(AbilityData data)
        {
            if(data.GetUser().GetComponent<Health>().IsDead()) return;
            if(!userMana.UseMana(manaCost)) return;
            if(data.IsCanceled()) return;
            
            cooldownStore.StartCooldown(this, cooldown);
            
            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
            
        }

        private void EffectFinished()
        {
            
        }
    }
}
