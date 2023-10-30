using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> getReviews();
        Review getReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool ReviewExists(int reviewId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool Save();
    }
}
