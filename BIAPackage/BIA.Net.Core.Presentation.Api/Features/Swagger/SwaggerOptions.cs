namespace BIA.Net.Core.Presentation.Api.Features.Swagger
{
    public class SwaggerOptions
    {
        internal bool IsActive { get; private set; }

        public SwaggerOptions()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
