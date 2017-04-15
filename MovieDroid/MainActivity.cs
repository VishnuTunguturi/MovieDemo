using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using static Android.Widget.ImageView;
using System;
using Android.Views;
using System.Net;
using MovieDemo.API;
using MovieDemo;
using Android.Support.V7.Widget;
using Android.Content;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using MovieDemo.Models;
using MovieDemo.Models.Param;
using System.Collections.Generic;
using Android.Net;
using MovieDemo.Interface;
using Android.Support.V7.App;

namespace MovieDroid
{
    [Activity(Label = "Movie Explorer",Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity, RefreshCallback
    {
        MovieListAdapter mAdapterTOP;
        MovieListAdapter mAdapterPOP;
        MovieListAdapter mAdapterNOW;
        MovieListAdapter mAdapterFAV;
        ProgressDialog progress;
        Android.Support.V7.App.AlertDialog.Builder builder;
        Android.Support.V7.App.AlertDialog alert;
        int cnttop = 1;
        int cntpop = 1;
        int cntnow = 1; 
        int cntfav = 1;
        bool ismorerated = true;
        bool ismorepop = true;
        bool ismorefav = true;
        bool ismorenow = true;
        Android.Support.V7.Widget.Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            
            toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);     

            GetAllMovies();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.home_menu, menu); 
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: Search",ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Execute All (top rated, popular etc...)
        /// </summary>
        private async void GetAllMovies(string page = "1")
        {
            
            if (isNetworkConnected())
            {
                progress = new ProgressDialog(this);
                progress.SetMessage("Loading...");
                progress.Show();

                Task syncTask1 = GetTopRated(page);
                Task syncTask2 = GetPopular(page);
                Task syncTask3 = GetNowWaching(page);
                Task syncTask4 = GetFavourites(page);
                await Task.WhenAll(syncTask1, syncTask2, syncTask3, syncTask4);

                progress.Hide();
            }
            else
            {
                builder = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("Alert!")
                .SetMessage("Please connect internet")
                .SetPositiveButton("Ok", OnClickListener);
                alert = builder.Create();
                alert.Show();
            }
        }      

        /// <summary>
        /// Get Top Rated Movies
        /// </summary>
        /// <returns></returns>
        private async Task GetTopRated(string page)
        {
            try
            {
                if (ismorerated)
                {
                    var mygalleryrated = FindViewById<RecyclerView>(Resource.Id.rv_top_rated);
                    ParamsBase paramsBase = new ParamsBase();
                    paramsBase.page = page;
                    var result = await MovieAPI.GetTopRatedMovies(paramsBase);
                    if (result.results.Count > 0)
                    {
                        if (result.total_pages == cnttop)
                        {
                            ismorerated = false;
                        }
                        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                        mygalleryrated.SetLayoutManager(mLayoutManager);
                        if (cnttop <= 1)
                        {
                            mAdapterTOP = new MovieListAdapter(this, result.results, MovieListAdapter.TOP, this);
                            mygalleryrated.SetAdapter(mAdapterTOP);
                        }
                        else
                        {
                            mAdapterTOP.mPhotoAlbum.AddRange(result.results);
                            mAdapterTOP.updateAdapter();
                        }                      
                        mAdapterTOP.ItemClick += OnItemClick;
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
            }
            cnttop++;
        }

        /// <summary>
        /// Get Top Popular Movies
        /// </summary>
        /// <returns></returns>
        private async Task GetPopular(string page)
        {
            try
            {
                if (ismorepop)
                {
                    var mygalleryrated = FindViewById<RecyclerView>(Resource.Id.rv_popular);
                    ParamsBase paramsBase = new ParamsBase();

                    paramsBase.page = page;
                    var result = await MovieAPI.GetPopularMovies(paramsBase);
                    if (result.results.Count > 0)
                    {
                        if (result.total_pages == cntpop)
                        {
                            ismorepop = false;
                        }
                        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                        mygalleryrated.SetLayoutManager(mLayoutManager);
                        if (cntpop <= 1)
                        {
                            mAdapterPOP = new MovieListAdapter(this, result.results, MovieListAdapter.POPULAR, this);
                            mygalleryrated.SetAdapter(mAdapterPOP);
                        }
                        else
                        {
                            mAdapterPOP.mPhotoAlbum.AddRange(result.results);
                            mAdapterPOP.updateAdapter();
                        }
                      
                        mAdapterPOP.ItemClick += OnItemClick;
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
            }
            cntpop++;
        }

        /// <summary>
        /// Get Top NowWaching Movies
        /// </summary>
        /// <returns></returns>
        private async Task GetNowWaching(string page)
        {
            try
            {
                if (ismorenow)
                {
                    var mygalleryrated = FindViewById<RecyclerView>(Resource.Id.rv_now);
                    ParamsBase paramsBase = new ParamsBase();

                    paramsBase.page = page;
                    var result = await MovieAPI.GetNowPlayingMovies(paramsBase);
                    if (result.results.Count > 0)
                    {
                        if (result.total_pages == cntnow)
                        {
                            ismorenow = false;
                        }
                        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                        mygalleryrated.SetLayoutManager(mLayoutManager);
                        if (cntnow <= 1)
                        {
                            mAdapterNOW = new MovieListAdapter(this, result.results, MovieListAdapter.NOW, this);
                            mygalleryrated.SetAdapter(mAdapterNOW);
                        }
                        else
                        {
                            mAdapterNOW.mPhotoAlbum.AddRange(result.results);
                            mAdapterNOW.updateAdapter();
                        }                       
                        mAdapterNOW.ItemClick += OnItemClick;
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
            }
            cntnow++;
        }

        /// <summary>
        /// Get Top Rated Favourites
        /// </summary>
        /// <returns></returns>
        private async Task GetFavourites(string page)
        {
            try
            {
                var mylinearlayout = FindViewById<LinearLayout>(Resource.Id.ll_fav);
                if (ismorefav)
                {                    
                    var mygalleryrated = FindViewById<RecyclerView>(Resource.Id.rv_fav);
                    ParamFavourite paramsBase = new ParamFavourite();
                    paramsBase.page = page;
                    paramsBase.sort_by = "created_at.asc";
                    var result = await MovieAPI.GetFavouriteMovies(paramsBase, MovieAPI.Accountid);                    
                    if (result.results.Count > 0)
                    {
                        if (result.total_pages == cntfav)
                        {
                            ismorefav = false;
                        }
                        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                        mygalleryrated.SetLayoutManager(mLayoutManager);
                        if (cntfav <= 1)
                        {
                            mAdapterFAV = new MovieListAdapter(this, result.results, MovieListAdapter.FAV, this);
                            mygalleryrated.SetAdapter(mAdapterFAV);
                        }
                        else
                        {
                            mAdapterFAV.mPhotoAlbum.AddRange(result.results);
                            mAdapterFAV.updateAdapter();
                        }                        
                        mAdapterFAV.ItemClick += OnItemClick;
                    }
                    //  mylinearlayout.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception - " + msg);
            }
            cntfav++;
        }

        /// <summary>
        /// Movie image click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="position"></param>
        void OnItemClick(object sender, int position)
        {
            Intent intent = new Intent(this, typeof(MovieDetailActivity));
            intent.PutExtra("Movie_ID", position.ToString());
            this.StartActivity(intent);
        }

        /// <summary>
        /// Network Connection check
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
            switch (webserviceType)
            {
                case MovieListAdapter.TOP:
                    await GetTopRated(cnttop.ToString());
                    break;
                case MovieListAdapter.NOW:
                    await GetNowWaching(cntnow.ToString());
                    break;
                case MovieListAdapter.POPULAR:
                    await GetPopular(cntpop.ToString());
                    break;
                case MovieListAdapter.FAV:
                    await GetFavourites(cntfav.ToString());
                    break;
            }
        }
        
        private void OnClickListener(object sender, DialogClickEventArgs e)
        {
            alert.Hide();
            this.Finish();
            // System.exit(0);
        }
    }
}

