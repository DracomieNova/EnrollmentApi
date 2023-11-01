using Microsoft.AspNetCore.Http;

namespace EnrollmentApi.DTOs
{
    public class StudentWithFilesDto
    {
        public string StudentName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int ProgramId { get; set; }
        public int ParishId { get; set; }
        public int ShirtSizeId { get; set; }

        public IFormFile ImageFile { get; set; } // Property to handle image upload
        public IFormFile File { get; set; } // Property to handle file upload
    }
}
