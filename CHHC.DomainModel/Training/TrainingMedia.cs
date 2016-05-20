namespace CHHC.DomainModel
{
    public class TrainingMedia
    {
        public int Id { get; set; }
        public virtual Training Training { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public virtual MediaType MediaType { get; set; }
    }
}
