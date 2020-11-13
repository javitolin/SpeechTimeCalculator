using System;

namespace SpeechTimeCalculator
{
    public class Row
    {
        public int Line { get; set; }
        public double Tmin { get; set; }
        public double Tmax { get; set; }
        public string Tier { get; set; }
        public string Text { get; set; }
        public double TotalTime => Tmax - Tmin;

        public bool IsTurn()
        {
            return Tier.Equals("Turn", StringComparison.CurrentCultureIgnoreCase)
                   && !string.IsNullOrWhiteSpace(Text);
        }

        public bool IsCut()
        {
            return Tier.Equals("TimesToCut", StringComparison.CurrentCultureIgnoreCase)
                   && Text.Equals("cut", StringComparison.CurrentCultureIgnoreCase);
        }

        public static Row ReadLine(string line, char separator = '\t')
        {
            var separatedLine = line.Split(separator);
            if (separatedLine.Length != 5)
                return null;

            var returnRow = new Row
            {
                Line = int.Parse(separatedLine[0]),
                Tmin = double.Parse(separatedLine[1]),
                Tier = separatedLine[2],
                Text = separatedLine[3],
                Tmax = double.Parse(separatedLine[4])
            };

            return returnRow;
        }

        public override string ToString()
        {
            return
                $"{nameof(Line)}: {Line}, " +
                $"{nameof(Tmin)}: {Tmin}, " +
                $"{nameof(Tmax)}: {Tmax}, " +
                $"{nameof(Tier)}: {Tier}, " +
                $"{nameof(Text)}: {Text}, " +
                $"{nameof(TotalTime)}: {TotalTime}";
        }
    }
}
