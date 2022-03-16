using UnityEngine;

namespace SidusTest.Control
{
    public class CubeActive : MonoBehaviour
    {
        [SerializeField] private float speed = 5;

        private Vector3 _defaultPos;

        private Transform _target;
        
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
            _target = transform;
            _defaultRotation = _targetRotation = transform.rotation;
            _defaultPos = transform.position;
        }

        private void SetTarget(Transform target)
        {
            _target = target;
            var nextWaypoint = Waypoints.NextWaypoint(transform.position, _target);
            SetWaypoint(nextWaypoint.position, nextWaypoint.rotation);
        }
        
        private void SetWaypoint(Vector3 targetPos, Quaternion targetRotation)
        {
            _lastRotation = transform.rotation;
            _targetRotation = targetRotation;

            BezierPath.PreparePath(transform.position, targetPos);
            
            _t = 0f;
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _target = transform;
                SetWaypoint(_defaultPos, _defaultRotation);
            }
        }

        private void Move()
        {
            if (_t >= 1f)
            {
                if(ReachedTarget()) return;
                SetTarget(_target);
            }
            
            _t = Mathf.Clamp01(_t + speed * Time.deltaTime);

            transform.position = BezierPath.PositionAt(_t);
            transform.rotation = Quaternion.Slerp(_lastRotation, _targetRotation, _t);
        }

        private bool ReachedTarget() 
            => Vector3.Distance(transform.position, _target.position) < float.Epsilon;
    }
}
