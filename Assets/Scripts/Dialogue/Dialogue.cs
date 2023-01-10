using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        
#if UNITY_EDITOR
        private void Awake() 
        {
            if(nodes.Count == 0)
            {
                var defaultNode = new DialogueNode();
                nodes.Add(defaultNode);
            }
        }
#endif
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }    
    }
}
