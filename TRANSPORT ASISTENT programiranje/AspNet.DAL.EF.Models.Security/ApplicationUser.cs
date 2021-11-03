using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNet.DAL.EF.Models.Security
{
    public class ApplicationUser : IdentityUser
    {
        //public string FavoriteColor { get; set; }

        //public int? KontaktId { get; set; }

        //[NotMapped]
        //public string NazivKontakta { get; set; }

        //[NotMapped]
        //public int RoleId { get; set; }

        //public int? RegionId { get; set; }

        //public string Barkod { get; set; }

        //public bool? Klijent { get; set; }

        //public bool? Aktivan { get; set; }

        //public int? UserId { get; set; }

        //public List<string> ListaRegionaId { get; set; }

       



        public async Task<ClaimsIdentity> CreateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            //ListaRegionaId = new List<string>();

            var userIdentity = await manager.CreateIdentityAsync(
                this,
                DefaultAuthenticationTypes.ApplicationCookie);

            //var clms = userIdentity.Claims;
            //foreach(var c in clms)
            //{
            //    Console.Write("Type:", c.Type);
            //    Console.Write("Value:", c.Value);

            //    if(c.Type.Split('/').Last().Equals("Region"))
            //        ListaRegionaId.Add(c.Value);
            //}

            //userIdentity.AddClaim(
            //    new Claim("ApplicationUser/FavoriteColor", FavoriteColor ?? ""));

            return userIdentity;
        }

        


    }
}