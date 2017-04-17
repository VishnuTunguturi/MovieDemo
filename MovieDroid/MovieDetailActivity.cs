using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using MovieDemo.API;
using Square.Picasso;
using MovieDemo;
using Android.Graphics.Drawables;
using MovieDemo.Models.RequestModel;
using MovieDemo.Models;
using MovieDemo.Models.Param;
using Android.Net;
using MovieDemo.Interface;

namespace MovieDroid
{
	[Activity(Label = "Movie Explorer", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class MovieDetailActivity : Activity, RefreshCallback
	{
		RecyclerView.LayoutManager mLayoutManager;
		MovieListAdapter mAdapter;
		ProgressDialog progress;
		string Movie_ID = "";
		int cnt = 1;
		bool ismore = true;
		Button favbutton;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Movie_ID = Intent.GetStringExtra("Movie_ID");
			SetContentView(Resource.Layout.moviedetail);
			favbutton = FindViewById<Button>(Resource.Id.btnfavourite);
			favbutton.Click += Favbutton_Click;

			GetAllMovieDetails(Movie_ID);
		}

		/// <summary>
		/// Make Movie Favourite
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Favbutton_Click(object sender, EventArgs e)
		{
			try
			{
				if (isNetworkConnected())
				{
					progress = new ProgressDialog(this);
					progress.SetMessage("Loading...");
					progress.Show();
					FavouriteRequestModel frm = new FavouriteRequestModel();
					frm.media_id = Movie_ID;
					if (favbutton.Text == "Favourite")
						frm.favorite = true;
					else
						frm.favorite = false;
					ParamFavourite paramsBase = new ParamFavourite();
					paramsBase.sort_by = "";
					paramsBase.page = "";
					var result = await MovieAPI.PostMovieFavourite(frm, MovieAPI.Accountid, paramsBase);
					progress.Hide();
					if (favbutton.Text == "Favourite")
						favbutton.Text = "UnFavourites";
					else
						favbutton.Text = "Favourite";
					Toast.MakeText(this, result.status_message, ToastLength.Long).Show();
				}
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
				System.Diagnostics.Debug.WriteLine("Exception - " + msg);
			}
		}

		public override void OnBackPressed() { 
			AppConstant.isfavpage = true;
			Finish();
		}


		private async void GetAllMovieDetails(string Movie_ID)
		{
			if (isNetworkConnected())
			{
				progress = new ProgressDialog(this);
				progress.SetMessage("Loading...");
				progress.Show();

				Task syncTask1 = GetMovieDetail(Movie_ID);
				Task syncTask2 = GetSimilarMovies(Movie_ID);
				await Task.WhenAll(syncTask1, syncTask2);

				progress.Hide();
			}
		}

		/// <summary>
		/// Get Similar Movies
		/// </summary>
		/// <param name="movieid"></param>
		/// <returns></returns>
		private async Task GetSimilarMovies(string movieid, string page = "1")
		{
			try
			{
				if (ismore)
				{
					var mygalleryrated = FindViewById<RecyclerView>(Resource.Id.rv_similar);
					ParamsBase paramsBase = new ParamsBase();
					paramsBase.page = page;
					paramsBase.sort_by = "";
					var result = await MovieAPI.GetSimilerMovies(movieid, paramsBase);
					if (result.results.Count > 0)
					{
						if (result.total_pages == cnt)
						{
							ismore = false;
						}
						mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
						mygalleryrated.SetLayoutManager(mLayoutManager);
						if (cnt <= 1)
						{
							mAdapter = new MovieListAdapter(this, result.results, MovieListAdapter.SIM, this);
							mygalleryrated.SetAdapter(mAdapter);
						}
						else
						{
							mAdapter.mPhotoAlbum.AddRange(result.results);
							mAdapter.updateAdapter();
						}
					}
				}
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
				System.Diagnostics.Debug.WriteLine("Exception - " + msg);
			}

			cnt++;
		}

		/// <summary>
		/// Get Movie Deatils
		/// </summary>
		/// <param name="movieid"></param>
		/// <returns></returns>
		private async Task GetMovieDetail(string movieid)
		{
			try
			{
				var mygalleryrated = FindViewById<RecyclerView>(Resource.Id.rv_similar);
				ParamsBase paramsBase = new ParamsBase();
				paramsBase.sort_by = "";
				paramsBase.page = "";
				var result = await MovieAPI.GetMovieDetail(movieid, paramsBase);
				if (result != null)
				{
					var lbltitle = FindViewById<TextView>(Resource.Id.lbltitle);
					var lblreleasedate = FindViewById<TextView>(Resource.Id.lblreleasedate);
					var lbltotalrate = FindViewById<TextView>(Resource.Id.lbltotalrate);
					var lbldetail = FindViewById<TextView>(Resource.Id.lbldetail);
					var movieimage = FindViewById<ImageView>(Resource.Id.movieimage);
					var ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);
					lbltitle.Text = result.original_title;
					lblreleasedate.Text = "Release Date : " + result.release_date;
					lbltotalrate.Text = "(from " + result.vote_count + " votes)";
					lbldetail.Text = result.overview;
					ratingbar.Rating = (float)(result.vote_average / 2);
					Picasso.With(this)
					  .Load(AppConstant.ImagePath + result.poster_path)
					  .Placeholder(Resource.Drawable.placehoder)
					  .Into(movieimage);
				}
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
				System.Diagnostics.Debug.WriteLine("Exception - " + msg);
			}
		}

		/// <summary>
		/// Network Connected Check
		/// </summary>
		/// <returns></returns>
		private bool isNetworkConnected()
		{
			ConnectivityManager cm = (ConnectivityManager)GetSystemService(Context.ConnectivityService);
			NetworkInfo netInfo = cm.ActiveNetworkInfo;
			return netInfo != null && netInfo.IsConnectedOrConnecting;
		}

		/// <summary>
		/// Call Refresh Api 
		/// </summary>
		/// <param name="webserviceType"></param>
		public async void CallRefreshApi(int webserviceType)
		{
			await GetSimilarMovies(Movie_ID, cnt.ToString());
		}
	}
}