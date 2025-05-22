using System.Text;

namespace shared.Helpers;

public static class SeederHelpers
{
    private static readonly Random _random = new Random();

    public static string GetRandomParagraph(int minWords = 50, int maxWords = 150)
    {
        string[] sentences = new string[]
        {
            "Sơn chống thấm Kova CT-11A là lựa chọn hàng đầu cho mọi công trình, mang lại khả năng bảo vệ vượt trội khỏi ẩm mốc và rêu phong.",
            "Với công nghệ tiên tiến, sản phẩm sơn nội thất Dulux 5in1 giúp tường nhà bạn luôn bền đẹp, dễ dàng lau chùi và chống nấm mốc hiệu quả.",
            "Chống thấm Sika Latex TH được ứng dụng rộng rãi trong việc cải thiện chất lượng vữa và bê tông, tăng cường độ bám dính và khả năng chống thấm cho các hạng mục quan trọng.",
            "Sơn lót kháng kiềm Jotun Jotashield Primer là lớp nền hoàn hảo, giúp tăng cường độ bám dính của lớp sơn phủ và bảo vệ bề mặt khỏi các tác nhân gây hại từ kiềm.",
            "Để có một ngôi nhà đẹp và bền vững, việc chọn lựa vật liệu chống thấm phù hợp là vô cùng quan trọng, đảm bảo không gian sống luôn khô thoáng.",
            "Sơn Epoxy KCC ET5660 chuyên dụng cho sàn nhà xưởng, mang lại bề mặt cứng chắc, chống mài mòn và kháng hóa chất, đảm bảo môi trường làm việc an toàn.",
            "Chống thấm CT-11A Gold của Kova với công nghệ nano siêu thẩm thấu, bảo vệ tối ưu cho bề mặt bê tông, xi măng khỏi sự xâm nhập của nước.",
            "Việc thi công chống thấm đúng kỹ thuật sẽ giúp kéo dài tuổi thọ công trình, ngăn ngừa các vấn đề ẩm thấp, nấm mốc gây ảnh hưởng đến sức khỏe.",
            "Sơn ngoại thất Mykolor Grand mang đến bảng màu phong phú, độ bền màu cao và khả năng chống bám bẩn hiệu quả, giữ cho vẻ đẹp ngôi nhà luôn tươi mới.",
            "Giải pháp chống thấm tường ngoài bằng sơn Elastomeric là phương án hiệu quả, tạo màng co giãn chịu được sự dịch chuyển của kết cấu, tránh nứt nẻ."
        };

        int numSentences = _random.Next(2, 5); // 2-4 sentences
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < numSentences; i++)
        {
            sb.Append(sentences[_random.Next(sentences.Length)]);
            if (i < numSentences - 1)
            {
                sb.Append(" ");
            }
        }
        return sb.ToString();
    }

    public static string GetRandomShortDescription()
    {
        string[] descriptions = new string[]
        {
            "Sản phẩm cao cấp, bảo vệ tối ưu.",
            "Chống thấm hiệu quả, bền bỉ với thời gian.",
            "Màu sắc đa dạng, dễ dàng thi công.",
            "Giải pháp toàn diện cho mọi bề mặt.",
            "Công nghệ tiên tiến, an toàn cho người sử dụng.",
            "Tăng cường độ bám dính, chống rêu mốc.",
            "Đặc tính vượt trội, thân thiện môi trường.",
            "Phù hợp cho cả nội và ngoại thất.",
            "Che phủ hoàn hảo, bề mặt nhẵn mịn.",
            "Kháng kiềm, chống thấm nước tuyệt đối."
        };
        return descriptions[_random.Next(descriptions.Length)];
    }

    public static string GetRandomImageUrl(int width = 800, int height = 600)
    {
        return $"https://picsum.photos/seed/{Guid.NewGuid().GetHashCode()}/{width}/{height}";
    }
    public static string GetRandomThumbnailUrl(int width = 150, int height = 150)
    {
        return $"https://picsum.photos/seed/{Guid.NewGuid().GetHashCode()}/{width}/{height}";
    }
    public static string GetRandomAvatarUrl()
    {
        return $"https://i.pravatar.cc/150?img={_random.Next(1, 70)}";
    }

    public static DateTime GetRandomDateInPast(int daysAgoMin = 30, int daysAgoMax = 365)
    {
        return DateTime.Now.AddDays(-_random.Next(daysAgoMin, daysAgoMax));
    }
    public static DateTime GetRandomFutureDate(int daysFutureMin = 1, int daysFutureMax = 30)
    {
        return DateTime.Now.AddDays(_random.Next(daysFutureMin, daysFutureMax));
    }

    public static decimal GetRandomPrice(decimal min = 50000, decimal max = 5000000)
    {
        return Math.Round((decimal)(_random.NextDouble() * (double)(max - min) + (double)min) / 1000) * 1000;
    }

    public static string GenerateRandomEmail()
    {
        var domains = new[] { "gmail.com", "outlook.com", "yahoo.com", "example.com" };
        var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var usernameLength = _random.Next(5, 10);
        var username = new string(Enumerable.Repeat(chars, usernameLength)
          .Select(s => s[_random.Next(s.Length)]).ToArray());
        return $"{username}@{domains[_random.Next(domains.Length)]}";
    }

    public static string GenerateRandomPhone()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("0"); // Start with 0
        int[] prefixes = { 90, 91, 92, 93, 94, 96, 97, 98, 99, 32, 33, 34, 35, 36, 37, 38, 39, 70, 79, 77, 76, 78, 83, 84, 85, 81, 82 }; // Common Vietnamese prefixes
        sb.Append(prefixes[_random.Next(prefixes.Length)]);
        for (int i = 0; i < 7; i++) // 7 more digits for a 10 digit number
        {
            sb.Append(_random.Next(0, 10));
        }
        return sb.ToString();
    }
}

