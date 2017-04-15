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
using Android.Support.V7.Widget;
using MovieDemo.Models;
using MovieDemo;
using Square.Picasso;
using MovieDemo.Interface;

namespace MovieDroid
{
    
    // VIEW HOLDER
    // Implement the ViewHolder pattern: each ViewHolder holds references
    // to the UI components (ImageView) 
    public class MovieViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        
        public MovieViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.iv_poster);

            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
    
    // ADAPTER
    // Adapter to connect the data set (Movies List) to the RecyclerView: 
    public class MovieListAdapter : RecyclerView.Adapter
    {
        // Event handler for item clicks:
        public event EventHandler<int> ItemClick;

        public List<MovieModel> mPhotoAlbum;

        private readonly Context context;
        public RefreshCallback mRefreshCallback;
        private bool isLoading = false;
        int movieListType = 0;
        public const int TOP = 1;
        public const int POPULAR = 2;
        public const int NOW = 3;
        public const int FAV = 4;
        public const int SIM = 5;


        // Load the adapter with the data set (Movies List) at construction time:
        public MovieListAdapter(Context _context,List<MovieModel> photoAlbum, int movieListType, RefreshCallback refreshCallback)
        {
            mPhotoAlbum = photoAlbum;
            context = _context;
            this.movieListType = movieListType;
            mRefreshCallback = refreshCallback;
        }
       
        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.layoutitem, parent, false);

            MovieViewHolder vh = new MovieViewHolder(itemView, OnClick);
            return vh;
        }

        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MovieViewHolder vh = holder as MovieViewHolder;            
            MovieModel mov = mPhotoAlbum[position];
            Picasso.With(context)
                    .Load(AppConstant.ImagePath + mov.poster_path)
                    .Placeholder(Resource.Drawable.placehoder)
                    .Into(vh.Image);

            if (!isLoading)
            {
                if (position == mPhotoAlbum.Count() - 2)
                {
                    isLoading = true;
                    mRefreshCallback.CallRefreshApi(movieListType);
                }
            }
        }

        public void updateAdapter()
        {
            isLoading = false;
            NotifyDataSetChanged();
        }
        
        // Return the number of movies available in the Movies List:
        public override int ItemCount
        {
            get { return mPhotoAlbum.Count; }
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                MovieModel mov = mPhotoAlbum[position];
                ItemClick(this, mov.id);
            }
        }
    }
}