using Application.Common.Enums;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetCombinedDetailsQuery : IRequest<ResponseModel<CombinedModel>>
    {
       
        public string CountryCode { get; set; }
    }
    public class GetCombinedDetailsQueryHandler : IRequestHandler<GetCombinedDetailsQuery, ResponseModel<CombinedModel>>
    {
        private readonly IFIService _fiService;

        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheOptionsProvider _cacheOptionsProvider;
        public GetCombinedDetailsQueryHandler(IFIService fIService, ICacheProvider cacheProvider, 
            ICacheOptionsProvider cacheOptionsProvider)
        {
            _fiService = fIService;
            _cacheOptionsProvider = cacheOptionsProvider;
            _cacheProvider = cacheProvider;
        }
        public async Task<ResponseModel<CombinedModel>> Handle(GetCombinedDetailsQuery request, CancellationToken cancellationToken)
        {
            StateModel stateModel = null;
           // stateModel = await _cacheProvider.Get<StateModel>("states");

            if (stateModel == null)
            {
                stateModel = await _fiService.GetAllStates(request.CountryCode);
                var cacheOptions = _cacheOptionsProvider.GetOptions(CacheEnum.States);
                await _cacheProvider.Set("states", stateModel, cacheOptions);
            }

            SalutationModel salutation = null;
          //  salutation = await _cacheProvider.Get<SalutationModel>("salutations");
            if (salutation == null)
            {
                salutation = await _fiService.GetSalutation(request.CountryCode);
                var cacheOptions = _cacheOptionsProvider.GetOptions(CacheEnum.Salutations);
                await _cacheProvider.Set("salutations", salutation, cacheOptions);
            }

            NationalityModel nationality = null;
          //  nationality = await _cacheProvider.Get<NationalityModel>("nationalities");
            if (nationality == null)
            {
                nationality = await _fiService.GetNationalities(request.CountryCode);
                var cacheOptions = _cacheOptionsProvider.GetOptions(CacheEnum.Nationalities);
                await _cacheProvider.Set("nationalities", nationality, cacheOptions);
            }

            MeansOfIdModel meansOfIdModel = null;
         //   meansOfIdModel = await _cacheProvider.Get<MeansOfIdModel>("meanOfId");
            if (meansOfIdModel == null)
            {
                meansOfIdModel = await _fiService.GetMeansOfIdentification(request.CountryCode);
                var cacheOptions = _cacheOptionsProvider.GetOptions(CacheEnum.MeansOfId);
                await _cacheProvider.Set("meanOfId", meansOfIdModel, cacheOptions);
            }
            CityModel cityModel = null;
          //  cityModel = await _cacheProvider.Get<CityModel>("cityModel");
            if (cityModel == null)
            {
                cityModel = await _fiService.GetCities(request.CountryCode);
                var cacheOptions = _cacheOptionsProvider.GetOptions(CacheEnum.Cities);
                await _cacheProvider.Set("cityModel", cityModel, cacheOptions);
            }
            BranchInfo branchInfo = null;
          //  branchInfo = await _cacheProvider.Get<BranchInfo>("branchInfo");
            if (branchInfo == null)
            {
                branchInfo = await _fiService.GetBranches(request.CountryCode);
                var cacheOptions = _cacheOptionsProvider.GetOptions(CacheEnum.Branches);
                await _cacheProvider.Set("branchInfo", branchInfo, cacheOptions);
            }
            MarritalStatusModel marritalStatus = null;
          //  marritalStatus = await _cacheProvider.Get<MarritalStatusModel>("marritalStatus");
            if (marritalStatus == null)
            {
                marritalStatus = await _fiService.GetMarritalStatus(request.CountryCode);
                var cacheOptions = _cacheOptionsProvider.GetOptions(CacheEnum.MaritalStatus);
                await _cacheProvider.Set("marritalStatus", marritalStatus, cacheOptions);
            }

            var gender = new List<GeneralResponse> { new GeneralResponse{Id="M", Value="Male" },
             new GeneralResponse { Id="F", Value= "Female"}
            };


            //var town = await _fiService.GetTown();
            //var gender = await _fiService.getg();
            var combined = new CombinedModel
            {
                MeansOfIdentification = meansOfIdModel.Types,
                Nationality = nationality.Nationalities,
                Salutations = salutation.Salutations,
                States = stateModel.States,
                Gender = gender,
                Branches = branchInfo.Branches,
                MaritalStatus = marritalStatus.MaritalStatuses,
                Cities =cityModel.Cities
            };
            return ResponseModel<CombinedModel>.Success(data: combined, "Successful"); ;
        }
    }
}
