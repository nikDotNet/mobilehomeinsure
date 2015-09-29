using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MobileHome.Insure.Model;

namespace mobilehome.insure.Models.Rental
{
    public class RentalViewModel
    {
        public Customer customer { get; set; }
        public Quote quote { get; set; }
        public Payment payment { get; set; }

        [Display(Name = "Enter your Zip Code")]
        public string Zip { get; set; }
        
        public int currentStep { get; set; }

        public RentalViewModel()
        {
            customer = new Customer();
            quote = new Quote();
            payment = new Payment();
        }

        public class Customer
        {
            public int CustomerId { get; set; }
            [Required]
            [Display(Name = "Insured First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Insured Last Name")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Email")]
            [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Enter valid email.")]
            public string Email { get; set; }

            [Display(Name = "Insured 2 First Name")]
            public string FirstName2 { get; set; }
            [Display(Name = "Insured 2 Last Name")]
            public string LastName2 { get; set; }
            [Display(Name = "Insured 2 Email")]
            
            public string Password { get; set; }
            [Display(Name = "Address")]
            [Required]
            public string Address { get; set; }
            [Display(Name = "State")]
            public int StateId { get; set; }
            public List<State> States { get; set; }

            public string City { get; set; }
            public string Zip { get; set; }
            [Required]
            [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Enter valid phone no.")]
            public string Phone { get; set; }
            public int ParkId { get; set; }
        }

        public class Quote
        {
            public Quote()
            {
                InstallmentList = new Dictionary<int, string>();
                InstallmentList.Add(0, "");
                InstallmentList.Add(1, "Full Payment");
                InstallmentList.Add(2, "2 Payments");
                InstallmentList.Add(3, "3 Payments");
                InstallmentList.Add(4, "4 Payments");

                SendLandlord = true;
            }

            public int QuoteId { get; set; }
            public int CustomerId { get; set; }

            [Display(Name = "Effective Date")]
            public DateTime EffectiveDate { get; set; }

            [Display(Name = "Personal Property")]
            public decimal PersonalProperty { get; set; }

            public decimal Liability { get; set; }
            public decimal Deductible { get; set; }
            public decimal Premium { get; set; }

            [Display(Name = "Choose Number of Payments")]
            [Range(1,4, ErrorMessage="Select Installment")]
            public int NumberOfInstallments { get; set; }

            [Display(Name = "Installment Fee")]
            public decimal InstallmentFee { get; set; }

            public Dictionary<int, string> InstallmentList { get; set; }

            public List<OptionListItem> Liabilities { get; set; }

            public List<OptionListItem> PersonalProperties { get; set; }

            public bool SendLandlord { get; set; }
        }

        public class Payment
        {

            public Payment()
            {
                MonthList = new List<int>();
                for (int i = 1; i <= 12; i++)
                    MonthList.Add(i);
                YearList = new List<int>();
                for (int i = DateTime.Now.Year; i < 2040; i++)
                    YearList.Add(i);

            }

            public int? CustomerId { get; set; }
            public int? QuoteId { get; set; }

            [Display(Name = "Name on Card")]
            [Required]
            public string NameOnCard { get; set; }

            [Display(Name = "Credit Card Number")]
            [Required]
            public string CreditCardNumber { get; set; }
            [Display(Name = "Expiry Month")]
            [Required]
            public int ExpiryMonth { get; set; }

            [Display(Name = "Expiry Year")]
            [Required]
            public int ExpiryYear { get; set; }
            public decimal Amount { get; set; }

            [Display(Name = "Billing Address Line1")]
            public string BillingAddressLine1 { get; set; }

            [Display(Name = "Billing Address Line2")]
            public string BillingAddressLine2 { get; set; }

            public string Zip { get; set; }


            public List<int> MonthList { get; set; }
            public List<int> YearList { get; set; }


        }

    }
}