using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Deploy.Server.Tests;

internal static class ActionResultExtensions
{
    // https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt
    public static T GetObjectResultContent<T>(this ActionResult<T> result) => (T)(result.Result as ObjectResult)!.Value!;

    // https://stackoverflow.com/questions/73594323/how-to-get-actionresult-statuscode-in-asp-net-core
    public static int? GetStatusCode<T>(this ActionResult<T?> result)
    {
        IConvertToActionResult convertToActionResult = result;

        var actionResultWithStatusCode = convertToActionResult.Convert() as IStatusCodeActionResult;

        return actionResultWithStatusCode?.StatusCode;
    }
} 
