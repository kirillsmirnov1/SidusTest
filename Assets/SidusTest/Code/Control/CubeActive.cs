using UnityEngine;

namespace SidusTest.Control
{
    public class CubeActive : MonoBehaviour
    {
        [SerializeField] private float speed = 5;

        private Vector3 _defaultPos;

        private Quaternion _defaultRotation;
        private Quaternion _targetRotation;
        private Quaternion _lastRotation;

        private float _t = 1f;

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
            _defaultPos = transform.position;
        }

        private void SetTarget(Vector3 targetPos, Quaternion targetRotation)
        {
            _lastRotation = transform.rotation;
            _targetRotation = targetRotation;

            Path.PreparePath(transform.position, targetPos);
            
            _t = 0f;
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
            if(_t >= 1f) return;
            
            _t = Mathf.Clamp01(_t + speed * Time.deltaTime);

            transform.position = Path.PositionAt(_t);
            transform.rotation = Quaternion.Slerp(_lastRotation, _targetRotation, _t);
        }
    }
}
