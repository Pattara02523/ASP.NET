using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAPI.Extensions;
using MyAPI.Interfaces;
using MyAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAPI.Repository
{
    public class CourseLibraryRepository : ICourseLibraryRepository
    {
        private readonly WorkshopAPI _context;
        private readonly AppSettings _appSettings;
        public CourseLibraryRepository(WorkshopAPI context,
            IOptions<AppSettings> appSettings)

        {
            _context = context ??
            throw new ArgumentNullException(nameof(context));
            _appSettings = appSettings?.Value??
            throw new ArgumentNullException(nameof(appSettings));
        }


        // Author
        public bool AuthorExists(int? authorId)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public Author? GetAuthor(int? authorId)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));
            var data = _context.Authors.SingleOrDefault(a => a.Id == authorId);
            return data;
        }

        public IEnumerable<Author> GetAuthors()
        {
            var data = _context.Authors.ToList();
            return data;
        }

        public IEnumerable<Author> GetAuthors(String mainCategory, string searchQuery)
        {
            var data = _context.Authors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(mainCategory))
            {
                mainCategory = mainCategory.Trim();
                data = data.Where(a => a.MainCategory == mainCategory);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                data = data.Where(a => a.MainCategory.Contains(searchQuery)
                || a.FirstName.Contains(searchQuery)
                || a.LastName.Contains(searchQuery));

            }
            return data.ToList();
        }
        public void AddAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));
            _context.Authors.Add(author);
        }
        public void UpdateAuthor(Author author)
        {
            var data = _context.Authors.SingleOrDefault(a => a.Id == author.Id);
            if (data == null)
                throw new ArgumentNullException(nameof(author));
            data.FirstName = author.FirstName;
            data.LastName = author.LastName;
            data.DateOfBirth = author.DateOfBirth;
            data.MainCategory = author.MainCategory;
            _context.Authors.Update(data);
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));
            _context.Authors.Remove(author);
        }

        // Course
        public Course? GetCourse(int? authorId, int? courseId)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));
            if (courseId == null)
                throw new ArgumentNullException(nameof(courseId));
            return _context.Courses.SingleOrDefault(c => c.AuthorId == authorId && c.Id == courseId);
        }

        public IEnumerable<Course> GetCourses(int? authorId)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));
            return _context.Courses.Where(c => c.AuthorId == authorId).ToList();
        }
        public void AddCourse(int? authorId, Course course)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            course.AuthorId = authorId.Value;
            _context.Courses.Add(course);
        }

        public void UpdateCourse(Course course)
        {
            var data = _context.Courses.SingleOrDefault(a => a.Id == course.Id);
            if (data == null)
                throw new ArgumentNullException(nameof(course));
            data.Title = course.Title;
            data.Description = course.Description;
            _context.Courses.Update(data);
        }
        public void DeleteCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            _context.Courses.Remove(course);
        }


        public void save()
        {
            _context.SaveChanges();
        }

        public void Register(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            _context.Users.Add(user);

        }

        public bool IsUserExist(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));
            return _context.Users.Any(u => u.Username == username);
        }

        public User? Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == Util.ComputeMD5Hash(password));
            if (user == null)
                return null;    


            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(7),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(tokenKey), 
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;
            return user;
        }
    }
}
