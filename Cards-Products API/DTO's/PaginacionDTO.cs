namespace Cards_Products_API.DTO_s
{
    public class PaginacionDTO<T>
    {
        public int Total_Records { get; set; }
        public int Total_Pages { get; set; }
        public int Page { get; set; }
        public int Page_Size { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
