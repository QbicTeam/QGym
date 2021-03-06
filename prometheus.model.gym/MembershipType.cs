namespace prometheus.model.gym
{
    public class MembershipType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int PeriodicityDays { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsActive { get; set; }
        public bool ForSale { get; set; }

    }
}