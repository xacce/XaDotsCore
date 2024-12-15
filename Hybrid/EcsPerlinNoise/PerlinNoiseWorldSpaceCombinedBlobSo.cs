#if UNITY_EDITOR
using System;
using Core.Runtime.PerlinNoise;
using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid.Hybrid.PerlinNoise
{
    [CreateAssetMenu(menuName = "XaDotsCore/PerlinNoise/Create world space noise asset", fileName = "[XaDotsCore] Perlin noise asset")]
    public class PerlinNoiseWorldSpaceCombinedBlobSo : BakedScriptableObject<Runtime.PerlinNoise.PerlinNoiseWorldSpaceCombinedBlob>
    {
        [SerializeField] public PerlinNoise3dParams[] positions = Array.Empty<PerlinNoise3dParams>();
        [SerializeField] public PerlinNoise3dParams[] directions = Array.Empty<PerlinNoise3dParams>();


        public override void Bake(ref Runtime.PerlinNoise.PerlinNoiseWorldSpaceCombinedBlob data, ref BlobBuilder blobBuilder)
        {
            var posBaked = blobBuilder.Allocate(ref data.positions, positions.Length);
            var dirBaked = blobBuilder.Allocate(ref data.directions, directions.Length);
            for (int i = 0; i < positions.Length; i++)
            {
                posBaked[i] = positions[i];
            }

            for (int i = 0; i < directions.Length; i++)
            {
                dirBaked[i] = directions[i];
            }
        }
    }
}
#endif