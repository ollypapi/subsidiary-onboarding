using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class ApplicationSettingHelper
    {
        private readonly ILogger<ApplicationSettingHelper> _logger;
        private readonly IOnboardingDbContext _context;

        public ApplicationSettingHelper(ILogger<ApplicationSettingHelper> logger, IOnboardingDbContext context )
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ResponseModel<long>> CreateAppSetting( ApplicationSetting setting)
        {
            var isExisting = _context.Settings.Any(s=>s.SettingType == setting.SettingType && s.CountryCode == setting.CountryCode);
            if(isExisting)
                throw new CustomException(" Setting Already Exist, You can Kindly Modify ");
            var objectString = JsonConvert.SerializeObject(setting);
            _logger.LogInformation($" data Object : {objectString}");
            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();
            return ResponseModel<long>.Success(setting.Id);
        }

        public async Task<ResponseModel<long>> CreateSchemeCodeAppSetting(SchemeCode schemeCode)
        {
            var objectString = JsonConvert.SerializeObject(schemeCode);
            _logger.LogInformation($" data Object : {objectString}");
            _context.SchemeCodes.Add(schemeCode);
            await _context.SaveChangesAsync();
            return ResponseModel<long>.Success(schemeCode.Id);
        }

        public async Task<ResponseModel> UpdateSchemeCode(SchemeCode schemeCode)
        {
            var isExisting = _context.SchemeCodes.Any(s => s.Id == schemeCode.Id);
            if (!isExisting)
                throw new CustomException("No SchemeCode found");
            _context.SchemeCodes.Update(schemeCode);
            await _context.SaveChangesAsync();
            return ResponseModel.Success("Successful");
        }

        public async Task<ResponseModel> CreateSchemeCodePermissions(List<SchemeCodeSettingPermission> Permissions)
        {
            var objectString = JsonConvert.SerializeObject(Permissions);
            _logger.LogInformation($" data Object : {objectString}");
            _context.SchemeCodeSettingPermissions.AddRange(Permissions);
            await _context.SaveChangesAsync();
            return ResponseModel.Success();
        }

      

        public async Task<ResponseModel> UpdateSetting(ApplicationSetting setting)
        {
            var isExisting = _context.Settings.Any(s => s.Id == setting.Id);
            if (!isExisting)
                throw new CustomException("No setting configuration for the object");
            _context.Settings.Update(setting);
            await _context.SaveChangesAsync();
            return ResponseModel.Success("Successful");
        }

        public async Task<ApplicationSetting> GetAppSetting(long Id)
        {
            var setting = await _context.Settings.Where(s=>s.Id==Id).FirstOrDefaultAsync();
            if(setting== null)
                throw new CustomException("No setting configuration for the object");
            return setting;
        }

        public async Task<List<SchemeCodeSettingPermission>> GetSchemeCodePermissions(long SettingId)
        {
            var permissions = _context.SchemeCodeSettingPermissions.Where(s => s.SchemeCodeId == SettingId);
            return await permissions.ToListAsync();
        }

        public async Task<ApplicationSetting> GetAppSetting(string CountryId, SettingEnum settingType )
        {
            var setting = await _context.Settings.Where(s=>s.SettingType == settingType && s.CountryCode == CountryId ).FirstOrDefaultAsync();
            return setting;
        }

        public async Task<List<ApplicationSetting>> GetAppSettings(string CountryId, SettingEnum settingType)
        {
            var setting = await _context.Settings.Where(s => s.SettingType == settingType && s.CountryCode == CountryId).ToListAsync();
            return setting;
        }

        public async Task<List<SchemeCode>> GetSchemeCodes(string countryCode)
        {
            var setting = await _context.SchemeCodes.Include(s => s.Permissions).Where(s => s.CountryCode == countryCode).ToListAsync();
            return setting;
        }

        public async Task<SchemeCode> GetSchemeCode(long Id)
        {
            var setting = await _context.SchemeCodes.Include(s => s.Permissions).Where(s => s.Id == Id).FirstOrDefaultAsync();
            return setting;
        }

        public async Task<ResponseModel> DeleteSetting(long Id, string CountryCode)
        {
            var setting = await _context.Settings.Where(s=>s.CountryCode==CountryCode && s.Id==Id).FirstOrDefaultAsync();
            _context.Settings.Remove(setting);
            await  _context.SaveChangesAsync();
            return ResponseModel.Success("Successful");
        }

        public async Task<ResponseModel> DeleteSchemeCode(long Id, string CountryCode)
        {
            var schemeCode = await _context.SchemeCodes.Where(s => s.CountryCode == CountryCode && s.Id == Id).FirstOrDefaultAsync();
            _context.SchemeCodes.Remove(schemeCode);
            await _context.SaveChangesAsync();
            return ResponseModel.Success("Successful");
        }


    }
}
