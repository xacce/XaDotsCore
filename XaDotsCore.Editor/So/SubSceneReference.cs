using Unity.Entities.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace GridWorld.Authoring
{
    [CreateAssetMenu(menuName = "Xadots/SubScene/Create reference")]
    public class SubSceneReference : ScriptableObject
    {
        [SerializeField] private SceneAsset sceneAsset_s;
        
        public EntitySceneReference AsReference() => new EntitySceneReference(sceneAsset_s);
    }
}