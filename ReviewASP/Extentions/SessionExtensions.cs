using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//import two namespace
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ReviewASP.Extentions
{
    //static class dont need to be instantiated
    //Memory??? when is it accessible? Before the application starts up
    /*
    Session can store strings only, not complex objects
    SetObject
    Object > serializing object into Json String > saving that to session
    JSON String > Deserializing that into Object > Returning object
     */

    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {

            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }





    }
}
