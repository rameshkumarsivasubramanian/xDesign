using System.Collections.Generic;
using Takehome.API.Models;

namespace Takehome.API.Services
{
    public interface IMunroLibraryRepository
    {
        IEnumerable<Munro> GetMunros();
    }
}
