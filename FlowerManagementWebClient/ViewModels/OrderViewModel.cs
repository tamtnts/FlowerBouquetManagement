using System;
using System.ComponentModel.DataAnnotations;

namespace FlowerManagementWebClient.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Shipped Date")]
        public DateTime? ShippedDate { get; set; }

        [Required]
        [Display(Name = "Total")]
        public decimal? Total { get; set; }

        [Required]
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
    }
}
