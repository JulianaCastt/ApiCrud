namespace ProdutoAPI
{
    // Classe para armazenar informações de previsão do tempo
    public class WeatherForecast
    {
        // Construtor para inicializar as propriedades com valores padrão
        public WeatherForecast(DateOnly date, int temperatureC, string summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }

        // Construtor opcional para permitir a criação sem parâmetros
        public WeatherForecast()
        {
            // Definir valores padrão caso o construtor sem parâmetros seja usado
            Date = DateOnly.FromDateTime(DateTime.Now);
            TemperatureC = 0;
            Summary = "Sem previsão";
        }

        // Data da previsão
        public DateOnly Date { get; set; }

        // Temperatura em Celsius
        public int TemperatureC { get; set; }

        // Temperatura em Fahrenheit (calculado automaticamente a partir da temperatura em Celsius)
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        // Resumo da previsão do tempo (ex.: "Ensolarado", "Chuvoso", etc.)
        public string? Summary { get; set; }
    }
}
