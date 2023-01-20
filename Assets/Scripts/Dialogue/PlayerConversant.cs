using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName;
        
        Dialogue currentDialogue;
        DialogueNode currentNode;
        AIConversant currentConversant = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        public void Next()
        {
            int numberOfPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numberOfPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;   
            }
            var children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();   
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentConversant = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();             
        }

        private void TriggerEnterAction()
        {
            if(currentNode != null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if(currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(TriggerType action)
        {
            if(action == TriggerType.Empty) return;

            foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        //Getters

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public bool IsActive()
        {
            return currentDialogue != null;
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

        public string GetText()
        {
            if(currentNode == null) return "";
            else
            {
                return currentNode.GetText();
            }
        }
        
        public IEnumerable<DialogueNode> GetChoices()
        {
            foreach (var node in currentDialogue.GetPlayerChildren(currentNode))
            {
                yield return node;
            } 
        }

        public string GetCurrentConversantName()
        {
            if(isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetAIConversantName();
            }
            
        }
    }
}
