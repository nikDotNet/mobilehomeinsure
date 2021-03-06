﻿using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileHome.Insure.Web.Models.Appraisal
{
    public class MobileHomeAppraisalViewModel
    {

        public MobileHomeAppraisalViewModel()
        {
            
            
        }


        public List<State> StateList { get; set; }
                
        public List<Manufacturer> ManufacturerList { get; set; }

        [Display(Name = "Options Type")]
        public Dictionary<int, string> OptionsType { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Please select a state")]
        public string State { get; set; }
    
        [Display(Name = "Manufacturer")]
        [Required(ErrorMessage = "Please select a manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Model Year")]
        [Required(ErrorMessage = "Please select a year of manufacture")]
        [Range(1975, 2014, ErrorMessage="Please enter a valid year")]
        public int ModelYear { get; set; }

        [Display(Name = "Length")]
        [Required(ErrorMessage = "Please select a length")]
        [Range(1, 100000, ErrorMessage = "Please enter length")]
        public int Length { get; set; }
            
        [Display(Name = "Width")]
        [Required(ErrorMessage = "Please select a width")]
        [Range(1, 100000, ErrorMessage = "Please enter width")]
        public int Width { get; set; }

        [Display(Name = "Estimated Value")]
        public decimal EstimatedValue { get; set; }

        [Display(Name = "Selected Options")]
        public Dictionary<int, string> selectedOptionsType { get; set; }

        [Display(Name = "Brick Skirting/Underpinning Footage")]
        public double BrickSkirtingLinearFootage { get; set; }

        [Display(Name = "Vinyl/Other Skirting/Underpinning Footage")]
        public double VinylSkirtingLinearFootage { get; set; }

        [Display(Name = "Area of Decks / Porches")]
        public double AreaOfDeckPorche { get; set; }

        [Display(Name = "Area of Additions")]
        public double AreaOfAdditions { get; set; }
    }


}