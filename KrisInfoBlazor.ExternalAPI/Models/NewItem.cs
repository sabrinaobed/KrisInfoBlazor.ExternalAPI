namespace KrisInfoBlazor.ExternalAPI.Models
{
    public class NewsItem
    {
        public string Identifier { get; set; }
        public string Headline { get; set; }
        public string Preamble { get; set; }
        public string BodyText { get; set; }
        public DateTime Published { get; set; }
    }
}
