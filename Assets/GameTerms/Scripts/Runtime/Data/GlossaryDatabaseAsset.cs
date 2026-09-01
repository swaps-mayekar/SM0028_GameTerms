using System.Collections.Generic;
using UnityEngine;

namespace GameTerms
{
    [CreateAssetMenu(fileName = "GlossaryDatabase", menuName = "Game Terms/Glossary Database")]
    public sealed class GlossaryDatabaseAsset : ScriptableObject
    {
        [SerializeField] private List<GlossaryTermAsset> terms = new();

        public IReadOnlyList<GlossaryTermAsset> Terms => terms;

        public void SetTerms(IEnumerable<GlossaryTermAsset> source)
        {
            terms = new List<GlossaryTermAsset>(source);
        }
    }
}
