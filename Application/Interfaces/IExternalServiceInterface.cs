using Application.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExternalServiceInterface
    {
        Task<MoveFileResponse> SaveFiles(SaveFilesModel fileDetail );
        Task<MoveFileResponse> MoveFiles(MoveFilesModel fileDetail );
    }
}
