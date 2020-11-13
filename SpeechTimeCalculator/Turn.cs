using System.Collections.Generic;

namespace SpeechTimeCalculator
{
    public class Turn
    {
        public string TurnName { get; set; }
        public double TurnStartTime { get; set; }
        public double TurnEndTime { get; set; }
        public double TurnCutTime { get; set; } = 0;
        public double TurnTotalTime => TurnEndTime - TurnStartTime;
        public double TurnTimeWithoutCutTime => TurnTotalTime - TurnCutTime;
        public List<Row> TurnRows { get; set; } = new List<Row>();

        public void CalculateCutTime()
        {
            foreach (var row in TurnRows)
            {
                if (!row.IsCut())
                    continue;
                TurnCutTime += row.TotalTime;
            }
        }

        public override string ToString()
        {
            return $"{TurnName} = {TurnTimeWithoutCutTime}";
        }

        public static Turn GetTurn(Row row)
        {
            if (!row.IsTurn())
                return null;

            var turn = new Turn
            {
                TurnName = row.Line + "_" + row.Tier + "_" + row.Text,
                TurnStartTime =  row.Tmin,
                TurnEndTime = row.Tmax
            };

            return turn;
        }
    }
}
