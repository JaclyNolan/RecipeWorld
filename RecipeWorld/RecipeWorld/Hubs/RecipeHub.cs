using Microsoft.AspNetCore.SignalR;

namespace RecipeWorld.Hubs
{
    public class RecipeHub : Hub
    {
        public async Task NotifyRecipeUpdated()
        {
            await Clients.All.SendAsync("ReceiveRecipeUpdate");
        }
    }
}
