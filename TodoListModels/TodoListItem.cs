using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TodoListModels
{
    public class TodoListItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Details")]
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string DetailText { get; set; }

        public bool IsCompleted { get; set; } = false;

        public ItemStatus Status { get; set; } = ItemStatus.NotStarted;

        //TODO: Add the User ID here for tracking
        [StringLength(450)]
        public string UserId { get; set; }
    }
}