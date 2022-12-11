using System;
using System.ComponentModel.DataAnnotations;

namespace MsFakes.Library
{
    public class User
    {
        public string Id { get; } = IdentityGenerator.New();
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime CreateDate { get; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; }

        public bool IsValid()
        {
            return !Name.IsNullOrWhiteSpace()
                && !Email.IsNullOrWhiteSpace()
                && Email.IsValidEmail();
        }
    }
}