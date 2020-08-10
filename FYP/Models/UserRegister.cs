using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class UserRegister
    {

        [Required(ErrorMessage = "Please enter User ID")]
        [Microsoft.AspNetCore.Mvc.Remote(action: "VerifyUserID", controller: "Account")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5 characters or more")]
        public string UserPw { get; set; }

        [Compare("UserPw", ErrorMessage = "Passwords do not match")]
        public string UserPw2 { get; set; }

        [Required(ErrorMessage = "Please enter your First Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Contact Number ")]
        [Phone(ErrorMessage = "Invalid Contact No.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Please enter your Postal code ")]
        public string Postal { get; set; }

        [Required(ErrorMessage = "Please enter your Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Please enter your Unit No")]
        [RegularExpression (@"\s\#\d{2}\-\d{2,3}", ErrorMessage = "Please enter your Unit No in this Format : #01-11 / #01-111")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "Please enter your Unit No. in this Format : #01-11 / #01-111")]
        public string UnitNo { get; set; }

        [RegularExpression(@"North|South|East|West|Central", ErrorMessage = "Please enter your Unit No in this Format : #01-11 / #01-111")]

        public string Region { get; set; }


        public string UserRole { get; set; }

        public DateTime LastLogin { get; set; }


    }
}