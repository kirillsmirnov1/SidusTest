using System;
using UnityEngine;

namespace SidusTest.Control
{
    public class CubePassive : MonoBehaviour
    {
        public static event Action<Transform> OnClick; 
        
        private void OnMouseDown() 
            => OnClick?.Invoke(transform);
    }
}
