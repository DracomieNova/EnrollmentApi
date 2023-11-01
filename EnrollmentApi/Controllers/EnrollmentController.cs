using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnrollmentApi.Data;
using EnrollmentApi.DTOs;
using EnrollmentApi.Model;

namespace EnrollmentApi.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly EnrollmentDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EnrollmentController(EnrollmentDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students
                .Include(s => s.Program)
                .Include(s => s.Parish)
                .Include(s => s.ShirtSize)
                .ToList();

            if (students == null || students.Count == 0)
            {
                return NotFound("No students found.");
            }

            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _context.Students
                .Include(s => s.Program)
                .Include(s => s.Parish)
                .Include(s => s.ShirtSize)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromForm] StudentWithFilesDto studentDto)
        {
            if (studentDto == null)
            {
                return BadRequest("Invalid student data.");
            }

            var student = new Student
            {
                StudentName = studentDto.StudentName,
                EmailAddress = studentDto.EmailAddress,
                PhoneNumber = studentDto.PhoneNumber,
                CoursesId = studentDto.ProgramId,
                ParishId = studentDto.ParishId,
                ShirtSizeId = studentDto.ShirtSizeId
            };

            // Handle image data
            if (studentDto.ImageFile != null && studentDto.ImageFile.Length > 0)
            {
                student.ImageData = await GetFileDataAsync(studentDto.ImageFile);
                student.ImageFileName = studentDto.ImageFile.FileName;
            }

            // Handle file data
            if (studentDto.File != null && studentDto.File.Length > 0)
            {
                student.FileData = await GetFileDataAsync(studentDto.File);
                student.FileName = studentDto.File.FileName;
            }

            _context.Students.Add(student);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while saving the student data.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromForm] StudentWithFilesDto studentDto)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Update student data
            student.StudentName = studentDto.StudentName;
            student.EmailAddress = studentDto.EmailAddress;
            student.PhoneNumber = studentDto.PhoneNumber;
            student.CoursesId = studentDto.ProgramId;
            student.ParishId = studentDto.ParishId;
            student.ShirtSizeId = studentDto.ShirtSizeId;

            // Handle image data
            if (studentDto.ImageFile != null && studentDto.ImageFile.Length > 0)
            {
                student.ImageData = await GetFileDataAsync(studentDto.ImageFile);
                student.ImageFileName = studentDto.ImageFile.FileName;
            }

            // Handle file data
            if (studentDto.File != null && studentDto.File.Length > 0)
            {
                student.FileData = await GetFileDataAsync(studentDto.File);
                student.FileName = studentDto.File.FileName;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(student);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while updating the student data.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok("Student deleted successfully.");
        }

        private async Task<byte[]> GetFileDataAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
