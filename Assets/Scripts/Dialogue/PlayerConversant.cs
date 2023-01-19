using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue testDialogue;
        Dialogue currentDialogue;
        DialogueNode currentNode;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        // private void Awake() 
        // {
        //     currentNode = currentDialogue.GetRootNode();
        // }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            if(testDialogue != null)
            {
                StartDialogue(testDialogue);
            }
            
        }

        public string GetText()
        {
            if(currentNode == null) return "";
            else
            {
                return currentNode.GetText();
            }
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            isChoosing = false;
            currentNode = chosenNode;
            Next();             
        }

        public void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            onConversationUpdated();

        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public void Next()
        {
            int numberOfPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numberOfPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;   
            }
            var children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            currentNode = children[randomIndex];
            onConversationUpdated();   
        }

        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            foreach (var node in currentDialogue.GetPlayerChildren(currentNode))
            {
                yield return node;
            } 
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

        public bool IsChoosing()
        {
            return isChoosing;
        }
    }
}
