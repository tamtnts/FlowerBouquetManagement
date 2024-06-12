using System.ComponentModel.DataAnnotations;

namespace FlowerManagementWebClient.ViewModels
{
    public class OrderDetailViewModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Flower Bouquet")]
        public int FlowerBouquetId { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Discount { get; set; }
    }
}
