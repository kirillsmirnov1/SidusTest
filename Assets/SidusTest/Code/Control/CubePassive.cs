using System;
using UnityEngine;

namespace SidusTest.Control
{
    public class CubePassive : MonoBehaviour
    {
        public static event Action<Vector3, Quaternion> OnClick; 
        
        private void OnMouseDown() 
            => OnClick?.Invoke(transform.position, transform.rotation);
    }
}
