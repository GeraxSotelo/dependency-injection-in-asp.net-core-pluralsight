using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TennisBookings.Web.Configuration;
using TennisBookings.Web.Services;
using TennisBookings.Web.ViewModels;

namespace TennisBookings.Web.Controllers
{
    public class HomeController : Controller
    {
        //readonly avoids the possibility of other methods from accidentally assigning a different value for the dependency after the controller is instantiated.
        private readonly IWeatherForecaster _weatherForecaster;
        private readonly FeaturesConfiguration _featuresConfiguration;

        //this supports dependency injection by allowing the passing of any dependencies to this consuming code when it's constructed. 
        //Since the HomeController now depends on an abstraction defined by the IWeatherForecaster interface, it is now decoupled from changes to the implementation, which gets injected
        public HomeController(IWeatherForecaster weatherForecaster, IOptions<FeaturesConfiguration> options)
        {
            _weatherForecaster = weatherForecaster;

            //Once bound, you can inject an instance of IOptions of T into any classes which need access to configuration values.
            _featuresConfiguration = options.Value;
        }

        [Route("")]
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel();

            // NO LONGER USING THESE
            //var weatherForecaster = new WeatherForecaster();
            //var currentWeather = weatherForecaster.GetCurrentWeather();

            // Now using IWeatherForecaster that was provided by the constructor rather than creating its own instance
            if(_featuresConfiguration.EnableWeatherForecast)
            {
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
            }

            return View(viewModel);
        }
    }
}