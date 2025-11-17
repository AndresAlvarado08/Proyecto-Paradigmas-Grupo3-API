namespace Cards_Products_API.DTO_s
{
    public class PaginacionDTO<P>
    {
        public IEnumerable<P> Items { get; set; } = Enumerable.Empty<P>();
        public int Total_Items { get; set; }
        public int Page { get; set; }
        public int Page_Size { get; set; }
    }
}
