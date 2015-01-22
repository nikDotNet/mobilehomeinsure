using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Models.Appraisal
{
    public class MobileHomeAppraisalViewModel
    {

        public MobileHomeAppraisalViewModel()
        {
            
            
        }


        public List<State> StateList { get; set; }
                
        public List<Manufacturer> ManufacturerList { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Please select a state")]
        public string State { get; set; }

        [Display(Name = "Manufacturer")]
        [Required(ErrorMessage = "Please select a manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Model Year")]
        [Required(ErrorMessage = "Please select a year of manufacture")]
        public int ModelYear { get; set; }

        [Display(Name = "Length")]
        [Required(ErrorMessage = "Please select a length")]
        public int Length { get; set; }

        [Display(Name = "Width")]
        [Required(ErrorMessage = "Please select a width")]
        public int Width { get; set; }

        [Display(Name = "Estimated Value")]
        public decimal EstimatedValue { get; set; }

    }
}