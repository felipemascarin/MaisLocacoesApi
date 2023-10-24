using System.Net;

namespace MaisLocacoes.WebApi.Utils.Helpers
{
    public static class TimeZoneConverter<T>
    {
        public static T ConvertToTimeZoneLocal(T objectToConverter, TimeZoneInfo timeZone)
        {
            if (objectToConverter == null)
                return objectToConverter;

            // Obtém o tipo do objeto
            Type objectType = typeof(T);

            // Verifica se o tipo do objeto é uma classe
            if (objectType.IsClass)
            {
                // Obtém as propriedades públicas do objeto
                var properties = objectType.GetProperties();

                foreach (var property in properties)
                {
                    if (((property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)) && property.GetValue(objectToConverter) != null) && !property.Name.Equals("bornDate", StringComparison.OrdinalIgnoreCase))
                    {
                        // Verifica se a propriedade é do tipo DateTime
                        DateTime originalValue = (DateTime)property.GetValue(objectToConverter);

                        //Somente propriedades borndate podem não ser UTC
                        if (originalValue.Kind != DateTimeKind.Utc)
                            throw new HttpRequestException("A API só aceita datas no formato UTC, exceto para propriedade BornDate.", null, HttpStatusCode.NotFound);

                        // Converte o valor para o fuso horário local
                        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(originalValue, timeZone);

                        // Define o novo valor da propriedade
                        property.SetValue(objectToConverter, localTime);
                    }
                }
            }

            return objectToConverter;
        }
    }
}