namespace BankingDAL.Entities
{
    public class Service: Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string IdentifierDescription { get; set; }

        public virtual Account Account { get; set; }

        public virtual Region Region { get; set; }
    }
}
