using System.Security.Cryptography;
using System.Text;

namespace SwimmingApi.Infraestructura.Servicios;

/// <summary>
/// Servicio para encriptar y verificar contraseñas usando el algoritmo SHA-256
/// combinado con un salt aleatorio único por usuario.
/// El salt evita que dos usuarios con la misma contraseña generen el mismo hash,
/// protegiendo el sistema frente a ataques con tablas precalculadas (rainbow tables).
/// </summary>
public class EncryptionService
{
    /// <summary>
    /// Genera un hash seguro de la contraseña concatenándola con un salt aleatorio.
    /// El resultado se devuelve en el formato "salt:hash" para poder verificarlo después.
    /// </summary>
    public string HashPassword(string password)
    {
        var salt = GenerarSalt();
        var hash = ComputeHash(password, salt);
        var resultado = $"{salt}:{hash}";
        return resultado;
    }

    /// <summary>
    /// Verifica que una contraseña en texto plano coincide con un hash almacenado.
    /// Extrae el salt del hash guardado, vuelve a calcular el hash con la contraseña
    /// que el usuario acaba de introducir y compara ambos.
    /// </summary>
    public bool VerificarPassword(string password, string hashAlmacenado)
    {
        var partes = hashAlmacenado.Split(':');
        var esCorrecto = partes.Length == 2 && ComputeHash(password, partes[0]) == partes[1];
        return esCorrecto;
    }

    /// <summary>
    /// Genera un salt criptográficamente seguro de 32 bytes,
    /// codificado en Base64 para poder guardarlo como texto.
    /// </summary>
    private string GenerarSalt()
    {
        var saltBytes = RandomNumberGenerator.GetBytes(32);
        var resultado = Convert.ToBase64String(saltBytes);
        return resultado;
    }

    /// <summary>
    /// Calcula el hash SHA-256 del resultado de concatenar la contraseña con el salt.
    /// Devuelve el hash codificado en Base64.
    /// </summary>
    private string ComputeHash(string password, string salt)
    {
        var datos = Encoding.UTF8.GetBytes($"{password}{salt}");
        var hashBytes = SHA256.HashData(datos);
        var resultado = Convert.ToBase64String(hashBytes);
        return resultado;
    }
}