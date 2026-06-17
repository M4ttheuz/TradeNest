using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TradeNest.Models;

namespace TradeNest.Models
{
    public class UserReview
    {
        public int Id { get; set; }

        // Trzy składowe oceny
        [Range(1, 5)]
        public int DescriptionRating { get; set; } 

        [Range(1, 5)]
        public int ResponseTimeRating { get; set; } 

        [Range(1, 5)]
        public int PolitenessRating { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int TargetUserId { get; set; }
        public User TargetUser { get; set; } = null!;

        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;
        public int? ListingId { get; set; }
        public Listing? Listing { get; set; }

        [Required(ErrorMessage = "Pole 'Co kupiono' jest wymagane.")]
        public string WhatWasBought { get; set; } = null!;

        [Required(ErrorMessage = "Wprowadź datę zakupu przedmiotu.")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        // Średnia ocena dla TEJ konkretnej opinii
        [NotMapped]
        public double AverageReviewRating => Math.Round((DescriptionRating + ResponseTimeRating + PolitenessRating) / 3.0, 2);
    }
}
