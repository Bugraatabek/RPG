using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] DialogueNode[] nodes;
    }
}
