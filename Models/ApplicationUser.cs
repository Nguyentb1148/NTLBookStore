using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NTLBookStore.Helpers;

namespace NTLBookStore.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [StringLength(50)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string HomeAddress { get; set; } = null!;
    
    [StringLength(100)]
    public override string PhoneNumber { get; set; } = null!;
    
    [NotMapped] public string Role { get; set; }
}

