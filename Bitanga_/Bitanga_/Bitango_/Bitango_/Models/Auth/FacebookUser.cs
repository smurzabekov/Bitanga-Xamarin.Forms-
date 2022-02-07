namespace Bitango_.Models
{
    //Facebook user for IOS Auth (Xamarin.Auth)
    public class FacebookUser
    {
        public class FacebookEmail
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Email { get; set; }
            public Picture Picture { get; set; }
            public override string ToString()
            {
                return $"Name: {First_Name} {Last_Name}, Email: {Email}, ID: {Id}";
            }
        }

        public class Picture
        {
            public Data Data { get; set; }
        }

        public class Data
        {
            public string Height { get; set; }
            public string Is_Silhouette { get; set; }
            public string Url { get; set; }
            public string Width { get; set; }
        }
    }
}
