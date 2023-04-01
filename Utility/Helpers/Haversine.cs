using System.Data.Entity;

namespace Utility.Helpers
{
    public static class Haversine
    {
        public static double Distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            //Biến "theta" đại diện cho góc giữa hai điểm trên mặt đất khi quan sát từ trung tâm của trái đất.

            //Trong công thức Haversine, các giá trị đầu vào và đầu ra được tính toán dưới đơn vị radian, 
            //do đó, các tọa độ đầu vào phải được chuyển đổi từ độ sang radian trước khi tính toán.

            double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            //Biến "dist" đại diện cho khoảng cách giữa hai điểm trên mặt đất dựa trên đường tròn lớn(great-circle distance) 
            //được tính bằng công thức Haversine.

            dist = Math.Acos(dist);
            //Lúc này biến "dist" chứa giá trị của hàm cosin của góc giữa hai điểm, tính theo độ.

            dist = Rad2Deg(dist);
            //Ta cần chuyển giá trị này thành radian, bằng cách sử dụng hàm Deg2Rad được định nghĩa trong code. 
            //Sau đó, giá trị này được sử dụng để tính toán khoảng cách giữa hai điểm trên mặt cầu.

            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;

            //Giá trị của "dist" tại điểm này là khoảng cách giữa hai điểm trên mặt cầu tính theo đơn vị radians, 
            //Để chuyển đổi khoảng cách này sang đơn vị miles.
            //Ta nhân với hệ số chuyển đổi từ radians sang degrees(60) và hệ số chuyển đổi từ degrees sang miles(1.1515).

            if (unit == 'K')
            {
                dist = dist * 1.609344;
                //Để chuyển đổi từ miles sang kilometers ta nhận "dist" với 1.609344
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
                //Để chuyển đổi từ miles sang nautical miles ta nhận "dist" với 0.8684
            }
            return dist;
        }

        //Trong hàm Distance, tham số unit được sử dụng để chỉ định đơn vị đo khoảng cách giữa hai điểm trên trái đất.
        //Tham số này có giá trị mặc định là 'K', tương ứng với đơn vị kilômét (km). Tham số 'N' tương ứng với đơn vị hải lý (nautical miles).

        //Vì vậy, nếu bạn muốn tính khoảng cách giữa hai điểm trong đơn vị kilômét,
        //bạn có thể gọi hàm Distance mà không cần truyền giá trị cho tham số unit, vì giá trị mặc định của unit đã là 'K'.

        //Nếu bạn muốn tính khoảng cách giữa hai điểm trong đơn vị hải lý,
        //bạn có thể truyền tham số 'N' cho tham số unit khi gọi hàm Distance.

        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        //Hai hàm Deg2Rad và Rad2Deg được sử dụng để chuyển đổi giữa đơn vị đo góc độ là độ(degree) và radian.
        //Hàm Deg2Rad được sử dụng để chuyển đổi giá trị độ sang radian, còn hàm Rad2Deg được sử dụng để chuyển đổi giá trị radian sang độ.
    }
}
