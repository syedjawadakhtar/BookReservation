public class BookStatusHistory
{
    public int BookStatusHistoryId { get; set; }
    public int BookId { get; set; }
    public required bool Reserved { get; set; }
    public DateTime StatusChangedOn { get; set; }

    public required Book Book { get; set; }
}

