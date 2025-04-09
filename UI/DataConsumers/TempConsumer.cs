using Shared.UI;
using UI.Helpers;
using UI.HttpClientServices;
using ViewModel;

namespace UI.DataConsumers
{
    public class TempConsumer
    {
        private ApiService _apiService;

        public TempConsumer(ApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<APIReturn<int>> UpdateCourseSyllabus(TemplateViewModel courseSyllabus)
        {
            var Ret = await _apiService.httpClient.PostJsonAsync<int, TemplateViewModel>("api/CourseSyllabus/Test", courseSyllabus);
            return Ret;
        }
        public async Task<APIReturn<TemplateViewModel>> GetTest()
        {
            var content = await _apiService.httpClient.GetJsonAsync<TemplateViewModel>($"api/Test/TestGet");
            return content;
        }
    }
}
