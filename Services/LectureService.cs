using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using ViewModel;

namespace Services
{
    public class LectureService : ILectureService
    {
        private readonly ILogger<LectureService> _logger;
        private readonly IRepository<Lecture> _lectureRepository;

        public async Task<ServiceReturn<LectureViewModel>> AddLecture(LectureViewModel lectureViewModel)
        {
            var serviceReturn = new ServiceReturn<LectureViewModel>();
            try
            {
                // Convert string properties to appropriate types
                var lecture = new Lecture
                {
                    LectureId = lectureViewModel.LectureId,
                    LectureName = lectureViewModel.LectureName,
                    Day = Enum.TryParse<DayOfWeek>(lectureViewModel.Day, true, out var day) ? day : throw new ArgumentException("Invalid DayOfWeek value"),
                    LectureStartTime = TimeSpan.TryParse(lectureViewModel.LectureStartTime, out var startTime) ? startTime : throw new ArgumentException("Invalid TimeSpan for LectureStartTime"),
                    LectureEndTime = TimeSpan.TryParse(lectureViewModel.LectureEndTime, out var endTime) ? endTime : throw new ArgumentException("Invalid TimeSpan for LectureEndTime"),
                    // Add other property mappings as needed
                };

                // Insert the lecture entity into the repository
                await _lectureRepository.Insert(lecture);

                // Map Lecture back to LectureViewModel after saving
                serviceReturn.Data = new LectureViewModel
                {
                    LectureId = lecture.LectureId,
                    LectureName = lecture.LectureName,
                    Day = lecture.Day.ToString(),
                    LectureStartTime = lecture.LectureStartTime.ToString(),
                    LectureEndTime = lecture.LectureEndTime.ToString(),
                    // Add other property mappings as needed
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions and add technical insert error
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding lecture: {ex.Message}");
            }

            return serviceReturn;
        }


        public async Task<ServiceReturn<bool>> DeleteLecture(int lectureId)
        {
            var serviceReturn = new ServiceReturn<bool>();
            try
            {
                var lecture = await _lectureRepository.FindBy(l => l.LectureId == lectureId);
                if (lecture.Any())
                {
                    await _lectureRepository.Delete(lectureId);
                    serviceReturn.Data = true;
                }
                else
                {
                    serviceReturn.AddNotFoundError();
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalDeleteError();
                _logger.LogError($"Error deleting lecture: {ex.Message}");
            }
            return serviceReturn;
        }

        public async Task<ServiceReturn<List<string>>> GetLectureNamesByCourseId(int courseId)
        {
            var serviceReturn = new ServiceReturn<List<string>>();
            try
            {
                var lectures = await _lectureRepository.FindBy(l => l.C_id == courseId);
                serviceReturn.Data = lectures.Select(l => l.LectureName).ToList();
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving lecture names: {ex.Message}");
            }
            return serviceReturn;
        }
    }
}
