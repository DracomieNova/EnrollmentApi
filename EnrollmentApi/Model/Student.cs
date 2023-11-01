using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentApi.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int CoursesId { get; set; }
        public int ParishId { get; set; }
        public int ShirtSizeId { get; set; }

        // Navigation properties
        [ForeignKey("CoursesId")]
        public virtual Course Program { get; set; }

        [ForeignKey("ParishId")]
        public virtual Parish Parish { get; set; }

        [ForeignKey("ShirtSizeId")]
        public virtual ShirtSize ShirtSize { get; set; }

        // New properties for image and file
        public byte[] ImageData { get; set; } // Store image as byte array
        public string ImageFileName { get; set; } // Store image file name
        public byte[] FileData { get; set; } // Store file as byte array
        public string FileName { get; set; } // Store file name
    }
}
