using System.ComponentModel.DataAnnotations;

namespace Auluxa.WebApp.Auth
{
    /// <summary>
    /// Model used when user forgot his password
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}