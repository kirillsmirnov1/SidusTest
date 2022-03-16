using UnityEngine;

namespace SidusTest.Control
{
    public class BezierPath : MonoBehaviour
    {
        [SerializeField] private Vector2 bezierRadius = new Vector2(1, 3);

        private readonly Vector3[] _bezierPoints = new Vector3[3];

        private static BezierPath _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static void PreparePath(Vector3 from, Vector3 to)
            => _instance.PreparePathImpl(from, to);

        private void PreparePathImpl(Vector3 from, Vector3 to)
        {
            var continueCurve = Vector3.Distance(from, _bezierPoints[2]) < float.Epsilon;
            _bezierPoints[0] = from;
            _bezierPoints[1] = continueCurve ? MirrorPoint(_bezierPoints[1], from) : ShiftPoint((from + to) / 2);
            _bezierPoints[2] = to;
        }

        private Vector3 MirrorPoint(Vector3 previous, Vector3 current)
        {
            var direction = (current - previous).normalized;
            var next = current + direction * RandomMultiplier();
            return next;
        }

        private Vector3 ShiftPoint(Vector3 middle1) 
            => RandomMultiplier() * Random.onUnitSphere + middle1;

        private float RandomMultiplier() 
            => Random.Range(bezierRadius.x, bezierRadius.y);

        public static Vector3 PositionAt(float t)
            => _instance.PositionAtImpl(t);

        private Vector3 PositionAtImpl(float t)
        {
            var u = 1 - t;
            var uu = u * u;
            var tt = t * t;

            var p = uu * _bezierPoints[0]
                    + 2 * u * t * _bezierPoints[1]
                    + tt * _bezierPoints[2];

            return p;
        }
    }
}
