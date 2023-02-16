using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.HUD
{
    public class DisplayLevel : MonoBehaviour
    {
        private void Update()
        {
            int level = GameObject.FindWithTag("Player").GetComponent<BaseStats>().CalculateLevel();
            GetComponent<Text>().text = level.ToString();
        }
    }
}
