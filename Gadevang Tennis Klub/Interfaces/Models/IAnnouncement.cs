namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface IAnnouncement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }  
        public DateTime UploadTime { get; set; }
        public string Type { get; set; }
        public IMember Announcer { get; set; }
        public bool? Actual { get; set; }
    }
}
