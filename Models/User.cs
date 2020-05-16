using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace TheWall.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Display(Name="First Name")]
        [Required(ErrorMessage = "Enter your first name")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        public string FirstName { get; set; }

        [Display(Name="Last Name")]
        [Required(ErrorMessage = "Enter your last name")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage ="Enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters")]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name="Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string Password2 { get; set; }

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Message> MessagesPosted {get;set;}

        public List<Comment> CommentsPosted {get;set;}


    }

    public class LoginUser
    {
        // No other fields!
        public string Email {get; set;}
        public string Password { get; set; }
    }

    public class Message 
    {
        [Key]
        public int MessageId { get; set; }

        public string MessageContent {get;set;}

        public int UserId { get; set; }

        public User CreatorOfMessage {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Comment> CommentsPostedOnMessage {get;set;}

    }

    public class MessageHelper 
    {
        [Required(ErrorMessage = "Cannot leave message field blank")]
        public string MessageContent {get;set;}

    }

    public class Comment 
    {
        [Key]
        public int CommentId { get; set; }

        public string CommentContent {get;set;}

        public int UserId { get; set; }

        public User CreatorOfCommment {get;set;}

        public int MessageId {get;set;}

        public Message OriginatedMessage {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }


    public class CommentHelper 
    {
        [Required(ErrorMessage = "Cannot leave comment field blank")]
        public string CommentContent {get;set;}

        public int MessageId {get;set;}

    }
}