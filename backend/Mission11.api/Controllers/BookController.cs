using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission11.API.Data;

namespace Mission11.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private BookDbContext _bookContext;
        public BookController(BookDbContext temp)
        {
            _bookContext = temp;
        }

        [HttpGet("AllBooks")]
        public IActionResult GetBooks(int pageSize = 10, int pageNum = 1, [FromQuery] List<string>? bookCategories = null, string? sortBy = null)
        {
            var booksQuery = _bookContext.Books.AsQueryable();

            // Filter by book categories if user selected any
            if (bookCategories != null && bookCategories.Any())
            {
                booksQuery = booksQuery.Where(b => bookCategories.Contains(b.Category));
            }

            // Only apply sorting if user selected a sort options
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "title")
                {
                    booksQuery = booksQuery.OrderBy(b => b.Title); // Replace with your actual property
                }
            }

            // Total count BEFORE pagination
            var totalNumBooks = booksQuery.Count();

            // Apply pagination
            var pagedBooks = booksQuery
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return results
            var result = new
            {
                Books = pagedBooks,
                TotalNumBooks = totalNumBooks
            };

            return Ok(result);
        }

        [HttpGet("GetBookCategory")]
        public IActionResult GetBookCategory ()
        {
            var categories = _bookContext.Books
                .Select(b => b.Category)
                .Distinct()
                .ToList();

            return Ok(categories);
        }

        [HttpPost("AddBook")]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            _bookContext.Books.Add(newBook);
            _bookContext.SaveChanges();
            return Ok(newBook);
        }

        [HttpPut("UpdateBook/{bookID}")]
        public IActionResult UpdateBook(int bookID, [FromBody] Book updatedBook)
        {
            var existingBook = _bookContext.Books.Find(bookID);

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Publisher = updatedBook.Publisher;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.Classification = updatedBook.Classification;
            existingBook.Category = updatedBook.Category;
            existingBook.PageCount = updatedBook.PageCount;
            existingBook.Price = updatedBook.Price;

            _bookContext.Books.Update(existingBook);
            _bookContext.SaveChanges();

            return Ok(existingBook);
        }

        [HttpDelete("DeleteBook/{bookID}")]
        public IActionResult DeleteBook(int bookID)
        {
            var bookToDelete = _bookContext.Books.Find(bookID);
            if (bookToDelete == null)
            {
                return NotFound(new {message = "Book not found"});
            }

            _bookContext.Books.Remove(bookToDelete);
            _bookContext.SaveChanges();

            return NoContent();
        }

    }


};
