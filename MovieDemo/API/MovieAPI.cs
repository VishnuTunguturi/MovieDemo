using MovieDemo.Models;
using MovieDemo.Models.Param;
using MovieDemo.Models.RequestModel;
using MovieDemo.Models.ResponseModels.Post;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieDemo.API
{
    public class MovieAPI
    {

        public static string Accountid = "6536967"; 
        public static string SessionId = "2de39045e5ae5642c8bae4220514613a110e9eb9";
        private const string AppJSonWord = "application/json";
        private const string AppUrlencoded = "urlencoded";

        #region Get/Post aSync

        /// <summary>
        /// Get Data aSync
        /// </summary>
        /// <param name="locationURL"></param>
        /// <param name="queryURL"></param>
        /// <returns></returns>
        private static async Task<string> GetData(string queryURL, CancellationToken cancellationToken = new CancellationToken())
        {
            HttpResponseMessage response; 

            string result = string.Empty;

            string restQuery = AppConstant.serviceUrl + queryURL;

            HttpClient queryClient = null;
            queryClient = new HttpClient();
            

            queryClient.Timeout = TimeSpan.FromMinutes(5);

            //new Http message
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, restQuery);
                       
            //return JSON to the caller
            try
            {
                //call endpoint async
                response = await queryClient.SendAsync(request, cancellationToken);

                //read string result
                result = await response.Content.ReadAsStringAsync();  
                
            }
            catch (OperationCanceledException ex)
            {
                var msg = ex.Message;               
                System.Diagnostics.Debug.WriteLine("OperationCanceledException - " + msg);
            }
            catch (Exception ex)
            {
                var msg1 = ex.Message;                
            }            

            return result;
        }

        /// <summary>
        /// Post Data aSync
        /// </summary>
        /// <param name="locationURL"></param>
        /// <param name="queryURL"></param>
        /// <returns></returns>
        private static async Task<string> PostData(string queryURL, EntityModelBase objPost)
        {
            HttpResponseMessage response;

            string result = string.Empty;
            

            string restQuery = AppConstant.serviceUrl + queryURL;
            var LastQueryURL = restQuery;

            // HttpClient queryClient = new HttpClient();

            HttpClient queryClient = null;           
            queryClient = new HttpClient();

            queryClient.Timeout = TimeSpan.FromMinutes(5);
            // SerializeObject
            string postBody = JsonConvert.SerializeObject(objPost);

            // Body Content JSON Values
            StringContent bodyContent = new StringContent(postBody, Encoding.UTF8, AppJSonWord);

            try
            {             
                //call endpoint async
                response = await queryClient.PostAsync(restQuery, bodyContent);
                //read string result
                result = await response.Content.ReadAsStringAsync();               
            }
            catch (Exception ex)
            {
                return null;
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
            }
            return result;
        }

        #endregion

        #region GET API

        /// <summary>
        /// Get Top Rated Movies
        /// </summary>
        /// <param name="parambase"></param>
        /// <returns></returns>
        public static async Task<MoviesResponseModel> GetTopRatedMovies(ParamsBase parambase)
        {
            try
            {
                string url = "movie/top_rated";
                url += parambase.GetParamsJSON(parambase);
                var data = await GetData(url);
                var result = JsonConvert.DeserializeObject<MoviesResponseModel>(data);                
                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
                return null;
            }
        }

        /// <summary>
        /// Get Popular Movies
        /// </summary>
        /// <param name="parambase"></param>
        /// <returns></returns>
        public static async Task<MoviesResponseModel> GetPopularMovies(ParamsBase parambase)
        {
            try
            {
                string url = "movie/popular";
                url += parambase.GetParamsJSON(parambase);
                var data = await GetData(url);
                var result = JsonConvert.DeserializeObject<MoviesResponseModel>(data);
                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
                return null;
            }
        }

        /// <summary>
        /// Get Now Playin Movies
        /// </summary>
        /// <param name="parambase"></param>
        /// <returns></returns>
        public static async Task<MoviesResponseModel> GetNowPlayingMovies(ParamsBase parambase)
        {
            try
            {
                string url = "movie/now_playing";
                url += parambase.GetParamsJSON(parambase);
                var data = await GetData(url);
                var result = JsonConvert.DeserializeObject<MoviesResponseModel>(data);
                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
                return null;
            }
        }

        /// <summary>
        /// Get Favourites Movies
        /// </summary>
        /// <param name="parambase"></param>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public static async Task<MoviesResponseModel> GetFavouriteMovies(ParamsBase parambase,string AccountKey)
        {
            try
            {
                string url = "account/ "+ AccountKey + "/favorite/movies";
                url += parambase.GetParamsJSON(parambase);
                var data = await GetData(url);
                var result = JsonConvert.DeserializeObject<MoviesResponseModel>(data);
                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
                return null;
            }
        }

        /// <summary>
        /// Get Similer Movies
        /// </summary>
        /// <param name="movieid"></param>
        /// <param name="parambase"></param>
        /// <returns></returns>
        public static async Task<MoviesResponseModel> GetSimilerMovies(string movieid, ParamsBase parambase)
        {
            try
            {
                string url = "movie/ " + movieid + "/similar";
                url += parambase.GetParamsJSON(parambase);
                var data = await GetData(url);
                var result = JsonConvert.DeserializeObject<MoviesResponseModel>(data);
                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
                return null;
            }
        }

        /// <summary>
        /// Get Movie Detail
        /// </summary>
        /// <param name="movieid"></param>
        /// <param name="parambase"></param>
        /// <returns></returns>
        public static async Task<MovieDetailResponseModel> GetMovieDetail(string movieid, ParamsBase parambase)
        {
            try
            {
                string url = "movie/ " + movieid;
                url += parambase.GetParamsJSON(parambase);
                var data = await GetData(url);
                var result = JsonConvert.DeserializeObject<MovieDetailResponseModel>(data);
                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
                return null;
            }
        }

        #endregion

        #region POST API

        /// <summary>
        /// Make as Fovourite
        /// </summary>
        /// <param name="model"></param>
        /// <param name="AccountKey"></param>
        /// <param name="parambase"></param>
        /// <returns></returns>
        public static async Task<PostFavouroteResponseModel> PostMovieFavourite(FavouriteRequestModel model, string AccountKey, ParamFavourite parambase)
        {
            try
            {
                string url = "account/ " + AccountKey + "/favorite";
                url += parambase.GetParamsJSON();
                var data = await PostData(url, model);
                var result = JsonConvert.DeserializeObject<PostFavouroteResponseModel>(data);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
