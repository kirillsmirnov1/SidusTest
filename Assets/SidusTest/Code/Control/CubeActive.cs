using UnityEngine;

namespace SidusTest.Control
{
    public class CubeActive : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 100;
        [SerializeField] private float rotationSpeed = 100;
        
        private Vector3 _defaultPos;
        private Vector3 _targetPos;

        private Quaternion _defaultRotation;
        private Quaternion _targetRotation;
        
        private void Awake()
        {
            InitFields();
            CubePassive.OnClick += SetTarget;
        }

        private void OnDestroy() 
            => CubePassive.OnClick -= SetTarget;

        private void Update()
        {
            HandleInput();
            Move();
        }

        private void InitFields()
        {
            _defaultRotation = _targetRotation = transform.rotation;
            _defaultPos = _targetPos = transform.position;
        }

        private void SetTarget(Vector3 targetPos, Quaternion targetRotation)
        {
            _targetPos = targetPos;
            _targetRotation = targetRotation;
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetTarget(_defaultPos, _defaultRotation);
            }
        }

        private void Move()
        {
            if (Vector3.Distance(transform.position, _targetPos) > float.Epsilon)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPos, movementSpeed * Time.deltaTime);
            }

            if (Quaternion.Angle(transform.rotation, _targetRotation) > float.Epsilon)
            {
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
