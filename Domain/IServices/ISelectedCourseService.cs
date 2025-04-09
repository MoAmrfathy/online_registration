using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Shared.Domain;
using ViewModel;

namespace Domain.IServices
{
    public interface ISelectedCourseService
    {
       public Task<ServiceReturn<SelectedCourseViewModel>> AddSelectedCourse(SelectedCourseViewModel selectedCourseViewModel);
       public Task<ServiceReturn<bool>> DeleteSelectedCourse(int id);
    }


}
