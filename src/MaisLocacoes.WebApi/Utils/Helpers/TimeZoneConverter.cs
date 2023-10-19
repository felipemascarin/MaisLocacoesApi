using Repository.v1.IRepository.UserSchema;
using System;
using System.Net;

namespace MaisLocacoes.WebApi.Utils.Helpers
{
    public class TimeZoneConverter<T>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TimeZoneConverter(ICompanyRepository companyRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> ConvertToTimeZoneLocal(T objectToConverter)
        {
            if (objectToConverter == null)
                return objectToConverter;

            var companyEntity = await _companyRepository.GetByCnpj(JwtManager.GetSchemaByToken(_httpContextAccessor)) ??
               throw new HttpRequestException("Empresa não encontrada para converter TimeZone", null, HttpStatusCode.NotFound);

            // Obtém o fuso horário local
            var localTimeZone = companyEntity.TimeZone;
            TimeZoneInfo customTimeZone = TimeZoneInfo.CreateCustomTimeZone("CustomTimeZone", TimeSpan.FromHours(localTimeZone), "Custom Time Zone", "Custom Time Zone");

            // Obtém o tipo do objeto
            Type objectType = typeof(T);

            // Verifica se o tipo do objeto é uma classe
            if (objectType.IsClass)
            {
                // Obtém as propriedades públicas do objeto
                var properties = objectType.GetProperties();

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(DateTime))
                    {
                        // Verifica se a propriedade é do tipo DateTime
                        DateTime originalValue = (DateTime)property.GetValue(objectToConverter);

                        // Converte o valor para o fuso horário local
                        DateTime localTime = TimeZoneInfo.ConvertTime(originalValue, customTimeZone);

                        // Define o novo valor da propriedade
                        property.SetValue(objectToConverter, localTime);
                    }
                }
            }

            return objectToConverter;
        }
    }
}