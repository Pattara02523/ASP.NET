using MyAPI.Models;

namespace MyAPI.Interfaces
{
    public interface ICourseLibraryRepository
    {
        // Author
        IEnumerable<Author> GetAuthors();
        Author? GetAuthor(int? authorId);
        bool AuthorExists(int? authorId);
        IEnumerable<Author> GetAuthors(String mainCategory, string searchQuery);

        void AddAuthor(Author author);
        void UpdateAuthor(Author author);
        void DeleteAuthor(Author author);



        // Course
        IEnumerable<Course> GetCourses(int? authorId);
        Course? GetCourse(int? authorId, int? courseId);

        void AddCourse(int? authorId, Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);

        // user
        void Register(User user);
        bool IsUserExist(string username);
        User? Authenticate (string username, string password);


        void save();
    }
}
