using System.Collections.Generic;
using UnityEngine;

namespace GameTerms
{
    [CreateAssetMenu(fileName = "GlossaryTerm", menuName = "Game Terms/Glossary Term")]
    public sealed class GlossaryTermAsset : ScriptableObject
    {
        [SerializeField] private GlossaryTermData data = new();

        public GlossaryTermData Data => data;

        public void SetData(GlossaryTermData source)
        {
            data = source.Clone();
        }
    }
}
