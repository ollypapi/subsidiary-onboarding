using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICryptoResource
    {
        string Encrypt(string text);
        string Decrypt(string cypherText);
        string MaskCardPan(string cardPan);
    }
}
