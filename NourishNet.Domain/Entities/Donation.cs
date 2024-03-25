namespace NourishNet.Domain.Entities;

public class Donation
{
    public int Id { get; set; }
    public int DonorId { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int StatusId { get; set; }

    public DonationStatus Status { get; set; }
}