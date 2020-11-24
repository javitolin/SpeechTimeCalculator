using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpeechTimeCalculator
{
    class Program
    {
        private static List<Row> Rows = new List<Row>();
        private static List<Turn> Turns = new List<Turn>();
        private static ConsolePrint cPrint = new ConsolePrint();
        private static CsvPrint csvPrint = new CsvPrint();

        private const string ExportFile = "Calculated.csv";

        static void Main(string[] args)
        {
            var filename = args.Length > 0 ? args[0] : null;
            if (filename == null)
            {
                PrintToUser($"File [{filename ?? "(No file)"}] not found", true);
                return;
            }

            try
            {
                Run(filename);
            }
            catch (Exception e1)
            {
                PrintToUser(e1.Message, true);
            }
        }

        private static void Run(string filename)
        {
            var lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                    continue;

                Row row = Row.ReadLine(lines[i]);
                if (row == null)
                    throw new Exception($"Line [{lines[i]}] is not OK. Please check it has all the values");

                Rows.Add(row);
            }

            foreach (var row in Rows)
            {
                if (row.IsTurn())
                    Turns.Add(Turn.GetTurn(row));
            }

            foreach (var row in Rows.Where(r => r.IsCut()))
            {
                // Find correct turn
                var correctTurn = Turns.FirstOrDefault(t =>
                    t.TurnStartTime <= row.Tmin && row.Tmin < t.TurnEndTime &&
                    t.TurnStartTime < row.Tmax && row.Tmax <= t.TurnEndTime);
                if (correctTurn == null)
                {
                    PrintToUser($"Couldn't find correct Turn for row: [{row}]. Turns are: [{string.Join(",", Turns)}]", true);
                    return;
                }
                correctTurn.TurnRows.Add(row);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Turn Name,Time");
            foreach (var turn in Turns)
            {
                turn.CalculateCutTime();
                PrintToUser(cPrint.GetText(turn));
                stringBuilder.AppendLine(csvPrint.GetText(turn));
            }

            if (File.Exists(ExportFile))
                File.Delete(ExportFile);

            File.WriteAllText(ExportFile, stringBuilder.ToString());
        }

        static void PrintToUser(string message, bool waitForUserInput = false)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine();

            if (waitForUserInput)
            {
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
