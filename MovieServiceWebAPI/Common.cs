using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;

namespace MovieServiceWebAPI
{
    public static class Common
    {
        #region Enum
        public enum MovieStatus
        {
            [Display(Name = "Complete")]
            Complete,
            [Display(Name = "In Progress")]
            InProgress,
            [Display(Name = "Upcoming")]
            UpComing,
            [Display(Name = "Cancel")]
            Cancel
        }

        #endregion
    }
}
