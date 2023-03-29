using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float yValue = 15;

        void LateUpdate()
        {
            Vector3 position = new Vector3(target.position.x, yValue, target.position.z);
            transform.position = position;
        }
    }   
}
