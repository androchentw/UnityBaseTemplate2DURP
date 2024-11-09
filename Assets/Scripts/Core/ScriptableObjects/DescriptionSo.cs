using JetBrains.Annotations;
using UnityEngine;
namespace Core.ScriptableObjects
{
    /// <summary>
    /// This is a base ScriptableObject that adds a description field.
    /// </summary>
    [CreateAssetMenu(fileName = "DescriptionSo", menuName = "Scriptable Objects/DescriptionSo")]
    public class DescriptionSo : ScriptableObject
    {
        [TextArea(5, 20)]
        [SerializeField] [CanBeNull]
        protected string description;
    }
}
