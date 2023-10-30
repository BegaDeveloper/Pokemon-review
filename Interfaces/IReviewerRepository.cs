using Microsoft.EntityFrameworkCore.Update.Internal;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> getReviewers();
        Reviewer getReviewer(int id);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool ReviewerExists(int id);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool Save();
       
    }
}
