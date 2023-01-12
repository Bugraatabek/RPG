using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
        
#if UNITY_EDITOR
        private void Awake() 
        {
            
            if(nodes.Count == 0)
            {
                var defaultNode = new DialogueNode();
                nodes.Add(defaultNode);
            }
            OnValidate();
        }
#endif
        private void OnValidate() 
        {
            BuildLookupDict();    
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        private void BuildLookupDict()
        {
            foreach (DialogueNode node in nodes)
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent)
        {
            var childeren = parent.children;
            foreach (String child in childeren)
            {
                if(nodeLookup.ContainsKey(child))
                {
                    yield return nodeLookup[child];
                }
            }
        }
    }
}
