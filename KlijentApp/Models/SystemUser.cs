using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using KlijentApp.Models;
using System.Linq;
using System.ComponentModel;

namespace KlijentApp.Models
{
    public class SystemUser
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { set; get; }
        [DisplayName("Nadimak korisnika")]
        public string Nick { set; get; }
        [DisplayName("Korisničko ime")]
        public string UserName { set; get; }
        [DisplayName("Lozinka")]
        public string Password { set; get; }
        [DisplayName("Odgovornost")]
        public string Role { set; get; }
        [DisplayName("Firma")]
        public string Company { get; set; }
        [DisplayName("Komentar")]
        public string Comment { get; set; }
        [DisplayName("Aktivan")]
        public bool Active { get; set; }
        [DisplayName("Napravljen nalog")]
        public DateTime FirstCreated { get; set; }
        [DisplayName("Poslednji put prijavljen")]
        public DateTime LastLogin { get; set; }
        [DisplayName("E pošta")]
        public string Email { get; set; }

   
        public ICollection<Transaction> Transactions { set; get; }

        public bool IsValid(string username, string password)
        {
            using (ModelEF dsbaza = new ModelEF())
            {
                var lista = dsbaza.SystemUsers.Where(x => @x.Active && @x.Password == @password && @x.UserName == @username ).ToList();
                if (lista.Count > 0) { return true; } else { return false; }
            }
          
          
           
            
        }
    }
}