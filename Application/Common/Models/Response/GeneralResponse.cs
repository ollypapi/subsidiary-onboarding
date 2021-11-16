using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response
{
    [Serializable]
    public class GeneralResponse
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class BranchDetail: ResponseBase
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }
    [Serializable]
    public class StateModel: ResponseBase
    {
        public List<GeneralResponse> States { get; set; }
    }
    [Serializable]
    public class BranchInfo: ResponseBase
    {
        public List<BranchDetail> Branches { get; set; }
    }
    [Serializable]
    public class NationalityModel : ResponseBase
    {
        public List<GeneralResponse> Nationalities { get; set; }
    }
    [Serializable]
    public class MeansOfIdModel: ResponseBase
    {
        public List<GeneralResponse> Types { get; set; }
    }
    [Serializable]
    public class CityModel: ResponseBase
    {
        public List<GeneralResponse> Cities { get; set; }
    }
    [Serializable]
    public class MarritalStatusModel : ResponseBase
    {
        public List<GeneralResponse> MaritalStatuses { get; set; }
    }

    [Serializable]
    public class CountriesModel : ResponseBase
    {
        public List<GeneralResponse> Countries { get; set; }
    }

    [Serializable]
    public class SalutationModel
    {
        public List<GeneralResponse> Salutations { get; set; }
    }
    [Serializable]
    public class CombinedModel 
    {
        public List<GeneralResponse> States { get; set; }
        public List<GeneralResponse> Salutations { get; set; }
        public List<GeneralResponse> Nationality { get; set; }
        public List<GeneralResponse> MeansOfIdentification { get; set; }
        public List<GeneralResponse> Gender { get; set; }
        public List<BranchDetail> Branches { get; set; }
        public List<GeneralResponse> MaritalStatus { get; set; }
        public List<GeneralResponse> Cities { get; set; }

    }
    public class MoveFileResponse
    {
        public List<string> MigratedFiles { get; set; }
    }
    public class MoveFilesModel
    {

      public List<FileModel> fileModels { get; set; }

    }
    public class FileModel
    {
        public string Folder { get; set; }
        public string File { get; set; }
    }
     public class SaveFilesModel
    {

        public string Folder { get; set; }
        public List<string> Files { get; set; }

    }
}

