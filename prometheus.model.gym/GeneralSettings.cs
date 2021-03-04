namespace prometheus.model.gym
{
    public class GeneralSettings
    {
        public int Id { get; set; }
        public int TotalCapacity { get; set; }
        public int ScheduleChangeHours { get; set; }
        public int LoginAttempts { get; set; }
        public string ScheduledWeek { get; set; }
        public string CovidMsg { get; set; }
        public int NotificationCapacity { get; set; }
        public bool RegistrationCost { get; set; }
        public bool ReregistrationCost { get; set; }

    }
}