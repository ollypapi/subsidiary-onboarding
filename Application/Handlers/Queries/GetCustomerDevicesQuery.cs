using Application.Common.Models;
using Application.Helper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetCustomerDevicesQuery : IRequest<ResponseModel<List<CustomerDeviceModel>>>
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
    }
    public class GetCustomerDevicesQueryValidator : AbstractValidator<GetCustomerDevicesQuery>
    {
        public GetCustomerDevicesQueryValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
        }
    }
    public class GetCustomerDevicesQueryandler : IRequestHandler<GetCustomerDevicesQuery, ResponseModel<List<CustomerDeviceModel>>>
    {
        private readonly CustomerHelper _customerHelper;
        public GetCustomerDevicesQueryandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel<List<CustomerDeviceModel>>> Handle(GetCustomerDevicesQuery request, CancellationToken cancellationToken)
        {
            var data = await _customerHelper.GetCustomerDevices(request.CustomerId);
            var deviceModel = new List<CustomerDeviceModel>();
            data.ForEach(d =>
            {
                deviceModel.Add(new CustomerDeviceModel { DeviceId = d.DeviceId, DeviceName = d.DeviceName, OS = d.OS, DateCreated= d.DateCreated,Status = d.Status, Id = d.Id });
            });
            return ResponseModel<List<CustomerDeviceModel>>.Success(deviceModel);
        }
    }
   
}
