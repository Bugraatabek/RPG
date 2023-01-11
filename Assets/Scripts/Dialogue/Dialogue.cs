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
            // else
            // {
            //     for (int i = 1; i < nodes.Count; i++)
            //     {
            //         nodes[i].rect.y = nodes[i-1].rect.position.y + 70;
            //     }
            // }
        }
#endif
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }    
    }
}
