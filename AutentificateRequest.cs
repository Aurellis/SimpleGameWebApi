using System.ComponentModel.DataAnnotations;

namespace TestTaskGame
{
    public class AutentificateRequest
    {
        [Required]
        public string Name { get;  set; }

        [Required]
        public string Password { get;  set; }
    }
}