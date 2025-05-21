namespace am3burger.Models
{
    public class HeadPicture
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
