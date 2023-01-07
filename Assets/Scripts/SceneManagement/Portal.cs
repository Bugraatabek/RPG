using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;



namespace RPG.Scenemanagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 3f;
        [SerializeField] float fadeInTime = 3f;
        [SerializeField] float waitTimeAfterSceneLoad = 0.5f;
        
        
        Fader fader;
        SavingWrapper savingWrapper;
        GameObject player;
        GameObject newPlayer;        


        
        

        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        [SerializeField] int sceneBuildIndex;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;


        private void OnTriggerEnter(Collider other) 
        {
           if(other.gameObject.tag == "Player")
           {
                StartCoroutine(Transation());
           }
        }

        private IEnumerator Transation()
        {
            player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;
            if (sceneBuildIndex < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            savingWrapper = FindObjectOfType<SavingWrapper>();
            fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            
            savingWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(sceneBuildIndex);
            newPlayer = GameObject.FindWithTag("Player");
            newPlayer.GetComponent<PlayerController>().enabled = false;
            
            
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.Save();
            
            yield return new WaitForSeconds(waitTimeAfterSceneLoad);
            fader.FadeIn(fadeInTime);

            newPlayer.GetComponent<PlayerController>().enabled = true;
            Destroy(gameObject);
            
            
            
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.transform.position = otherPortal.spawnPoint.position;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination == this.destination)
                {
                return portal;
                }
                  
            }
            return null;

        }
    }

}
