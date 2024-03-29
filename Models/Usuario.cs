using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace RpgApi.Models
{
    public class Usuario
    {        
        public int Id { get; set; } //Atalho para propridade (PROP + TAB)
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] Foto { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? DataAcesso { get; set; } //using System;

        [NotMapped] // using System.ComponentModel.DataAnnotations.Schema
        public string Token { get; set; }
        [NotMapped]
        public string PasswordString { get; set; }
        public List<Personagem> Personagens { get; set; }//using System.Collections.Generic;
        public string Perfil { get; set; }
        public string Email { get; set; }
    }
}