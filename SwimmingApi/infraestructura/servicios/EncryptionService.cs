using System.Security.Cryptography;
using System.Text;

namespace SwimmingApi.Infraestructura.Servicios;

/// <summary>
/// Servicio para encriptar y verificar contraseñas usando SHA-256 con salt.
/// </summary>
public class EncryptionService
{
    /// <summary>
    /// Genera un hash seguro de la contraseña con un salt aleatorio.
    /// </summary>
    public string HashPassword(string password)
    {
        var salt = GenerarSalt();
        var hash = ComputeHash(password, salt);
        var resultado = $"{salt}:{hash}";
        return resultado;
    }

    /// <summary>
    /// Verifica que una contraseña coincide con su hash almacenado.
    /// </summary>
    public bool VerificarPassword(string password, string hashAlmacenado)
    {
        var partes = hashAlmacenado.Split(':');
        var esCorrecto = partes.Length == 2 && ComputeHash(password, partes[0]) == partes[1];
        return esCorrecto;
    }

    // Genera un salt aleatorio en Base64
    private string GenerarSalt()
    {
        var saltBytes = RandomNumberGenerator.GetBytes(32);
        var resultado = Convert.ToBase64String(saltBytes);
        return resultado;
    }

    // Calcula el hash SHA-256 de la contraseña + salt
    private string ComputeHash(string password, string salt)
    {
        var datos = Encoding.UTF8.GetBytes($"{password}{salt}");
        var hashBytes = SHA256.HashData(datos);
        var resultado = Convert.ToBase64String(hashBytes);
        return resultado;
    }
}
