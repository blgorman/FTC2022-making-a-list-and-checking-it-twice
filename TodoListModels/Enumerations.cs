using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListModels
{
    public enum ItemStatus
    {
        [Display(Name = "Not Started")]
        NotStarted = 1,
        [Display(Name = "In Progress")]
        InProgress = 2,
        [Display(Name = "Completed")]
        Completed = 3,
        [Display(Name = "Abandoned")]
        Abandoned = 4,
        [Display(Name = "On Hold")]
        OnHold = 5
    }


}
