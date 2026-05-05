using Delwings.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;


namespace Delwings.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _settings;

        public ApiService(HttpClient httpClient, IOptions<ApiSettings> settings) 
        { 
            _httpClient = httpClient;
            _settings = settings;
        }


        private string? GetLocalIP()
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            socket.Connect("8.8.8.8", 65530);

            var endPoint = socket.LocalEndPoint as IPEndPoint;
            var address = endPoint?.Address.ToString();
            var port = endPoint?.Port.ToString();

            return $"{address}:{port}";
        }

        public async Task<Dictionary<string, double>?> GetPointsAsync(string fromCity, string toCity)
        {
            string fromCityApi = $"{_settings.Value.GisApi}?q={fromCity}&{_settings.Value.GisQuery}&key={_settings.Value.GisApiKey}";
            string toCityApi = $"{_settings.Value.GisApi}?q={toCity}&{_settings.Value.GisQuery}&key={_settings.Value.GisApiKey}";

            var fromResponse = await _httpClient.GetAsync(fromCityApi);
            var toResponse = await _httpClient.GetAsync(toCityApi);

            fromResponse.EnsureSuccessStatusCode();
            toResponse.EnsureSuccessStatusCode();

            string fromResponseString = await fromResponse.Content.ReadAsStringAsync();
            string toResponseString = await toResponse.Content.ReadAsStringAsync();

            var fromJson = JsonDocument.Parse(fromResponseString);
            var toJson = JsonDocument.Parse(toResponseString);

            if (fromJson.RootElement.GetProperty("meta").GetProperty("code").GetInt16() == 404)
            {
                return null;
            }

            var fromPoint = fromJson
                .RootElement
                .GetProperty("result")
                .GetProperty("items")[0]
                .GetProperty("point");

            var toPoint = toJson
                .RootElement
                .GetProperty("result")
                .GetProperty("items")[0]
                .GetProperty("point");

            double fromLat = fromPoint.GetProperty("lat").GetDouble();
            double fromLon = fromPoint.GetProperty("lon").GetDouble();

            double toLat = toPoint.GetProperty("lat").GetDouble();
            double toLon = toPoint.GetProperty("lon").GetDouble();

            var result = new Dictionary<string, double>()
            {
                { "fromLat", fromLat },
                { "fromLon", fromLon },
                { "toLat", toLat },
                { "toLon", toLon },
            };

            return result;
        }

        public string GetQRApi(string trackId)
        {
            string? ip = GetLocalIP();
            string? trackURL = $"http://{ip}/Track?trackToken=";

            string qrAPI = $"{_settings.Value.QRApi}&data={trackURL}{trackId}";
            return qrAPI;
        }
    }
}
