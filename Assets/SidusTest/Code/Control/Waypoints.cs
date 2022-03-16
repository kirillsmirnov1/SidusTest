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

        private Transform NextWaypointImpl(Vector3 from, Transform target)
        {
            var targetPos = target.position;
            var direction = (targetPos - from).normalized;
            var minDistance = Vector3.Distance(from, targetPos);
            
            for (int i = 0; i < waypoints.Length; i++)
            {
                var waypoint = waypoints[i];
                var waypointPos = waypoint.position;
                var toWaypoint = waypointPos - from;
                var dirToWaypoint = toWaypoint.normalized;
                
                if(TooClose(from, waypointPos) || NotAligned(dirToWaypoint, direction)) continue;
                
                var distToWaypoint = toWaypoint.magnitude;
                if(distToWaypoint > minDistance) continue;

                minDistance = distToWaypoint;
                target = waypoint;
            }

            return target;
        }

        private static bool NotAligned(Vector3 dirToWaypoint, Vector3 direction) 
            => Vector3.Dot(dirToWaypoint, direction) <= 0;

        private static bool TooClose(Vector3 @from, Vector3 pos) 
            => Vector3.Distance(@from, pos) < .1f;
    }
}