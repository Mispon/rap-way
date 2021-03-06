using System;
using Core;
using Models.Info.Production;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Analyzers
{
    /// <summary>
    /// Анализатор клипа
    /// </summary>
    public class ClipAnalyzer : Analyzer<ClipInfo>
    {
        /// <summary>
        /// Анализирует успешность клипа
        /// </summary>
        public override void Analyze(ClipInfo clip)
        {
            var track = ProductionManager.GetTrack(clip.TrackId);

            float listenImpact = settings.ClipTrackListensImpact;
            float trackListenFactor = Mathf.Min(listenImpact * GetListenRatio(track.ListenAmount), listenImpact);
            float clipQuality = trackListenFactor + CalculateWorkPointsFactor(clip.DirectorPoints, clip.OperatorPoints);
            clip.Quality = clipQuality;

            clip.Views = CalculateViewsAmount(clipQuality, GetFans());

            var (likes, dislikes) = CalculateReaction(clipQuality, clip.Views);
            clip.Likes = likes;
            clip.Dislikes = dislikes;

            var (fans, money) = CalculateIncomes(clipQuality, clip.Views, settings.ClipViewCost);
            clip.FansIncome = fans;
            clip.MoneyIncome = money;
        }

        /// <summary>
        /// Вычисляет вклад рабочих очков в качество трека
        /// </summary>
        private float CalculateWorkPointsFactor(int dirPoints, int opPoints)
        {
            float workPointsImpact = 1f - settings.ClipTrackListensImpact;
            float workPointsRatio = 1f * (dirPoints + opPoints) / settings.ClipWorkPointsMax;

            float workPointsFactor = workPointsImpact * Mathf.Min(workPointsRatio, 1f);

            return workPointsFactor;
        }

        /// <summary>
        /// Вычисляет количество просмотров на основе качества клипа, кол-ва фанатов и уровня хайпа
        /// </summary>
        private int CalculateViewsAmount(float clipQuality, int fansAmount)
        {
            bool isHit = Random.Range(0f, 1f) <= settings.ClipHitChance;

            float clipGrade = settings.ClipGradeCurve.Evaluate(clipQuality);
            float hypeFactor = Mathf.Max(0.1f, PlayerManager.Data.Hype / 100f);

            int views = Convert.ToInt32(fansAmount * (clipGrade + hypeFactor));
            if (isHit)
            {
                views *= 2;
            }

            int randomizer = Convert.ToInt32(views * TEN_PERCENTS);
            return Random.Range(views - randomizer, views + randomizer);
        }

        /// <summary>
        /// Вычисляет количество лайков / дизлайков
        /// </summary>
        private (int likes, int dislikes) CalculateReaction(float clipQuality, int views)
        {
            int activeViewers = Convert.ToInt32(views * settings.ClipActiveViewers);

            int likes = Convert.ToInt32(clipQuality * activeViewers * 2f);
            int dislikes = Convert.ToInt32((1f - clipQuality) * activeViewers * 0.5f);

            return (likes, dislikes);
        }
    }
}