using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Delwings.Services
{
    public class CourierApplicationService
    {
        private readonly ICourierApplicationRepository _repository;

        public CourierApplicationService(ICourierApplicationRepository repository) { _repository = repository; }
        public async Task<List<CourierApplication>> GetAllApplicationsAsync() => await _repository.GetAllApplicationsAsync();
        public async Task<CourierApplication?> GetApplicationByIdAsync(int id) => await _repository.GetApplicationByIdAsync(id);

        public async Task<CourierStatuses> GetStatusByCourierIdAsync(int courierId)
        {
            var application = await _repository.GetApplicationByCourierIdAsync(courierId);
            if (application == null) { return CourierStatuses.Declined; }

            return application.Status;
        }

        public async Task<bool> SetStatusAsync(int applicationId, CourierStatuses status)
        {
            CourierApplication? newData = await _repository.GetApplicationByIdAsync(applicationId);
            if (newData == null) { return false; }

            newData.Status = status;

            await _repository.UpdateApplicationAsync(newData);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateApplicationAsync(CourierApplication CourierApplication)
        {
            await _repository.CreateApplicationAsync(CourierApplication);
            await _repository.SaveChangesAsync();
            return CourierApplication.Id;
        }

        public async Task<bool> UpdateApplicationAsync(int id, CourierApplication newData)
        {
            var application = await _repository.GetApplicationByIdAsync(id);
            if (application == null) { return false; }

            application.CourierId = newData.CourierId;
            application.Name = newData.Name;
            application.Surname = newData.Surname;
            application.Phone = newData.Phone;
            application.Status = newData.Status;

            await _repository.UpdateApplicationAsync(application);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var applicationToDelete = await _repository.GetApplicationByIdAsync(id);
            if (applicationToDelete == null) { return false; }

            await _repository.DeleteApplicationByIdAsync(applicationToDelete);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
