namespace CirclesSchedule
{
    public class CircleInfo
    {
        public string Name { get; set; }
        public string Leader { get; set; }
        public string DaysOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Room { get; set; }
        public string AgeGroup { get; set; }

        public CircleInfo()
        {
            Name = string.Empty;
            Leader = string.Empty;
            DaysOfWeek = string.Empty;
            StartTime = string.Empty;
            EndTime = string.Empty;
            Room = string.Empty;
            AgeGroup = string.Empty;
        }
    }
}