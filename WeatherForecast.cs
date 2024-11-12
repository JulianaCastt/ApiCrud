namespace ProdutoAPI
{
    // Classe para armazenar informa��es de previs�o do tempo
    public class WeatherForecast
    {
        // Construtor para inicializar as propriedades com valores padr�o
        public WeatherForecast(DateOnly date, int temperatureC, string summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }

        // Construtor opcional para permitir a cria��o sem par�metros
        public WeatherForecast()
        {
            // Definir valores padr�o caso o construtor sem par�metros seja usado
            Date = DateOnly.FromDateTime(DateTime.Now);
            TemperatureC = 0;
            Summary = "Sem previs�o";
        }

        // Data da previs�o
        public DateOnly Date { get; set; }

        // Temperatura em Celsius
        public int TemperatureC { get; set; }

        // Temperatura em Fahrenheit (calculado automaticamente a partir da temperatura em Celsius)
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        // Resumo da previs�o do tempo (ex.: "Ensolarado", "Chuvoso", etc.)
        public string? Summary { get; set; }
    }
}
