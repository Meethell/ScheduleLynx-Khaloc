using Console.Entities.ModelEntities;
namespace Console.Entities.TicketEntities
{
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Model Model { get; set; }
        public int ModelId { get; set; }
        public string IssueDescription { get; set; }
        public string IssueSolution { get; set; }
        public string IssueCause { get; set; }
    }
}
