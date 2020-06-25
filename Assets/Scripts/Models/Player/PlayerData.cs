﻿using System;
using System.Collections.Generic;
using Enums;
using Models.Player.DynamicData;

namespace Models.Player
{
    /// <summary>
    /// Все данные игрока
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public PlayerInfo Info;
        public PlayerStats Stats;
        public PlayerDynamicData Data;
        public PlayerHistory History;
        public PlayerTeam Team;
        
        public List<Good> Goods;
        public List<Achievement> Achievements;
        public List<Themes> Themes;
        public List<Styles> Styles;
        public List<Skills> Skills;
        
        public static PlayerData New => new PlayerData
        {
            Info = PlayerInfo.New,
            Stats = PlayerStats.New,
            Data = PlayerDynamicData.New,
            History = PlayerHistory.New,
            Team = PlayerTeam.New,
            
            Goods = new List<Good>(),
            Achievements = new List<Achievement>(),
            Themes = new List<Themes> { Enums.Themes.Self },
            Styles = new List<Styles> { Enums.Styles.Common },
            Skills = new List<Skills>()
        };
    }
}