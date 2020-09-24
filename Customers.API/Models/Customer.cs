using System;
using System.ComponentModel.DataAnnotations;

namespace Customers.API.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "First Name is missing.")]
        [MaxLength(50, ErrorMessage = "First Name should not have more than 50 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is missing.")]
        [MaxLength(50, ErrorMessage = "Last Name should not have more than 50 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Date of Birth is missing.")]
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
