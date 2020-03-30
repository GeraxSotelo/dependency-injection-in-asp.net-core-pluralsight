using Microsoft.AspNetCore.Mvc;
using TennisBookings.Web.Services;
using TennisBookings.Web.ViewModels;

namespace TennisBookings.Web.Controllers
{
    public class HomeController : Controller
    {
        //readonly avoids the possibility of other methods from accidentally assigning a different value for the dependency after the controller is instantiated.
        private readonly IWeatherForecaster _weatherForecaster;

        //this supports dependency injection by allowing the passing of any dependencies to this consuming code when it's constructed. 
        public HomeController(IWeatherForecaster weatherForecaster)
        {
            _weatherForecaster = weatherForecaster;
        }

        [Route("")]
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel();

            // No longer using these
            //var weatherForecaster = new WeatherForecaster();
            //var currentWeather = weatherForecaster.GetCurrentWeather();
            
            // Now using IWeatherForecaster that was provided by the constructor rather than creating its own instance
            var currentWeather = _weatherForecaster.GetCurrentWeather();
            
            switch (currentWeather.WeatherCondition)
            {
                case WeatherCondition.Sun:
                    viewModel.WeatherDescription = "It's sunny right now. " +
                                                   "A great day for tennis.";
                    break;
                case WeatherCondition.Rain:
                    viewModel.WeatherDescription = "We're sorry but it's raining " +
                                                   "here. No outdoor courts in use.";
                    break;
                default:
                    viewModel.WeatherDescription = "We don't have the latest weather " +
                                                   "information right now, please check again later.";
                    break;
            }

            return View(viewModel);
        }
    }
}