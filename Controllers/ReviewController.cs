using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult getReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.getReviews());
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult getReview(int reviewId) {
            var review = _mapper.Map<ReviewDto>(_reviewRepository.getReview(reviewId));
            if(!_reviewRepository.ReviewExists(reviewId)) 
                return NotFound();
            return Ok(review);
        }

        [HttpGet("review/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult getReviewOfAPokemon(int pokeId) {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate) {
            if (reviewCreate == null)
                return BadRequest(ModelState);
            var reviews = _reviewRepository.getReviews().Where(r => r.Title.Trim().ToUpper() == reviewCreate.Title.Trim().ToUpper()).FirstOrDefault();
            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.getReviewer(reviewerId);


            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewId, [FromBody] ReviewDto reviewUpdated)
        {
            if (reviewUpdated == null)
                return BadRequest(ModelState);
            if (reviewId != reviewUpdated.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewUpdated);
            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
