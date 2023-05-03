using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgApi.Models;
using RpgApi.Utils;
using System.Collections.Generic;
using System.Linq;



[ApiController]
[Route("[Controller]")]
public class UsuariosController : ControllerBase
{
    private readonly DataContext _context;
    public UsuariosController(DataContext context)
    {
        _context = context;
    }

    private async Task<bool> UsuarioExistente(string username)
    {
        if (await _context.Usuarios.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
        {
            return true;
        }
        return false;
    }

    [HttpPost("Registrar")]

    public async Task<IActionResult> RegistrarUsuario(Usuario user)
    {
        try
        {
            if (await UsuarioExistente(user.Username))
                throw new System.Exception("Nome de usuario já existe");

            Criptografia.CriarPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
            user.PasswordString = string.Empty;
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            await _context.Usuarios.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user.Id);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("Autenticar")]
    public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
    {
        try
        {
            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower()));

            if (usuario == null)
            {
                throw new System.Exception("Usuário não encontrado.");
            }
            else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
            {
                throw new System.Exception("Senha incorreta.");
            }
            else
            {
                usuario.DataAcesso = System.DateTime.Now;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync(); //Confirma a alteração no banco

                return Ok(usuario);

            }
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("AlterarSenha")]
    public async Task<IActionResult> AlterarSenhaUsuario (Usuario credenciais)
    {
        try
        {
            Usuario usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower()));

            if (usuario == null)
            {
                throw new System.Exception("Usuário não encontrado.");
            }
            Criptografia.CriarPasswordHash(credenciais.PasswordString, out byte[] hash, out byte [] salt);
            usuario.PasswordHash = hash;
            usuario.PasswordSalt = salt;

            _context.Usuarios.Update(usuario);
            int linhasAfetadas = await _context.SaveChangesAsync();
            return Ok(linhasAfetadas);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
