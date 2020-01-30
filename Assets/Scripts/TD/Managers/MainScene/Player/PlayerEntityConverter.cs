using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TD.Managers.MainScene.Player
{
    public class PlayerEntityConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float startYRotation = -90;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            RotationEulerXYZ rotationEuler = new RotationEulerXYZ()
            {
                Value = new float3(0, math.radians(startYRotation), 0)
            };
            dstManager.AddComponentData(entity, rotationEuler);
        }
    }
}