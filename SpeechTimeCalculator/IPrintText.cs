namespace SpeechTimeCalculator
{
    public interface IPrintText
    {
        string GetText(Turn t);
    }

    public class ConsolePrint : IPrintText
    {
        public string GetText(Turn t)
        {
            return $"{t}";
        }
    }

    public class CsvPrint : IPrintText
    {
        public string GetText(Turn t)
        {
            return $"{t.TurnName},{t.TurnTimeWithoutCutTime}";
        }
    }
}
