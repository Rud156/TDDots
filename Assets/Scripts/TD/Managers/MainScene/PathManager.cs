using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TD.Managers.MainScene
{
    public class PathManager : MonoBehaviour
    {
        public float pathPointVariation;
        public List<Transform> wayPoints;

        private List<Vector3> _wayPointPositions;

        #region Unity Functions

        private void Start()
        {
            _wayPointPositions = new List<Vector3>();
            CreateWayPoints();
        }

        #endregion

        #region External Functions

        public bool GetNextPointInPath(int pathIndex, out float3 nextPoint)
        {
            if (!IsValidWayPointIndex(pathIndex) || !IsValidWayPointIndex(pathIndex + 1))
            {
                nextPoint = Vector3.zero;
                return false;
            }

            nextPoint = _wayPointPositions[pathIndex + 1];
            return true;
        }

        public bool GetNextPointInPathWithVariation(int pathIndex, out float3 nextPoint)
        {
            if (!IsValidWayPointIndex(pathIndex) || !IsValidWayPointIndex(pathIndex + 1))
            {
                nextPoint = Vector3.zero;
                return false;
            }

            nextPoint = _wayPointPositions[pathIndex + 1];
            nextPoint += Random.Range(-pathPointVariation, pathPointVariation);
            return true;
        }

        public bool GetCurrentPointInPath(int pathIndex, out float3 currentPoint)
        {
            if (!IsValidWayPointIndex(pathIndex))
            {
                currentPoint = Vector3.zero;
                return false;
            }

            currentPoint = _wayPointPositions[pathIndex];
            return true;
        }

        public bool GetCurrentPointInPathWithVariation(int pathIndex, out float3 currentPoint)
        {
            if (!IsValidWayPointIndex(pathIndex))
            {
                currentPoint = Vector3.zero;
                return false;
            }

            currentPoint = _wayPointPositions[pathIndex];
            currentPoint += Random.Range(-pathPointVariation, pathPointVariation);
            return true;
        }

        public bool IsValidWayPointIndex(int pathIndex) => pathIndex >= 0 && pathIndex < _wayPointPositions.Count;

        #endregion

        #region Utility Functions

        private void CreateWayPoints()
        {
            for (int i = 0; i < wayPoints.Count; i++)
            {
                _wayPointPositions.Add(wayPoints[i].position);
            }
        }

        #endregion

        #region Singleton

        private static PathManager _instance;
        public static PathManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}