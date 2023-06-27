using Microsoft.AspNetCore.Mvc;

namespace com.mahonkin.tim.TeaApi.Exceptions
{
    public class InternalServerErrorObjectResult: ObjectResult
	{
        public InternalServerErrorObjectResult(object value) : base(value)
		{
            StatusCode = 500;
		}
	}
}

