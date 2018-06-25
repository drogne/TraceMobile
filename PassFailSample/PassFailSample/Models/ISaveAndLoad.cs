using System.Threading.Tasks;

namespace PassFailSample.Models
{
    public interface ISaveAndLoad
    {
        bool Initialize();
        bool AppendText(string text);
        string LoadText();
    }
}