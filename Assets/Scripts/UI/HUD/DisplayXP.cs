using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.HUD
{
    public class DisplayXP : MonoBehaviour
    {
        Experience experience;
        private void Awake() 
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();    
        }
        private void Update()
        {
            GetComponent<Text>().text = experience.GetXP().ToString(); 
        }
    }
}
