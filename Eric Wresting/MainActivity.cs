using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Eric_Wresting
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        Button reset;
        Button coinFlip;
        Button startWeigh;

        EditText myTeam;
        EditText opponent;        TextView myTeamMatch;        TextView opponentMatch;               TextView myTeamScore;        TextView oppScore;        TextView matchLeft;        TextView NeedWin;        TextView matchesLeftList;        TextView homeShow;        TextView awayShow;        RadioGroup myTeamGroup;        RadioGroup oppGroup;        RadioButton MTF;        RadioButton MTTF;        RadioButton MTMD;        RadioButton MTD;        RadioButton OF;        RadioButton OTF;        RadioButton OMD;        RadioButton OD;

        Spinner weighClasses;        MatchClass currentMatch;        string Team1 = "";
        string Team2 = "";
        string matchleftString = "";
        int previousMenu;
        bool inverseShowFirst = false;
        string showFirst = "ODD/EVEN";//false for home even,true for away even
        int startingWeightInt;
        bool largeScreen = false;
        MatchClass[] matchList = {new MatchClass(106,0),new MatchClass(113,1),new MatchClass(120,2),new MatchClass(126,3),new MatchClass(132,4),new MatchClass(138,5),new MatchClass(145,6),new MatchClass(152,7),new MatchClass(160,8),new MatchClass(170,9),new MatchClass(182,10),new MatchClass(195,11),new MatchClass(220,12),new MatchClass(285,13) };
        int[] weighClassList = {106, 113, 120, 126, 132, 138, 145, 152, 160, 170, 182, 195, 220, 285 };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            largeScreen = heightInDp > 800;
            if (heightInDp > 800)
            {
                
                //largeScreen = true;
            }
                previousMenu = Resource.Id.navigation_home;
                base.OnCreate(savedInstanceState);
                displayHomeLayout();           
        }
        protected override void OnPause()
        {
            base.OnPause();
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Unspecified;

        }
        protected override void OnResume()
        {
            base.OnResume();
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            if (previousMenu == Resource.Id.navigation_home)
            {
                EditText myTeam;
                EditText opponent;

                myTeam = FindViewById<EditText>(Resource.Id.myTeam);
                opponent = FindViewById<EditText>(Resource.Id.opp);
                Team1 = myTeam.Text;
                Team2 = opponent.Text;
            }
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    if (largeScreen)
                    {
                        displayHomeLayout();
                        previousMenu = Resource.Id.navigation_home;
                        return true;
                    }
                    displayHomeLayout();
                    previousMenu = Resource.Id.navigation_home;
                    return true;
                case Resource.Id.navigation_dashboard:
                    displayMatchesLayout();
                    previousMenu = Resource.Id.navigation_dashboard;
                    return true;
                case Resource.Id.navigation_notifications:
                    
                    displayNeededLayout();
                    previousMenu = Resource.Id.navigation_notifications;
                    return true;
            }
            return false;
        }
        ///////////////////////////////////////
        /// Sceen Layout methods
        ///////////////////////////////////////
        public void registerNavListener() {
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }

        //left screen
        public void displayHomeLayout() {
            
            SetContentView(Resource.Layout.activity_main);
            registerNavListener();
            initGUIHomeLayout();
            
        }
        
        //middle Screen        
        public void displayMatchesLayout() {
            SetContentView(Resource.Layout.matchs);
            registerNavListener();
            initGUIMatchesLayout();            
        }
        //Far right Screen
        public void displayNeededLayout() {
            SetContentView(Resource.Layout.needed);
            registerNavListener();
            initGUINeededLayout();
        }
        ///////////////////////////////////////
        /// GUI init methods
        ///////////////////////////////////////
        public void initGUIHomeLayout() {
            reset = FindViewById<Button>(Resource.Id.Reset);
            myTeam = FindViewById<EditText>(Resource.Id.myTeam);
            opponent = FindViewById<EditText>(Resource.Id.opp);
            coinFlip = FindViewById<Button>(Resource.Id.CoinFlip);
            startWeigh = FindViewById<Button>(Resource.Id.StartingWeight);

            coinFlip.Click += coinFlipOnClick;
            startWeigh.Click += startWeighOnClick;
            reset.Click += resetOnClick;            
            myTeam.Text = Team1;
            opponent.Text = Team2;
            if (startingWeightInt != 0)
            {
                startWeigh.Text = "Start Weigh:" + startingWeightInt.ToString();
                startWeigh.Clickable = !startWeigh.Clickable;
            }
            if (showFirst != "ODD/EVEN")
            {
                coinFlip.Clickable = !coinFlip.Clickable;
            }
            coinFlip.Text = showFirst;
        }       

        public void initGUIMatchesLayout()
        {
            myTeamGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            MTF = FindViewById<RadioButton>(Resource.Id.MTFall);
            MTTF = FindViewById<RadioButton>(Resource.Id.MTTechFall);
            MTMD = FindViewById<RadioButton>(Resource.Id.MTMajorDecision);
            MTD = FindViewById<RadioButton>(Resource.Id.MTDecision);
            OF = FindViewById<RadioButton>(Resource.Id.OFall);
            OTF = FindViewById<RadioButton>(Resource.Id.OTechFall);
            OMD = FindViewById<RadioButton>(Resource.Id.OMajorDecision);
            OD = FindViewById<RadioButton>(Resource.Id.ODecision);
            myTeamMatch = FindViewById<TextView>(Resource.Id.myTeamMatch);
            opponentMatch = FindViewById<TextView>(Resource.Id.oppMatch);
            oppGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup2);
            weighClasses = FindViewById<Spinner>(Resource.Id.weightClasses);
            matchesLeftList = FindViewById<TextView>(Resource.Id.MatchLeftlistClasses);
            homeShow = FindViewById<TextView>(Resource.Id.homeShowFirst);
            awayShow = FindViewById<TextView>(Resource.Id.awayShowFirst);
            
            MTF.Click += myTeamGroupOnClick;
            MTTF.Click += myTeamGroupOnClick;
            MTMD.Click += myTeamGroupOnClick;
            MTD.Click += myTeamGroupOnClick;
            OF.Click += oppGroupOnClick;
            OTF.Click += oppGroupOnClick;
            OMD.Click += oppGroupOnClick;
            OD.Click += oppGroupOnClick;

            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, weighClassList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            weighClasses.Adapter = adapter;

            myTeamMatch.Text = Team1;
            opponentMatch.Text = Team2;
            
            weighClasses.ItemSelected += weighClassesOnClick;
            //resets the current match to 106
            currentMatch = matchList[0];
            whoShowFirst();
            matchleftString = "";
            //get list for remaining matches
            for (int i = 0; i < matchList.Length; i++)
            {
                if (matchList[i].GetMatchResult() == 0)
                {
                    matchleftString += matchList[i].getWeighClass() + ", ";
                }

            }
            if (matchleftString != "")
            {
                matchleftString = matchleftString.Substring(0, matchleftString.Length - 2);
            }

            matchesLeftList.Text = matchleftString;
        }
        public void initGUINeededLayout()
        {
            int matchesLeft = 0;
            int MTPoint = 0;
            int OPoint = 0;
            for (int i = 0; i < matchList.Length; i++)
            {
                switch (matchList[i].GetMatchResult())
                {
                    case 0:
                        matchesLeft++;
                        break;
                    case 1:
                        MTPoint += 6;
                        break;
                    case 2:
                        MTPoint += 5;
                        break;
                    case 3:
                        MTPoint += 4;
                        break;
                    case 4:
                        MTPoint += 3;
                        break;
                    case 5:
                        OPoint += 6;
                        break;
                    case 6:
                        OPoint += 5;
                        break;
                    case 7:
                        OPoint += 4;
                        break;
                    case 8:
                        OPoint += 3;
                        break;
                }
            }
            
            myTeamMatch = FindViewById<TextView>(Resource.Id.Team1);
            opponentMatch = FindViewById<TextView>(Resource.Id.Team2);
            myTeamScore = FindViewById<TextView>(Resource.Id.myTeamScore);
            oppScore = FindViewById<TextView>(Resource.Id.oppScore);
            matchLeft = FindViewById<TextView>(Resource.Id.MatchesLeft);
            NeedWin = FindViewById<TextView>(Resource.Id.neededToWin);
            myTeamMatch.Text = Team1;
            opponentMatch.Text = Team2;

            matchLeft.Text = matchesLeft.ToString();
            NeedWin.Text = (MTPoint - OPoint).ToString();
            myTeamScore.Text = MTPoint.ToString();
            oppScore.Text = OPoint.ToString();
            if (MTPoint > (matchesLeft * 6) + OPoint)
            {
                //lock
                NeedWin.Text = "You Win";
                NeedWin.SetTextColor(Android.Graphics.Color.Green);
            }
            else
            if (MTPoint + (matchesLeft * 6) < OPoint)
            {
                // opp lock
                NeedWin.Text = "You Lose";
                NeedWin.SetTextColor(Android.Graphics.Color.Red);
            }
            else
            {
                NeedWin.Text = ((MTPoint + OPoint + (matchesLeft * 6)) / 2 + 1 - MTPoint).ToString();

            }
        }
        ///////////////////////////////////////
        /// Button Click methods
        ///////////////////////////////////////
        private void resetOnClick(object sender, EventArgs e)
        {
            opponent.Text = "";
            startWeigh.Clickable = true;
            coinFlip.Text = "ODD/EVEN";
            showFirst = "ODD/EVEN";
            startWeigh.Text = "Starting Weight";
            startingWeightInt = 0;
            for (int i = 0; i < matchList.Length; i++)
            {
                matchList[i].SetMatchResult(0);
            }
        }
        private void startWeighOnClick(object sender, EventArgs e)
        {
            Random rnd = new Random();
            startWeigh.Clickable = !startWeigh.Clickable;
            int tmp = rnd.Next(0, matchList.Length);
            startingWeightInt = matchList[tmp].getWeighClass();
            startWeigh.Text = "Start Weigh:" + startingWeightInt.ToString();
            if (tmp % 2 == 1)
            {
                inverseShowFirst = true;
            }
        }

        private void coinFlipOnClick(object sender, EventArgs e)
        {
            if (showFirst.Equals("ODD/EVEN"))
            {
                showFirst = "Home is Even";
            }
            else
            if (showFirst.Equals("Home is Even"))
            {
                showFirst = "Home is Odd";
            }
            else
            {
                showFirst = "ODD/EVEN";
            }
            coinFlip.Text = showFirst;
            
        }
        private void weighClassesOnClick(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            currentMatch = matchList[e.Position];
            myTeamGroup.ClearCheck();
            oppGroup.ClearCheck();
            switch (currentMatch.GetMatchResult())
            {
                case (0):
                    break;
                case 1:
                    MTF.Checked = true;
                    break;
                case 2:
                    MTTF.Checked = true;
                    break;
                case 3:
                    MTMD.Checked = true;
                    break;
                case 4:
                    MTD.Checked = true;
                    break;
                case 5:
                    OF.Checked = true;
                    break;
                case 6:
                    OTF.Checked = true;
                    break;
                case 7:
                    OMD.Checked = true;
                    break;
                case 8:

                    OD.Checked = true;
                    break;
            }
            whoShowFirst();
        }

        private void oppGroupOnClick(object sender, EventArgs e)
        {
            if (currentMatch.GetMatchResult() == 0)
            {
                removeFromMatchLeftString(currentMatch.weighClass);
            }
            myTeamGroup.ClearCheck();
            if (OF.Checked)
            {
                currentMatch.SetMatchResult(5);
                AdvanceWeighClasses();
                return;
            }
            if (OTF.Checked)
            {
                currentMatch.SetMatchResult(6);
                AdvanceWeighClasses();
                return;
            }
            if (OMD.Checked)
            {
                currentMatch.SetMatchResult(7);
                AdvanceWeighClasses();
                return;
            }
            if (OD.Checked)
            {
                currentMatch.SetMatchResult(8);
                AdvanceWeighClasses();
                return;
            }

        }

        private void myTeamGroupOnClick(object sender, EventArgs e)
        {
            if (currentMatch.GetMatchResult() == 0)
            {
                removeFromMatchLeftString(currentMatch.weighClass);
            }
            oppGroup.ClearCheck();
            if (MTF.Checked)
            {
                currentMatch.SetMatchResult(1);
                AdvanceWeighClasses();
                return;
            }
            if (MTTF.Checked)
            {
                currentMatch.SetMatchResult(2);
                AdvanceWeighClasses();
                return;
            }
            if (MTMD.Checked)
            {
                currentMatch.SetMatchResult(3);
                AdvanceWeighClasses();
                return;
            }
            if (MTD.Checked)
            {
                currentMatch.SetMatchResult(4);
                AdvanceWeighClasses();
                return;
            }

        }
        ///////////////////////////////////////
        /// General methods
        ///////////////////////////////////////
        public void removeFromMatchLeftString(int weight) {
            
            string[] tmp = matchleftString.Split(", ");
            matchleftString = "";
            for (int i = 0; i < tmp.Length; i++)
            {
                if (Convert.ToInt32(tmp[i]) != weight)
                {
                    matchleftString += tmp[i] + ", ";
                }

            }
            if (matchleftString != "")
            {
                matchleftString = matchleftString.Substring(0, matchleftString.Length - 2);
            }
            matchesLeftList = FindViewById<TextView>(Resource.Id.MatchLeftlistClasses);
            matchesLeftList.Text = matchleftString;
        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        public void AdvanceWeighClasses() {
            int pos = currentMatch.getIndexedLocation();
            if (pos == matchList.Length - 1)
            {
                pos = 0;
            }
            currentMatch = matchList[pos + 1];

            weighClasses.SetSelection(currentMatch.getIndexedLocation());
        }
        public void whoShowFirst() {
            if (showFirst == "Home is Odd")
            {
                if (weighClasses.SelectedItemPosition % 2 == 0)
                {
                    if (inverseShowFirst)
                    {
                        homeShow.Text = "";
                        awayShow.Text = "SHOW FIRST";
                    }
                    else
                    {
                        homeShow.Text = "SHOW FIRST";
                        awayShow.Text = "";
                    }
                }
                else
                {
                    if (inverseShowFirst)
                    {
                        homeShow.Text = "SHOW FIRST";
                        awayShow.Text = "";
                    }
                    else
                    {
                        homeShow.Text = "";
                        awayShow.Text = "SHOW FIRST";
                    }
                }
            }
            else
            {
                if (weighClasses.SelectedItemPosition % 2 != 0)
                {
                    if (inverseShowFirst)
                    {
                        homeShow.Text = "SHOW FIRST";
                        awayShow.Text = "";
                    }
                    else
                    {
                        homeShow.Text = "";
                        awayShow.Text = "SHOW FIRST";
                    }
                }
                else
                {
                    if (inverseShowFirst)
                    {
                        homeShow.Text = "";
                        awayShow.Text = "SHOW FIRST";
                    }
                    else
                    {
                        homeShow.Text = "SHOW FIRST";
                        awayShow.Text = "";
                    }
                }

            }
        }

        public void ShortToast(string message)
        {
            RunOnUiThread(delegate {
                Toast.MakeText(this, message, ToastLength.Short).Show();
            });
        }
        private void LongToast(string message)
        {
            RunOnUiThread(delegate {
                Toast.MakeText(this, message, ToastLength.Long).Show();
            });
        }
    }
}

public class MatchClass {
    //{ noResult, MTF, MTTF, MTMD, MTD, OF, OTF, OMD, OD };
    public int weighClass;
    int MR;
    int index = 0;
    public MatchClass(int weight,int index){
        weighClass = weight;
        this.index = index;
        MR = 0;
    }
    public int getWeighClass() {
        return weighClass;
    }
    public int GetMatchResult() {
        return(int) MR;
    }
    public void SetMatchResult(int Result)
    {
        MR = Result;
    }
    public int getIndexedLocation() {
        return index;
    }
}

