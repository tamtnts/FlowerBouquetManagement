using System;
using System.ComponentModel.DataAnnotations;

namespace FlowerManagementWebClient.ViewModels
{
    public class CustomerViewModel
    {
        [Required]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Birthday { get; set; }
    }
}
