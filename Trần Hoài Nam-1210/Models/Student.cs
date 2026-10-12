namespace hoccsharp.Models
{
    public class Student
    {
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string AvatarPath { get; set; } = string.Empty;

        public Student() { }

        public Student(string studentId, string fullName, int age, string gender, string avatarPath = "")
        {
            StudentId = studentId;
            FullName = fullName;
            Age = age;
            Gender = gender;
            AvatarPath = avatarPath;
        }
    }
}