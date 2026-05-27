using KarAfarin.Domain.Slider.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Domain.Slider.Entities
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string BannerPath { get; set; }

        public SliderPosition SliderPosition { get; set; }

        public bool IsActive { get; set; }
    }
}
