using Data.Models.System;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Data.Hubs
{
    public class LocationHub : Hub
    {
        // Lưu trữ các kết nối máy khách
        private static readonly Dictionary<string, string> ConnectedClients = new();

        // Phương thức được gọi khi máy khách kết nối tới hub
        public override async Task OnConnectedAsync()
        {
            // Lưu trữ kết nối máy khách
            ConnectedClients[Context.ConnectionId] = Context.ConnectionId;

            await base.OnConnectedAsync();
        }

        // Phương thức được gọi khi máy khách ngắt kết nối tới hub
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Xóa kết nối máy khách
            ConnectedClients.Remove(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        // Phương thức được gọi khi máy khách gửi tọa độ vị trí mới
        public async Task SendLocation(TrackingModel model)
        {
            // Lấy tên kết nối của máy khách gửi tọa độ vị trí
            string clientConnectionId = Context.ConnectionId;

            // Lưu trữ tọa độ vị trí
            var tracking = JsonConvert.SerializeObject(model);
            ConnectedClients[clientConnectionId] = tracking;

            // Gửi thông báo về tọa độ vị trí mới cho các máy khách khác
            await Clients.All.SendAsync("ReceiveLocation", clientConnectionId, tracking);
        }
    }
}
