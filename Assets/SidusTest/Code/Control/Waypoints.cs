using System.Linq;
using UnityEngine;

namespace SidusTest.Control
{
    public class Waypoints : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;

        private static Waypoints _instance;

        private void Awake()
        {
            _instance = this;
        }

        private void OnValidate()
        {
            if(Application.isPlaying) return;
            
            waypoints = FindObjectsOfType<CubePassive>()
                .Select(cube => cube.transform)
                .ToArray();
        }

        public static Transform NextWaypoint(Vector3 from, Transform to)
            => _instance.NextWaypointImpl(from, to);

        private Transform NextWaypointImpl(Vector3 from, Transform to)
        {
            var direction = (to.position - from).normalized;
            var nearestPoints = waypoints
                .OrderBy(p => Vector3.Distance(from, p.position));
            
            foreach (var nearestPoint in nearestPoints)
            {
                var waypoint = nearestPoint.position;
                var dirToWaypoint = (waypoint - from).normalized;
                if(TooClose(@from, waypoint) || NotAligned(dirToWaypoint, direction)) continue;
                return nearestPoint;
            }

            return to;
        }

        private static bool NotAligned(Vector3 dirToWaypoint, Vector3 direction) 
            => Vector3.Dot(dirToWaypoint, direction) <= 0;

        private static bool TooClose(Vector3 @from, Vector3 pos) 
            => Vector3.Distance(@from, pos) < .1f;
    }
}