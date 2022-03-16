using UnityEngine;

namespace SidusTest.Control
{
    public class BezierPath : MonoBehaviour
    {
        [SerializeField] private Vector2 bezierRadius = new Vector2(1, 3);

        private readonly Vector3[] _bezierPoints = new Vector3[4];

        private static BezierPath _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static void PreparePath(Vector3 from, Vector3 to)
            => _instance.PreparePathImpl(from, to);

        private void PreparePathImpl(Vector3 from, Vector3 to)
        {
            var continueCurve = Vector3.Distance(from, _bezierPoints[3]) < float.Epsilon;
            _bezierPoints[0] = from;
            _bezierPoints[3] = to;
            var middlePoint = (from + to) / 2;
            var middle1 = (from + middlePoint) / 2;
            var middle2 = (to + middlePoint) / 2;
            _bezierPoints[1] = continueCurve ? MirrorPoint(_bezierPoints[2], from) : ShiftPoint(middle1);
            _bezierPoints[2] = ShiftPoint(middle2);
        }

        private Vector3 MirrorPoint(Vector3 previous, Vector3 current)
        {
            var next = 2 * current - previous;
            return next;
        }

        private Vector3 ShiftPoint(Vector3 middle1) 
            => Random.Range(bezierRadius.x, bezierRadius.y) * Random.onUnitSphere + middle1;

        public static Vector3 PositionAt(float t)
            => _instance.PositionAtImpl(t);

        private Vector3 PositionAtImpl(float t)
        {
            var u = 1 - t;
            var uu = u * u;
            var uuu = uu * u;
            var tt = t * t;
            var ttt = tt * t;

            var p = uuu * _bezierPoints[0]
                + 3 * uu * t * _bezierPoints[1]
                + 3 * u * tt * _bezierPoints[2]
                + ttt * _bezierPoints[3];

            return p;
        }
    }
}
