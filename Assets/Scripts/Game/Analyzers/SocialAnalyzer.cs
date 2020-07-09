using UnityEngine;
using Models.Info;

namespace Game.Analyzers
{
    public class SocialAnalyzer: Analyzer<SocialInfo>
    {
        /// <summary>
        /// Анализирует успешность социального действия
        /// </summary>
        public override void Analyze(SocialInfo social)
        {
            social.HypeIncome = Random.Range(1, 100);
        }
    }
}