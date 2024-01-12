using AuthenticationEzBooking.DTO;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using AuthenticationEzBooking.SwaggerExamples;

public class ExamplesSchemaFilters : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(AuthDto))
        {
            schema.Example = new OpenApiObject
            {
                ["email"] = new OpenApiString(Examples.EmailExample),
                ["password"] = new OpenApiString(Examples.PasswordExample)
            };
        }

    }
}
