using UnityEngine;

namespace SidusTest.Control
{
    public class CubePassive : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Debug.Log($"{gameObject.name} got clicked");
        }
    }
}
