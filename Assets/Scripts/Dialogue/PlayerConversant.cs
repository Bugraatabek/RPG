using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode;

        private void Awake() 
        {
            currentNode = currentDialogue.GetRootNode();
        }

        public string GetText()
        {
            if(currentNode == null) return "";
            else
            {
                return currentNode.GetText();
            }
        }

        public void Next()
        {
            var childeren = currentDialogue.GetAllChildren(currentNode).ToArray();
            int randomIndex = Random.Range(0, childeren.Length);
            currentNode = childeren[randomIndex];   
        }

        public void Quit()
        {

        }

        public bool HasNext()
        {
            var childeren = currentDialogue.GetAllChildren(currentNode).ToArray();
            if (childeren.Length <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
