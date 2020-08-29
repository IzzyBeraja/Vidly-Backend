namespace VidlyBackend.Dto
{
    public class MovieWriteDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int NumberInStock { get; set; }

        public int DailyRentalRate { get; set; }
    }
}
