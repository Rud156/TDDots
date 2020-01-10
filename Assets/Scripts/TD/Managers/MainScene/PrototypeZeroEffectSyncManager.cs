using System.Collections.Generic;
using UnityEngine;

namespace TD.Managers.MainScene
{
    public class PrototypeZeroEffectSyncManager : MonoBehaviour
    {
        public GameObject prototypeZeroEffectPrefab;
        public Vector3 positionSyncOffset;

        private struct ParticleHolder
        {
            public ParticleSystem particleSystem;
            public Transform particleTransform;
            public bool isActive;
            public int elementIndex;
        }

        private List<ParticleHolder> _particleHolders;

        #region Unity Functions

        private void Start() => _particleHolders = new List<ParticleHolder>();

        #endregion

        #region External Functions

        public int GetEffectParticleIndex(Vector3 position)
        {
            ParticleHolder particleHolder = GetOrCreateEmptyParticleHolder();
            particleHolder.isActive = true;
            particleHolder.particleTransform.position = position + positionSyncOffset;
            particleHolder.particleSystem.Play();

            _particleHolders[particleHolder.elementIndex] = particleHolder;
            return particleHolder.elementIndex;
        }

        public void UpdateEffectPosition(Vector3 position, int effectIndex)
        {
            ParticleHolder particleHolder = _particleHolders[effectIndex];
            if (!particleHolder.isActive)
            {
                Debug.Log("Invalid Effect Updated");
                return;
            }

            particleHolder.particleTransform.position = position + positionSyncOffset;
            _particleHolders[particleHolder.elementIndex] = particleHolder;
        }

        public void StopParticleEffect(int effectIndex)
        {
            ParticleHolder particleHolder = _particleHolders[effectIndex];
            if (!particleHolder.isActive)
            {
                Debug.Log("Effect Is Already InActive");
                return;
            }

            particleHolder.isActive = false;
            particleHolder.particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _particleHolders[particleHolder.elementIndex] = particleHolder;
        }

        #endregion

        #region Utility Functions

        private ParticleHolder GetOrCreateEmptyParticleHolder()
        {
            for (int i = 0; i < _particleHolders.Count; i++)
            {
                if (!_particleHolders[i].isActive)
                {
                    return _particleHolders[i];
                }
            }

            GameObject particleEffect = Instantiate(prototypeZeroEffectPrefab, Vector3.zero, Quaternion.identity);
            ParticleHolder particleHolder = new ParticleHolder()
            {
                isActive = false,
                elementIndex = _particleHolders.Count,
                particleSystem = particleEffect.GetComponent<ParticleSystem>(),
                particleTransform = particleEffect.transform
            };

            _particleHolders.Add(particleHolder);
            return particleHolder;
        }

        #endregion

        #region Singleton

        private static PrototypeZeroEffectSyncManager _instance;

        public static PrototypeZeroEffectSyncManager Instance => _instance;

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