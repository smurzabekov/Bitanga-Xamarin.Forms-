using Newtonsoft.Json;

namespace Bitango_.Models
{
    //Facebook user for Android Auth (Plugin.FacebookClient)
    public class FacebookProfile
    {
        public string Email { get; set; }
        public string Id { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}, Email: {Email}, ID: {Id}";
        }
    }
}
