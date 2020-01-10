using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace TD.Managers.MainScene
{
    public class RockerTrailSyncManager : MonoBehaviour
    {
        public GameObject blueBullEffectPrefab;

        private struct TrailRendererHolder
        {
            public TrailRenderer trailRenderer;
            public Transform trailTransform;
            public GameObject effectGameObject;
            public bool isActive;
            public int elementIndex;
        }

        private List<TrailRendererHolder> _trailHolders;

        #region Unity Functions

        private void Start() => _trailHolders = new List<TrailRendererHolder>();

        #endregion

        #region External Functions

        public int GetTrailEffectIndex(float3 position)
        {
            TrailRendererHolder trailRendererHolder = GetOrCreateEmptyParticleHolder();
            trailRendererHolder.isActive = true;
            trailRendererHolder.trailTransform.position = position;
            trailRendererHolder.trailRenderer.Clear();
            trailRendererHolder.effectGameObject.SetActive(true);

            _trailHolders[trailRendererHolder.elementIndex] = trailRendererHolder;
            return trailRendererHolder.elementIndex;
        }

        public void UpdateTrailPosition(float3 position, int effectIndex)
        {
            TrailRendererHolder trailRendererHolder = _trailHolders[effectIndex];
            if (!trailRendererHolder.isActive)
            {
                Debug.Log("Invalid Effect Updated");
                return;
            }

            trailRendererHolder.trailTransform.position = position;
            _trailHolders[trailRendererHolder.elementIndex] = trailRendererHolder;
        }

        public void StopTrailEffect(int effectIndex)
        {
            TrailRendererHolder trailRendererHolder = _trailHolders[effectIndex];
            if (!trailRendererHolder.isActive)
            {
                Debug.Log("Effect Is Already InActive");
                return;
            }

            trailRendererHolder.isActive = false;
            trailRendererHolder.effectGameObject.SetActive(false);
            _trailHolders[trailRendererHolder.elementIndex] = trailRendererHolder;
        }

        #endregion

        #region Utility Functions

        private TrailRendererHolder GetOrCreateEmptyParticleHolder()
        {
            for (int i = 0; i < _trailHolders.Count; i++)
            {
                if (!_trailHolders[i].isActive)
                {
                    return _trailHolders[i];
                }
            }

            GameObject trailEffect = Instantiate(blueBullEffectPrefab, Vector3.zero, Quaternion.identity);
            TrailRendererHolder trailHolder = new TrailRendererHolder()
            {
                isActive = false,
                elementIndex = _trailHolders.Count,
                trailRenderer = trailEffect.GetComponent<TrailRenderer>(),
                trailTransform = trailEffect.transform,
                effectGameObject = trailEffect
            };

            _trailHolders.Add(trailHolder);
            return trailHolder;
        }

        #endregion

        #region Singleton

        private static RockerTrailSyncManager _instance;

        public static RockerTrailSyncManager Instance => _instance;

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