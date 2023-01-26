using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Utils;

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
            currentNode = GetFirstAvailableRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        private DialogueNode GetFirstAvailableRootNode()
        {
            List<DialogueNode> rootNodes = currentDialogue.GetRootNodes() as List<DialogueNode>;

            for (int i = 0; i < rootNodes.Count(); i++)
            {
                if(rootNodes[i].CheckCondition(GetEvaluators()) == true)
                {
                    return rootNodes[i];
                }
            }
            return currentDialogue.GetRootNode();
        }

        public void Next()
        {
            int numberOfPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if(numberOfPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;   
            }
            var children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
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

        private void TriggerAction(ETrigger action)
        {
            if(action == ETrigger.Select) return;

            foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        //Getters//

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
            var childeren = FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).ToArray();
            if (childeren.Length <= 0)
            {
                return false;
            }
            return true;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private List<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>().ToList<IPredicateEvaluator>();
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
            foreach (var node in FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)))
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

        //Getters//
    }
}
