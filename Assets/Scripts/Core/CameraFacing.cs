using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        
        void LateUpdate() // calling this on LateUpdate instead of Update because camera position is being updating on other updates and this might cause glitches.
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
