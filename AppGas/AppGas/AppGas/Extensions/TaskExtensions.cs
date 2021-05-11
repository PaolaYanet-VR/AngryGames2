using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppGas.Extensions
{
    public static class TaskExtensions
    {
        //Extensión para un Task para poderlo invocar sin necesidad de la palabra reservada await
        public static async void SafeFireAndForget(this Task task,
                                                   bool returnToCallingContext,
                                                   Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }
            catch (Exception ex) when (onException != null)
            {

                onException(ex);
            }
        }
    }
}
