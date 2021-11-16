using System.ComponentModel;

namespace Application.Common.Enums
{
    public enum IdentificationEnum
    {
        DriverLicense = 1,
        InternationalPassport
    }
    public enum DocumentTypeEnum
    {
        Selfie = 1,
        Signature,
        IdentityCard,
        CertificateOfResidence
    }
    public enum CacheEnum
    {
        States,
        MaritalStatus,
        Nationalities,
        MeansOfId,
        Salutations,
        Categories,
        Branches,
        Cities
    }
}
